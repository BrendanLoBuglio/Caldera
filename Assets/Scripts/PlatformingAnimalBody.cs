using UnityEngine;
using System.Collections;

public enum PlatformingState {running, jumping}

public class PlatformingAnimalBody : AnimalBody 
{
	public bool goingLeft = false;
	public bool goingRight = false;
	private PlatformingState moveState;
	private SmartJump smartJump;
	private AnimalSensory sensory;

	void Start()
	{
		sensory = gameObject.GetComponent<AnimalSensory>();
		moveSpeed = 5f; // The amount of unity units I move each frame
		//rigidbody = gameObject.GetComponent<Rigidbody2D>();
		moveState = PlatformingState.running;
		smartJump = gameObject.GetComponent<SmartJump>();
	}
	
	
	public override void AIMove(Transform target)
	{
		CompareTargetPosition(target);
		
		float xMove = 0;
		float yMove = 0;
		
		if(moveState == PlatformingState.running)
		{
			if (goingLeft)
				xMove = -moveSpeed * Time.deltaTime;
			if (goingRight)
				xMove = moveSpeed * Time.deltaTime;
		}
		if(moveState == PlatformingState.jumping)
		{
			//I have a force on me, so I'm not doing much right now
			if(!smartJump.isJumping)
			{
				moveState = PlatformingState.running;
			}
		}
		
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
	}
	
	void CompareTargetPosition(Transform target)
	{
		Vector2 positionDifference = target.position - transform.position;
		
		if(positionDifference.x > 0)
			goingRight = true;
		else
			goingRight = false;
		if(positionDifference.x < 0)
			goingLeft = true;
		else
			goingLeft = false;
	}
	
	void WaypointCollision(GameObject other) //Receives a SendMessage call from AnimalBrain
	{
		if (other.CompareTag ("Waypoint") && moveState == PlatformingState.running)
		{
			Waypoint waypoint = other.GetComponent<Waypoint>();
			waypoint.Navigate(this);
		}
	}
	
	public void NewJump (Vector2 jumpDistance)
	{
		if(!smartJump.isJumping && sensory.isGrounded)
		{
			moveState = PlatformingState.jumping;
			smartJump.Jump(jumpDistance);
		}
		
	}
}
