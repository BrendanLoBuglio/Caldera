using UnityEngine;
using System.Collections;

public enum JumpStage {NotJumping, SmartJumpVertical, SmartJumpHorizontal, DumbJump}

public class JumpController : MonoBehaviour 
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
	private JumpStage jumpStage = JumpStage.NotJumping;
	private AnimalSensory sensory;
	
	private Vector2 oldPosition = Vector2.zero;
	void Start()
	{
		sensory = gameObject.GetComponent<AnimalSensory>();
		gravityAcceleration = 9.8f * rigidbody2D.gravityScale;
		CalculateSmartJump();
	}
	void CalculateSmartJump()
	{
		relativeYExcess = yExcess * Mathf.Abs (positionDifference.x); //Scale yExcess to the x distance of the jump
		
		float yVel = Mathf.Sqrt (2f*gravityAcceleration*(positionDifference.y + relativeYExcess));
		jumpVelocityStage1 = new Vector2(0f, yVel);		
		
		float stage2Time = Mathf.Sqrt (2f*gravityAcceleration*relativeYExcess)/ (0.5f*gravityAcceleration);
		float xVel = positionDifference.x / stage2Time;
		jumpVelocityStage2 = new Vector2(xVel, 0);
	}
	void CalculateDumbJump()
	{
		float xVel = (gravityAcceleration * positionDifference.x) / Mathf.Sqrt(2f*gravityAcceleration*positionDifference.y);
		float yVel = Mathf.Sqrt(2f*gravityAcceleration * positionDifference.y);
		jumpVelocityStage1 = new Vector2 (xVel, yVel);
	}

	void Update()
	{
		if(Input.GetKey (KeyCode.Space) && !isJumping && debugMode)
		{
			SmartJump(positionDifference);
		}
		
		if(isJumping && jumpStage == JumpStage.SmartJumpVertical && transform.position.y - yBeforeJump >= positionDifference.y)
		{
			jumpVelocityStage2 = new Vector2(jumpVelocityStage2.x, rigidbody2D.velocity.y);
			rigidbody2D.velocity = jumpVelocityStage2;
			jumpStage = JumpStage.SmartJumpHorizontal;
		}
		if(isJumping && (jumpStage == JumpStage.SmartJumpHorizontal || jumpStage == JumpStage.DumbJump) && sensory.isGrounded)
		{
			rigidbody2D.velocity = Vector2.zero;
			jumpStage = JumpStage.NotJumping;
			isJumping = false; 
		}
		
		if(isJumping && jumpStage == JumpStage.DumbJump)
		{
			Debug.DrawLine(oldPosition, oldPosition + positionDifference, Color.magenta);
		}
	}
	public void SmartJump(Vector2 newPositionDifference)
	{
		positionDifference = newPositionDifference;
		isJumping = true;
		CalculateSmartJump();
		rigidbody2D.velocity = jumpVelocityStage1;
		yBeforeJump = transform.position.y;
		jumpStage = JumpStage.SmartJumpVertical;
	}
	public void DumbJump (Vector2 newPositionDifference)
	{
		oldPosition = transform.position;
		positionDifference = newPositionDifference;
		isJumping = true;
		CalculateDumbJump();
		rigidbody2D.velocity = jumpVelocityStage1;
		jumpStage = JumpStage.DumbJump;
	}
	
}
