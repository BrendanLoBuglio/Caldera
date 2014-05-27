using UnityEngine;
using System.Collections;

public class AnimalStateMachine : MonoBehaviour {

	public float hydration = 100f; //100 is full, 50 is hungry, 0 is dead
	public float nutrition = 100f; // 100 is full, 50 is thirsty, 0 is dead
	public float eatThreshold = 0.66f; //The nutrition value low enough to motivate me to eat (as a proportion of the maximum)
	public float drinkThreshold = 0.4f; //The hydration value low enough to motivate me to drink (as a proportion of the maximum)
	public float drinkTime = 1.5f; // The amount of time it takes me to drink
	public float eatTime = 0.8f; // The amount of time it takes me to eat.
	
	public AnimalType myType;
	
	[HideInInspector] public float maximumHydration;
	[HideInInspector] public float maximumNutrition;
	// Use this for initialization
	void Start () 
	{
		maximumHydration = hydration;
		maximumNutrition = nutrition;
		
		hydration = maximumHydration * Random.Range(drinkThreshold, 1f);
		nutrition = maximumNutrition * Random.Range (eatThreshold, 1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		nutrition -= Time.deltaTime;
		hydration -= Time.deltaTime;
		//Debug.Log ("ANIMAL STATE MACHINE IS OFF!!!!");
	}
}
