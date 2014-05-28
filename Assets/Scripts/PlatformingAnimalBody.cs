using UnityEngine;
using System.Collections;

public enum PlatformingState {running, jumping, hopping}

public class PlatformingAnimalBody : AnimalBody 
{
	public bool flyIntoTheSun = false;

	public bool goingLeft = false;
	public bool goingRight = false;
	private PlatformingState moveState;
	private JumpController jumpController;
	private AnimalSensory sensory;
	
	public Vector2 hopDistance = new Vector2(1, 0.3f);

	void Start()
	{
		sensory = gameObject.GetComponent<AnimalSensory>();
		moveSpeed = 5f; // The amount of unity units I move each frame		
		moveState = PlatformingState.running;
		jumpController = gameObject.GetComponent<JumpController>();
	}
	
	void Update()  
	{
		if(Input.GetKey (KeyCode.P))
		{
			jumpController.DumbJump(new Vector2(3f, 3f));
			Debug.Log ("I just fuckin' jumped, and my Velocity is now (" + rigidbody2D.velocity.x + "," + rigidbody2D.velocity.y + ")");
		}
		if(flyIntoTheSun)
		{
			rigidbody2D.velocity = new Vector2(0f, 99999f);
			Debug.Log ("I'm FLYYYYYYYYYYYYYYYYYYYYYING INTO THE SUN, BABY! (I'm fucking crazy)");
		}
	}
	
	public override void AIMove(Vector2 target)
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
			if(!jumpController.isJumping)
			{
				moveState = PlatformingState.running;
			}
		}
		
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
	}
	
	public override bool AIHop(Vector2 target)
	{
		//Returns true if you've passed the target
		if(goingRight && transform.position.x > target.x)
		{
			goingRight = false; //Set to false to account for when the wanderTarget moves to the other side of me
			return true;
		}
		else if (goingLeft && transform.position.x < target.x)
		{
			goingLeft = false; //Set to false to account for when the wanderTarget moves to the other side of me
			return true;
		}
		
			
		CompareTargetPosition(target);
		if(!jumpController.isJumping)
		{
			moveState = PlatformingState.running;
		}
		if(sensory.isGrounded && moveState == PlatformingState.running)
		{
			moveState = PlatformingState.jumping;
			jumpController.DumbJump(new Vector2(hopDistance.x * (goingRight ? 1f : -1f), hopDistance.y));
		}

			
		return false;
	}
	
	void CompareTargetPosition(Vector2 target)
	{
		Vector2 positionDifference = target - (Vector2)transform.position;
		
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
		if (other.CompareTag ("Waypoint") && sensory.isGrounded)
		{
			Waypoint waypoint = other.GetComponent<Waypoint>();
			waypoint.Navigate(this);
		}
	}
	
	public void WaypointJump (Vector2 jumpDistance)
	{
		if( sensory.isGrounded)
		{
			moveState = PlatformingState.jumping;
			jumpController.SmartJump(jumpDistance);
		}	
	}
}
