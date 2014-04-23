using UnityEngine;
using System.Collections;

public class PhantomMovement : MonoBehaviour {

	private PhantomController controller;
	private LevelMap tracker;
	public float speed = 5;

	void Start () 
	{
		controller = gameObject.GetComponent<PhantomController>();
		tracker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelMap>();
	}
	

	void Update () 
	{
		if(!controller.isPossessing)
		{
			float xMove = 0, yMove = 0;
		
			if(Input.GetAxis("Horizontal") > 0 && transform.position.x < tracker.cameraBox.xMax)
			{
				xMove = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
			}
			else if(Input.GetAxis ("Horizontal") < 0 && transform.position.x > tracker.cameraBox.xMin)
			{
				xMove = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
			}
			if(Input.GetAxis("Vertical") > 0 && transform.position.y < tracker.cameraBox.yMax)
			{
				yMove = Input.GetAxis ("Vertical") * speed * Time.deltaTime;
			}
			else if(Input.GetAxis ("Vertical") < 0 && transform.position.y > tracker.cameraBox.yMin)
			{
				yMove = Input.GetAxis ("Vertical") * speed * Time.deltaTime;
			}
				
			
			transform.Translate (new Vector2(xMove, yMove));
		}		
	}
}
