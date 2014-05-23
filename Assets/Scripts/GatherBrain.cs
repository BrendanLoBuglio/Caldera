using UnityEngine;
using System.Collections;

public class GatherBrain : AnimalBrain 
{
	[HideInInspector]public AnimalStateMachine stateMachine;
	[HideInInspector]public Animator animator;
	[HideInInspector]public AnimalBody body;
	[HideInInspector]public FoodMap foodMap;
	[HideInInspector]public AnimalMap animalMap;
	public float consumeTimer = 0f; //Timer to keep track of how long I've been drinking
	public GameObject pursueTarget; //The food or water source that the player is currently targeting	
	
	public  float conversationRange = 3f;
	public float converseTime = 6f;
	public bool closeOrFarConversationAlternator = true;
	
	void Start () 
	{
		myState = BehaviorState.idle;
		stateMachine = gameObject.GetComponent<AnimalStateMachine>();
		animator = gameObject.transform.FindChild("AnimatedChild").gameObject.GetComponent<Animator>();
		body = gameObject.GetComponent<AnimalBody>();
		foodMap = GameObject.FindGameObjectWithTag("Map").GetComponent<FoodMap>();
		animalMap = GameObject.FindGameObjectWithTag("Map").GetComponent<AnimalMap>();
	}
	
	void Update ()
	{
		//Idle state behaviors:
		if(myState == BehaviorState.idle)
		{
			animator.SetTrigger ("Idle");
			CheckNeeds();
			ConversationIdleDecide();
		}
		
		//Pursuing state behaviors:		
		if( myState == BehaviorState.pursue && pursueTarget == null)
		{
			myState = BehaviorState.idle;
		}
		
		if(myState == BehaviorState.pursue)
		{		
			animator.SetTrigger ("Pursue");
			body.AIMove(pursueTarget.transform);
			
			ConversationPursueCheck();
			
			//Choose a new target if you're targeting eaten/withered food
			if(pursueTarget.GetComponent<Resource>())
			{
				if(pursueTarget.GetComponent<Resource>().myType == ResourceType.food && !pursueTarget.GetComponent<FoodSource>().isGrown)
				{
					gameObject.SendMessage ("IfFoodTargetChanged", SendMessageOptions.DontRequireReceiver);
					FindTarget(ResourceType.food);
				}
			}
		}
		
		//Consuming state behavior:
		if(myState == BehaviorState.consuming)
		{
			//Do nothing for now. I'll stick little eating animations in here
			consumeTimer += Time.deltaTime;
			
			ConversationConsume();
			
			if(pursueTarget.transform != null && pursueTarget.CompareTag("Food"))
			{
				animator.SetTrigger ("Consume");
				if(consumeTimer >= stateMachine.eatTime)
				{
					consumeTimer = 0f;
					myState = BehaviorState.idle;
				}
			}
			else if(pursueTarget.transform != null && pursueTarget.CompareTag ("Water"))
			{
				animator.SetTrigger ("Consume");
				if(consumeTimer >= stateMachine.drinkTime )
				{
					consumeTimer = 0f;
					myState = BehaviorState.idle;
				}
			}
		}
	}
	
	public virtual void ConversationIdleDecide() {}
	public virtual void ConversationPursueCheck() {}
	public virtual void ConversationConsume() {}
	
	public void CheckNeeds()
	{
		//It's important to note that FindTarget will change myState to pursue no matter which resource is chosen
		bool needFood = false, needWater = false;
		//Check to see if my needs need attending to:
		if(stateMachine.nutrition <= stateMachine.eatThreshold * stateMachine.maximumNutrition)
			needFood = true;
		if(stateMachine.hydration <= stateMachine.drinkThreshold * stateMachine.maximumHydration)
			needWater = true;
		
		//Get whatever resource I need. If I need both, get whichever resource is lower
		if(needWater && !needFood)
			FindTarget(ResourceType.water);
		if(needFood && !needWater)
			FindTarget(ResourceType.food);
		if(needFood && needWater)
		{
			if(stateMachine.nutrition >= stateMachine.hydration)
				FindTarget(ResourceType.water);
			else
				FindTarget(ResourceType.food);
		}
	}
	
	public void FindTarget(ResourceType resource)
	{
		pursueTarget = foodMap.FindClosestResource(resource, transform.position);
		myState = BehaviorState.pursue;
	}
	
	public virtual void ResourceCollision(GameObject other) //Depends on a SendMessage Call from AnimalSensory
	{
		if(myState == BehaviorState.pursue && other == pursueTarget && other.GetComponent<Resource>() != null)
		{
			Resource resource = other.GetComponent<Resource>();
			resource.Consume(stateMachine, false);
			consumeTimer = 0f;
			myState = BehaviorState.consuming;
		}
	}
	
	public virtual void ActorDeath()
	{
		Destroy(gameObject);
	}
}
