using UnityEngine;
using System.Collections;

public class DepletableWaterSource : Resource {
	
	// Use this for initialization
	public float nutritionValue = 0f;
	public float hydrationValue = 100f;
	public float drinkCapacity = 12f;
	public float drinksRemaining = 0f;
	private RainManager rain;
	private Animator animator;
	
	void Start () 
	{
		rain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RainManager>();
		animator = gameObject.GetComponent<Animator>();
	}
	
	
	void Update () 
	{
		if(rain.isRaining && drinksRemaining < drinkCapacity)
		{
			drinksRemaining = drinkCapacity;
		}
		
		animator.SetFloat ("drinksRemaining",drinksRemaining);
	}
	
	public override void Consume(AnimalStateMachine consumer, bool witherResource)
	{
		consumer.hydration += hydrationValue;
		consumer.hydration = Mathf.Clamp (consumer.hydration, 0, consumer.maximumHydration);
		drinksRemaining--;
	}
}
