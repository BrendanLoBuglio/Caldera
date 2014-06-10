using UnityEngine;
using System.Collections;

public class StoragePointSource : FoodSource 
{	
	public override void Start () 
	{
		animator = gameObject.GetComponent<Animator>();
		float nutritionValue = 10f;
		bool isGrown = false;
		myType = ResourceType.food;
	}
	
	public override void Update () 
	{
		animator.SetBool ("isGrown", isGrown);
	}
	
	public override void Consume(AnimalStateMachine consumer, bool witherResource)
	{
		consumer.nutrition += nutritionValue;
		consumer.nutrition = Mathf.Clamp (consumer.nutrition, 0, consumer.maximumNutrition);
		isGrown = false;
	}
	public override void Stash(AnimalStateMachine consumer)
	{
		Debug.Log ("Uh, a prairie Dog is trying to stash an already stored piece of food");
	}
}
