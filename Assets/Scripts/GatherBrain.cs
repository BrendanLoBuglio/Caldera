using UnityEngine;
using System.Collections;

public class GatherBrain : AnimalBrain {
	private AnimalStateMachine stateMachine;
	private float consumeTimer = 0f; //Timer to keep track of how long I've been drinking
	private GameObject resourceTarget; //The food or water source that the player is currently targeting	
	private AnimalBody body;
	private FoodMap foodMap;

	void Start () 
	{
		myState = BehaviorState.idle;
		stateMachine = gameObject.GetComponent<AnimalStateMachine>();
		body = gameObject.GetComponent<AnimalBody>();
		foodMap = GameObject.FindGameObjectWithTag("Map").GetComponent<FoodMap>();
	}
	
	
	void Update ()
	{
		//idle state behaviors:
		if(myState == BehaviorState.idle)
		{
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
			//It's important to note that FindTarget will change myState to pursue no matter which resource is chosen
			
		}
		
		//Pursuing state behaviors:
		if(myState == BehaviorState.pursue)
		{
			body.AIMove(resourceTarget.transform);
			
			//Choose a new target if yours is already eaten
			if(resourceTarget.GetComponent<Resource>().myType == ResourceType.food && !resourceTarget.GetComponent<FoodSource>().isGrown)
			{
				gameObject.SendMessage ("IfFoodTargetChanged", SendMessageOptions.DontRequireReceiver);
				FindTarget(ResourceType.food);
			}
		}
		
		//Consuming state behavior:
		if(myState == BehaviorState.consuming)
		{
			//Do nothing for now. I'll stick little eating animations in here
			consumeTimer += Time.deltaTime;
			if(consumeTimer >= stateMachine.eatTime && resourceTarget.CompareTag("Food"))
			{
				consumeTimer = 0f;
				myState = BehaviorState.idle;
			}
			if(consumeTimer >= stateMachine.drinkTime && resourceTarget.CompareTag ("Water"))
			{
				consumeTimer = 0f;
				myState = BehaviorState.idle;
			}
		}
	}
	
	public void FindTarget(ResourceType resource)
	{
		resourceTarget = foodMap.FindClosestResource(resource, transform.position);
		myState = BehaviorState.pursue;
	}
	
	void ResourceCollision(GameObject other) //Depends on a SendMessage Call from AnimalSensory
	{
		if(myState == BehaviorState.pursue && other == resourceTarget && other.GetComponent<Resource>() != null)
		{
			Resource resource = other.GetComponent<Resource>();
			resource.Consume(stateMachine);
			consumeTimer = 0f;
			myState = BehaviorState.consuming;
		}
	}
}
