using UnityEngine;
using System.Collections;

public class PlatformingPossesion : MonoBehaviour {

	public Vector2 jumpVelocity;
	
	private AnimalSensory sensory;
	private AnimalBody body;
	private AnimalBrain brain;
	private SmartJump jumpController;
	
	private float moveSpeed;
	private bool isInitialized = false;
	
	void Start () 
	{
		InitializeMe();
	}
	
	void InitializeMe()
	{
		sensory = gameObject.GetComponent<AnimalSensory>();
		
		body = gameObject.GetComponent<AnimalBody>();
		brain = gameObject.GetComponent<AnimalBrain>();
		jumpController = gameObject.GetComponent<SmartJump>();
		
		moveSpeed = body.moveSpeed;
		isInitialized = true;
	}
	
	
	void OnEnable () 
	{
		if(!isInitialized)
			InitializeMe();
	
		body.enabled = false;
		brain.enabled = false;
		jumpController.enabled = false;
	}
	void OnDisable ()
	{
		body.enabled = true;
		brain.enabled = true;
		jumpController.enabled = true;
	}
	
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
}
