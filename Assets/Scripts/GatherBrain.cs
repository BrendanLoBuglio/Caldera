using UnityEngine;
using System.Collections;

public class GatherBrain : AnimalBrain 
{
	[HideInInspector]public AnimalStateMachine stateMachine;
	[HideInInspector]public Animator animator;
	[HideInInspector]public AnimalBody body;
	[HideInInspector]public FoodMap foodMap;
	[HideInInspector]public AnimalMap animalMap;
	public GameObject pursueTarget; //The food or water source that the player is currently targeting
	public float consumeTimer = 0f; //Timer to keep track of how long I've been drinking
	public float idleTime = 5f;
	public float idleTimer = 0f;
	private Vector2 wanderTarget;
	private bool needNewWanderTarget = true;
	
	void Start () 
	{
		myState = BehaviorState.idle;
		stateMachine = gameObject.GetComponent<AnimalStateMachine>();
		animator = gameObject.transform.FindChild("AnimatedChild").gameObject.GetComponent<Animator>();
		body = gameObject.GetComponent<AnimalBody>();
		foodMap = GameObject.FindGameObjectWithTag("Map").GetComponent<FoodMap>();
		animalMap = GameObject.FindGameObjectWithTag("Map").GetComponent<AnimalMap>();
		PrairieBrainStart(); //Only meaningful in the child class PrairieDogBrain
	}
	
	void Update ()
	{
		//Idle state behaviors:
		if(myState == BehaviorState.idle)
		{
			animator.SetTrigger ("Idle");
			CheckNeeds();
			
			if(idleTimer < idleTime)
			{
				if(needNewWanderTarget)
				{
					wanderTarget = new Vector2(transform.position.x + Random.Range(-4f, 4f), 0);
					needNewWanderTarget = false;
				}
				if(body.AIHop(wanderTarget))
				{
					//In addition to moving the actor with its hopping movement, AIHop evaluates to true when the actor has "passed" the target
					needNewWanderTarget = true;
				}
			}
			
			idleTimer += Time.deltaTime;
			if(idleTimer > idleTime)
			{
				bool randomizer = (Random.Range (0f, 1f) > 0.333f);
				if(randomizer)
					ConversationIdleDecide(); //Only meaningful in the child class PrairieDogBrain
				if(myState == BehaviorState.idle) //This triggers either if randomizer ends up false, or if I didn't get a conversation partner from ConversationIdleDecide()
					StashFood();			
			}
		}
		
		//Pursuing state behaviors:		
		if( myState == BehaviorState.pursue && pursueTarget == null)
		{
			myState = BehaviorState.idle;
			idleTimer = 0f;
		}
		
		if(myState == BehaviorState.pursue || myState == BehaviorState.pursueStash)
		{		
			animator.SetTrigger ("Pursue");
			body.AIMove(pursueTarget.transform.position);
			
			ConversationPursueCheck(); //Only meaningful in the child class PrairieDogBrain
			
			//Choose a new target if you're targeting eaten/withered food
			if(pursueTarget.GetComponent<Resource>())
			{
				if(pursueTarget.GetComponent<Resource>().myType == ResourceType.food && !pursueTarget.GetComponent<FoodSource>().isGrown)
				{
					gameObject.SendMessage ("IfFoodTargetChanged", SendMessageOptions.DontRequireReceiver);
					FindTarget(ResourceType.food, myState);
				}
			}
		}
		
		//Consuming state behavior:
		if(myState == BehaviorState.consuming)
		{
			//Do nothing for now. I'll stick little eating animations in here
			consumeTimer += Time.deltaTime;
			
			ConversationConsume(); //Only meaningful in the child class PrairieDogBrain
			
			if(pursueTarget != null && pursueTarget.CompareTag("Food"))
			{
				animator.SetTrigger ("Consume");
				if(consumeTimer >= stateMachine.eatTime)
				{
					consumeTimer = 0f;
					idleTimer = 0f;
					myState = BehaviorState.idle;
				}
			}
			else if(pursueTarget != null && pursueTarget.CompareTag ("Water"))
			{
				animator.SetTrigger ("Consume");
				if(consumeTimer >= stateMachine.drinkTime )
				{
					consumeTimer = 0f;
					idleTimer = 0f;
					myState = BehaviorState.idle;
				}
			}
		}
		ReturnStash(); //Only meaningful in the child class PrairieDogBrain
	}
	
	public virtual void PrairieBrainStart() {}
	public virtual void ConversationIdleDecide() {}
	public virtual void ConversationPursueCheck() {}
	public virtual void ConversationConsume() {}
	public virtual void StashFood() {}
	public virtual void StashCollisionCheck(GameObject other) {}
	public virtual void ReturnStash() {}
	
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
			FindTarget(ResourceType.water, BehaviorState.pursue);
		if(needFood && !needWater)
			FindTarget(ResourceType.food, BehaviorState.pursue);
		if(needFood && needWater)
		{
			if(stateMachine.nutrition >= stateMachine.hydration)
				FindTarget(ResourceType.water, BehaviorState.pursue);
			else
				FindTarget(ResourceType.food, BehaviorState.pursue);
		}
	}
	
	public void FindTarget(ResourceType resource, BehaviorState newState)
	{
		pursueTarget = foodMap.FindClosestResource(resource, transform.position, stateMachine.myType);
		myState = newState;
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
		
		StashCollisionCheck(other);
	}
	
	public virtual void ActorDeath()
	{
		Destroy(gameObject);
	}
}
