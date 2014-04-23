using UnityEngine;
using System.Collections;

public class HunterBrain : AnimalBrain 
{
	private AnimalStateMachine stateMachine;
	private AnimalMap animalMap;
	
	public GameObject home;
	
	public GameObject carryDogPrefab;
	private GameObject carriedPrey;
	private Vector2 carriedPreyOffset = Vector2.zero;
	private GameObject animalTarget; //The food or water source that the player is currently targeting
	private FlyingAnimalBody body;
	private AnimalSensory sensory;
	public float consumeTime = 1.5f;
	private float consumeTimer = 0;
	
	void Start () 
	{
		myState = BehaviorState.idle;
		stateMachine = gameObject.GetComponent<AnimalStateMachine>();
		animalMap = GameObject.FindGameObjectWithTag("Map").GetComponent<AnimalMap>();
		body = gameObject.GetComponent<FlyingAnimalBody>();
		sensory = gameObject.GetComponent<AnimalSensory>();
	}
	
	
	void Update ()
	{
		if(myState == BehaviorState.idle || myState == BehaviorState.consuming)
			rigidbody2D.gravityScale = 1;
		else
			rigidbody2D.gravityScale = 0;
		
		//idle state behaviors:
		if(myState == BehaviorState.idle)
		{		
			bool needFood = false;
			if(stateMachine.nutrition <= stateMachine.eatThreshold * stateMachine.maximumNutrition)
				needFood = true;
			
			if(needFood)
			{
				FindTarget(AnimalType.prairieDog);
			}
		}
		
		//Pursuing state behaviors:
		if(myState == BehaviorState.pursue)
		{
			body.AIMove(animalTarget.transform);
		}
		
		//Returning state behavior:
		if(myState == BehaviorState.returnHome && carriedPrey != null)
		{
			carriedPrey.transform.position = (new Vector2(transform.position.x, transform.position.y) + carriedPreyOffset);
			body.AIMove (home.transform);
			
			if(sensory.objectAbove == home && sensory.distanceFromGround <= 1.5f)
			{
				myState = BehaviorState.consuming;
				consumeTimer = 0;
				Destroy(carriedPrey);
				stateMachine.nutrition += 30f;
			}
		}
		
		//Consuming state behavior:
		if(myState == BehaviorState.consuming)
		{
			//Do nothing for now. I'll stick little eating animations in here
			consumeTimer += Time.deltaTime;
			if(consumeTimer >= consumeTime)
			{
				consumeTimer = 0f;
				myState = BehaviorState.idle;
			}
		}
	}
	
	public void FindTarget(AnimalType animal)
	{
		animalTarget = animalMap.FindClosestAnimal(animal, transform.position);
		myState = BehaviorState.pursue;
	}
	
	void ActorCollision(GameObject other) //Depends on a SendMessage Call from AnimalSensory
	{
		if(myState == BehaviorState.pursue && other == animalTarget)
		{
			carriedPrey = (GameObject) Instantiate(carryDogPrefab, other.transform.position, Quaternion.identity);
			carriedPreyOffset = carriedPrey.transform.position - transform.position;
			Destroy(other);
			body.myState = FlyingState.idle;
			myState = BehaviorState.returnHome;
		}
	}
}