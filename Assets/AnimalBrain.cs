using UnityEngine;
using System.Collections;

public enum BehaviorState {idle, pursue, consuming}

public class AnimalBrain : MonoBehaviour {
	private BehaviorState myState = BehaviorState.idle;
	public float hydration = 50f; //100 is full, 50 is hungry, 0 is dead
	public float nutrition = 50f; // 100 is full, 50 is thirsty, 0 is dead
	private const float eatThreshold = 33f; //The nutrition value low enough to motivate me to eat
	private const float drinkThreshold = 20f; //The hydration value low enough to motivate me to drink
	public float drinkTime = 1.5f; // The amount of time it takes me to drink
	public float eatTime = 0.8f; // The amount of time it takes me to eat.
	
	private float consumeTimer = 0f; //Timer to keep track of how long I've been drinking
	private GameObject resourceTarget; //The food or water source that the player is currently targeting
	
	private float height; //My Height
	public LayerMask resourceMask; //Layermask used to discriminate against all but resource collisions
	public LayerMask waypointMask; //Layermask used to discriminate against all but waypoint collisions
	
	private AnimalBody body;
	private FoodMap foodMap;

	void Start () 
	{
		foodMap = GameObject.FindGameObjectWithTag("Map").GetComponent<FoodMap>();
		body = gameObject.GetComponent<AnimalBody>();
		height = Mathf.Abs (renderer.bounds.min.y - renderer.bounds.max.y);
	}
	
	
	void Update ()
	{
		nutrition -= Time.deltaTime;
		hydration -= Time.deltaTime;
	
		//idle state behaviors:
		if(myState == BehaviorState.idle)
		{
			bool needFood = false, needWater = false;
			//Check to see if my needs need attending to:
			if(nutrition <= eatThreshold)
				needFood = true;
			if(hydration <= drinkThreshold)
				needWater = true;
			
			//Get whatever resource I need. If I need both, get whichever resource is lower
			if(needWater && !needFood)
				FindTarget(ResourceType.water);
			if(needFood && !needWater)
				FindTarget(ResourceType.food);
			if(needFood && needWater)
			{
				if(nutrition >= hydration)
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
			
			//Choose a new target if yours is already eaten food
			if(resourceTarget.GetComponent<Resource>().myType == ResourceType.food && !resourceTarget.GetComponent<FoodSource>().isGrown)
			{
				FindTarget(ResourceType.food);
			}
			
			CheckResourceCollision();
			CheckWaypointCollision();
		}
		
		//Consuming state behavior:
		if(myState == BehaviorState.consuming)
		{
			//Do nothing for now. I'll stick little eating animations in here
			consumeTimer += Time.deltaTime;
			if(consumeTimer >= eatTime && resourceTarget.CompareTag("Food"))
			{
				consumeTimer = 0f;
				myState = BehaviorState.idle;
			}
			if(consumeTimer >= drinkTime && resourceTarget.CompareTag ("Water"))
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
	
	void CheckResourceCollision()
	{
		//Cast a ray to determine whether I've hit my target:
		Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + (height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, height, resourceMask.value);
		Vector2 drawHelper = new Vector2(rayOrigin.x, rayOrigin.y - height);
		Debug.DrawLine (rayOrigin,drawHelper);
		
		if(hit.transform != null)
		{
			GameObject other = hit.transform.gameObject;
			
			if(myState == BehaviorState.pursue && other == resourceTarget && other.GetComponent<Resource>() != null)
			{
				Resource resource = other.GetComponent<Resource>();
				resource.Consume(this);
				consumeTimer = 0f;
				myState = BehaviorState.consuming;
			}
			else
			{
				Debug.Log ("Haha, you're fucked!");
			}
		}
	}
	
	void CheckWaypointCollision()
	{
		//Cast a ray to determine whether I've hit a waypoint on the map:
		Vector2 rayOrigin = new Vector2(transform.position.x + 0.05f, transform.position.y + (height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, height, waypointMask.value);
		Vector2 drawHelper = new Vector2(rayOrigin.x, rayOrigin.y - height);
		Debug.DrawLine (rayOrigin,drawHelper, Color.blue);
		
		if(hit.transform != null)
		{
			GameObject other = hit.transform.gameObject;
			gameObject.SendMessage ("WaypointCollision", other);
		}
	}
}
