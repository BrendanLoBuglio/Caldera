using UnityEngine;
using System.Collections;

public class FoodSource : Resource {

	public float nutritionValue = 10f;
	public float hydrationValue = 0f;
	public float regrowTime = 15f;
	private float regrowTimer = 0f;
	public bool isGrown = true;
	
	private Animator animator;

	void Start () 
	{
		animator = gameObject.GetComponent<Animator>();
		myType = ResourceType.food;
	}
	
	void Update () 
	{
		if(!isGrown)
			regrowTimer += Time.deltaTime;
		if (regrowTimer >= regrowTime)
		{
			isGrown = true;
			regrowTimer = 0f;
		}
	
		if(isGrown)
			animator.SetBool ("isGrown", true);
		else
			animator.SetBool ("isGrown", false);
	}
	
	public override void Consume(AnimalStateMachine consumer)
	{
		consumer.nutrition += nutritionValue;
		consumer.nutrition = Mathf.Clamp (consumer.nutrition, 0, consumer.maximumNutrition);
		isGrown = false;
		regrowTimer = 0f;
	}
}
