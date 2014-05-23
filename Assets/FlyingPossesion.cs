using UnityEngine;
using System.Collections;

public class FlyingPossesion : PossesionController 
{
	public GameObject carryDogPrefab;
	private GameObject carriedPrey;
	private Vector2 carriedPreyOffset = Vector2.zero;
	public GameObject home;

	void Update () 
	{
		rigidbody2D.gravityScale = 0f;
	
		float xMove = 0;
		float yMove = 0;
		
		xMove = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		yMove = moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
		
		if(carriedPrey != null)
		{
			carriedPrey.transform.position = new Vector3(transform.position.x, transform.position.y + carriedPreyOffset.y, transform.position.z);
			
			if(sensory.objectAbove == home && sensory.distanceFromGround <= 1.5f)
			{
				stateMachine.nutrition += 30f;
				Destroy(carriedPrey);
				carriedPrey = null;
			}
		}
	}
	
	void ResourceCollision(GameObject other)
	{
		if(enabled)
		{
			if(Input.GetKey (KeyCode.B) && stateMachine.myType == AnimalType.basicBird)
			{
				Resource resource = other.GetComponent<Resource>();
				resource.Consume(stateMachine, false);
			}
		}
	}
	
	void ActorCollision(GameObject other)
	//A little note: I'd like to get all the prey carrying stuff into a separate PreyCarry Script, just so I don't have to replicate code between Hunterbrain and FlyingPossession, as I am now.
	{
		if(enabled)
		{
			if(stateMachine.myType == AnimalType.eagle)
			{
				if(carriedPrey == null && other.GetComponent<AnimalStateMachine>().myType == AnimalType.prairieDog)
				{
					carriedPrey = (GameObject) Instantiate(carryDogPrefab, other.transform.position, Quaternion.identity);
					carriedPreyOffset = carriedPrey.transform.position - transform.position;
					other.SendMessage("ActorDeath", SendMessageOptions.RequireReceiver);
				}
			}
		}
	}
}
