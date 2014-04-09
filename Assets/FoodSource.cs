using UnityEngine;
using System.Collections;

public class FoodSource : Resource {

	public float nutritionValue = 10f;
	public float hydrationValue = 0f;
	public float regrowTime = 15f;
	private float regrowTimer = 0f;
	public bool isGrown = true;
	public const ResourceType myType = ResourceType.food;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
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
	
	public override void Consume(AnimalBrain consumer)
	{
		consumer.nutrition += nutritionValue;
		consumer.nutrition = Mathf.Clamp (consumer.nutrition, 0, 50f);
		isGrown = false;
		regrowTimer = 0f;
	}
}
