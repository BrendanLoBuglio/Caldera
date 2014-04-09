using UnityEngine;
using System.Collections;

public class WaterSource : Resource {

	// Use this for initialization
	public float nutritionValue = 0f;
	public float hydrationValue = 100f;
	public const ResourceType myType = ResourceType.water;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Consume(AnimalBrain consumer)
	{
		consumer.hydration += hydrationValue;
		consumer.hydration = Mathf.Clamp (consumer.hydration, 0, 50f);
	}
}
