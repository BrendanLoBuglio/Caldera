using UnityEngine;
using System.Collections;

public class WaterSource : Resource {

	// Use this for initialization
	public float nutritionValue = 0f;
	public float hydrationValue = 100f;
	
	
	void Start () 
	{
		myType = ResourceType.water;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Consume(AnimalStateMachine consumer)
	{
		consumer.hydration += hydrationValue;
		consumer.hydration = Mathf.Clamp (consumer.hydration, 0, consumer.maximumHydration);
	}
}
