using UnityEngine;
using System.Collections;

public class FoodSource : Resource {

	public float nutritionValue = 10f;
	public float hydrationValue = 0f;
	public float regrowTime = 15f;
	private float regrowTimer = 0f;
	public bool isGrown = true; //Assuming that I'm an intact plant, have I grown a fruit?
	public bool isWithered = false; //If isWithered is true, I can't grow fruit until it rains and isWithered becomes false
	private Animator animator;

	void Start () 
	{
		animator = gameObject.GetComponent<Animator>();
		myType = ResourceType.food;
	}
	
	void Update () 
	{
		if(!isGrown && !isWithered)
			regrowTimer += Time.deltaTime;
		if (regrowTimer >= regrowTime && !isWithered)
		{
			isGrown = true;
			regrowTimer = 0f;
		}
	
		animator.SetBool ("isGrown", isGrown);
		animator.SetBool ("isWithered", isWithered);
	}
	
	public override void Consume(AnimalStateMachine consumer, bool witherResource)
	{
		consumer.nutrition += nutritionValue;
		consumer.nutrition = Mathf.Clamp (consumer.nutrition, 0, consumer.maximumNutrition);
		isGrown = false;
		regrowTimer = 0f;
		
		isWithered = witherResource;
	}
}
