using UnityEngine;
using System.Collections;

public class JumpTest : MonoBehaviour 
{
	
	public Vector2 positionDifference;
	public float yExcess = 0.5f;
	public bool isJumping = false;
	private Vector2 jumpSpeed; // Changes depending on the required distance of the jump

	private Vector2 jumpVelocityStage1;
	private Vector2 jumpVelocityStage2;
	private float gravityAcceleration;
	private float yBeforeJump;
	private float xBeforeStage2;
	private int jumpStage = 0;
	
	void Start()
	{
		gravityAcceleration = 9.8f * rigidbody2D.gravityScale;
	
		float yVel = Mathf.Sqrt (2f*gravityAcceleration*(positionDifference.y + yExcess));
		jumpVelocityStage1 = new Vector2(0f, yVel);		
		
		float stage2Time = Mathf.Sqrt (2f*gravityAcceleration*yExcess)/ (0.5f*gravityAcceleration);
		float xVel = positionDifference.x / stage2Time;
		jumpVelocityStage2 = new Vector2(xVel, 0);
	}

	void Update()
	{
		if(Input.GetKey (KeyCode.Space) && !isJumping)
		{
			isJumping = true;
			rigidbody2D.velocity = jumpVelocityStage1;
			yBeforeJump = transform.position.y;
			jumpStage = 1;
		}
		
		if(isJumping && jumpStage == 1 && transform.position.y - yBeforeJump >= positionDifference.y)
		{
			jumpVelocityStage2 = new Vector2(jumpVelocityStage2.x, rigidbody2D.velocity.y);
			rigidbody2D.velocity = jumpVelocityStage2;
			xBeforeStage2 = transform.position.x;
			jumpStage = 2;
		}
		if(isJumping && jumpStage == 2 && transform.position.x - xBeforeStage2 >= positionDifference.x)
		{
			rigidbody2D.velocity = Vector2.zero;
			jumpStage = 0;
			isJumping = false;
		}
		
	
	}
}
