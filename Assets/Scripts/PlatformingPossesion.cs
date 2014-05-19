using UnityEngine;
using System.Collections;

public class PlatformingPossesion : PossesionController 
{
	public Vector2 jumpVelocity;
	
	void Update () 
	{
		float xMove = 0;
		float yMove = 0;
		
		if (Input.GetKey (KeyCode.LeftArrow))
			xMove = -moveSpeed * Time.deltaTime;
		else if (Input.GetKey (KeyCode.RightArrow))
			xMove = moveSpeed * Time.deltaTime;
		
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
		
		if(Input.GetKey(KeyCode.UpArrow) && sensory.isGrounded)
		{
			rigidbody2D.velocity = jumpVelocity;
		}
	}
	
	void ResourceCollision(GameObject other)
	{
		if(Input.GetKey (KeyCode.B))
		{
			Resource resource = other.GetComponent<Resource>();
			if(stateMachine.myType == AnimalType.turkey)
				resource.Consume(stateMachine, true);
			else
				resource.Consume(stateMachine, false);
		}
	}
	
	public void ActorDeath()
	{
		Debug.Log ("ActorDeath Called from PlatformingPossession!");
		brain.enabled = true;
		if(stateMachine.myType == AnimalType.prairieDog && gameObject.GetComponent<GatherBrain>().pursueTarget.GetComponent<GatherBrain>())
		{
			gameObject.GetComponent<GatherBrain>().pursueTarget.GetComponent<GatherBrain>().ClearFriends();
		}
		else
		{
			Debug.Log ("The statement was false!");
		}
		
		possesor.DetachActor();
		Destroy(gameObject);
	}
}
