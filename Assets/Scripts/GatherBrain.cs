using UnityEngine;
using System.Collections;

public class GatherBrain : AnimalBrain 
{
	public AnimalStateMachine stateMachine;
	private Animator animator;
	public float consumeTimer = 0f; //Timer to keep track of how long I've been drinking
	public GameObject pursueTarget; //The food or water source that the player is currently targeting	
	private AnimalBody body;
	private FoodMap foodMap;
	private AnimalMap animalMap;
	
	public  float conversationRange = 3f;
	public float converseTime = 6f;
	
	private bool closeOrFarConversationAlternator = true;

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
			
			//Checkneeds will ususally change myState, so I have to check if I'm idle again again:
			if(myState == BehaviorState.idle && animalMap.CountIdleAnimals(AnimalType.prairieDog) >= 2 && stateMachine.myType == AnimalType.prairieDog) 
			{
				//Find a buddy to talk to
				pursueTarget = animalMap.FindClosestAnimal(AnimalType.prairieDog, gameObject, true, closeOrFarConversationAlternator);;
				pursueTarget.GetComponent<GatherBrain>().FriendRequest(gameObject);
				myState = BehaviorState.pursue;
				closeOrFarConversationAlternator = !closeOrFarConversationAlternator;
			}
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
			
			//Check if you're within friendlyRange of your friendyFriend
			if(pursueTarget.GetComponent<GatherBrain>())
			{
				if(Vector2.Distance (transform.position, pursueTarget.transform.position) <= conversationRange || Mathf.Abs(transform.position.x - pursueTarget.transform.position.x) <= 0.2f)
				{
					myState = BehaviorState.consuming;
					consumeTimer = 0f;
				}
			}
			
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
			
			if (pursueTarget.transform != null && pursueTarget.GetComponent<GatherBrain>() && pursueTarget.GetComponent<AnimalStateMachine>().myType == AnimalType.prairieDog)
			{
				animator.SetTrigger ("Converse");
				if(consumeTimer >= converseTime)
				{
					consumeTimer = 0f;
					myState = BehaviorState.idle;
				}
			}
			else if(pursueTarget.transform != null && pursueTarget.CompareTag("Food"))
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
	
	void CheckNeeds()
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
	
	public void FriendRequest(GameObject potentialFriend)
	{
		if(potentialFriend.GetComponent<AnimalBrain>().myState == BehaviorState.idle)
		{
			CheckNeeds();
			if(myState == BehaviorState.idle)
			{
				pursueTarget = potentialFriend;
				myState = BehaviorState.pursue;
			}
			else
			{
				Debug.Log ("I got a request, but I'm not squeaking idle!");
				potentialFriend.GetComponent<GatherBrain>().ClearFriends();
			}
		}
		else
		{
			Debug.Log ("I got a request, but my buddy isn't squeaking idle!");
			potentialFriend.GetComponent<GatherBrain>().ClearFriends();
		}
	}
	
	public void ClearFriends()
	{
		myState = BehaviorState.idle;
		pursueTarget = null;
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
	
	public void ActorDeath()
	{
		if(pursueTarget != null && pursueTarget.GetComponent<GatherBrain>())
		{
			pursueTarget.GetComponent<GatherBrain>().ClearFriends();
			Destroy(gameObject);
		}
	}
}
