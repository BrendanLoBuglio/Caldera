using UnityEngine;
using System.Collections;

public class SmartJump : MonoBehaviour 
{
	public bool debugMode = false;
	public Vector2 positionDifference;
	public float yExcess = 0.5f;
	[HideInInspector] public bool isJumping = false;
	private Vector2 jumpSpeed; // Changes depending on the required distance of the jump

	private float relativeYExcess;
	private Vector2 jumpVelocityStage1;
	private Vector2 jumpVelocityStage2;
	private float gravityAcceleration;
	private float yBeforeJump;
	private int jumpStage = 0;
	private AnimalBrain animalBrain;
	public LayerMask floorMask;
	[HideInInspector] public bool isGrounded;
	
	void Start()
	{
		CalculateJump();
		animalBrain = gameObject.GetComponent<AnimalBrain>();
	}
	void CalculateJump()
	{
		relativeYExcess = yExcess * Mathf.Abs (positionDifference.x); //Scale yExcess to the x distance of the jump
		gravityAcceleration = 9.8f * rigidbody2D.gravityScale;
		
		float yVel = Mathf.Sqrt (2f*gravityAcceleration*(positionDifference.y + relativeYExcess));
		jumpVelocityStage1 = new Vector2(0f, yVel);		
		
		float stage2Time = Mathf.Sqrt (2f*gravityAcceleration*relativeYExcess)/ (0.5f*gravityAcceleration);
		float xVel = positionDifference.x / stage2Time;
		jumpVelocityStage2 = new Vector2(xVel, 0);
	}

	void Update()
	{
		if(Input.GetKey (KeyCode.Space) && !isJumping && debugMode)
		{
			Jump(positionDifference);
		}
		
		if(isJumping && jumpStage == 1 && transform.position.y - yBeforeJump >= positionDifference.y)
		{
			jumpVelocityStage2 = new Vector2(jumpVelocityStage2.x, rigidbody2D.velocity.y);
			rigidbody2D.velocity = jumpVelocityStage2;
			jumpStage = 2;
		}
		CheckIfGrounded();
		if(isJumping && jumpStage == 2 && isGrounded)
		{
			rigidbody2D.velocity = Vector2.zero;
			jumpStage = 0;
			isJumping = false;
		}
		
	
	}
	public void Jump(Vector2 newPositionDifference)
	{
		positionDifference = newPositionDifference;
		isJumping = true;
		CalculateJump();
		rigidbody2D.velocity = jumpVelocityStage1;
		yBeforeJump = transform.position.y;
		jumpStage = 1;
	}
	
	public void CheckIfGrounded()
	{
		//Cast a ray to determine whether I'm standing on the ground
		Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (animalBrain.height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, (animalBrain.height / 20f), floorMask.value);
		Vector2 drawHelper = new Vector2(rayOrigin.x, rayOrigin.y - (animalBrain.height / 20f));
		Debug.DrawLine (rayOrigin,drawHelper, Color.red);
		
		if(hit.transform != null)
		{
			isGrounded = true;	
		}
		else
		{
			isGrounded = false;
		}
	}
}
