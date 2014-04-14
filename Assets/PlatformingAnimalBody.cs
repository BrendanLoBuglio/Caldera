using UnityEngine;
using System.Collections;

public enum PlatformingState {running, jumping}

public class PlatformingAnimalBody : AnimalBody 
{
	public bool goingLeft = false;
	public bool goingRight = false;
	public bool goingUp = false;
	public bool goingDown = false;
	private PlatformingState moveState;
	
	//private Rigidbody2D rigidbody;

	void Start()
	{
		moveSpeed = 5f; // The amount of unity units I move each frame
		//rigidbody = gameObject.GetComponent<Rigidbody2D>();
		moveState = PlatformingState.running;
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
		}
		
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
	}
	
	void CompareTargetPosition(Transform target)
	{
		Vector2 positionDifference = target.position - transform.position;
		
		if(positionDifference.x > 0)
		{
			goingRight = true;
		}
		if(positionDifference.x < 0)
		{
			goingLeft = true;
		}
		//5 and -5 are half the height of a "screen,"; Most of the terrain is pretty linear, but has small topographic variation,
		// so I only want him to be thinking about going "up" if he needs to move a substantial difference.
		if(positionDifference.y > 5)
		{
			goingUp = true;
		}
		if(positionDifference.y < -5)
		{
			goingDown = true;
		}
	}
	
	void WaypointCollision(GameObject other) //Receives a SendMessage call in AnimalBrain
	{
		if (other.CompareTag ("Waypoint") && moveState == PlatformingState.running)
		{
			Waypoint waypoint = other.GetComponent<Waypoint>();
			waypoint.Navigate(this);
		}
	}
	
	
	
	public void Jump (Vector2 jumpDistance)
	{
		rigidbody2D.velocity = jumpDistance;
		moveState = PlatformingState.jumping;
		
	}
}
