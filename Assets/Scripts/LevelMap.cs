using UnityEngine;
using System.Collections;

public class LevelMap : MonoBehaviour {
	
	public int cameraX = 0;
	public int cameraY = 0;
	public Rect cameraBox;
	public GameObject trackedObject;
	private Vector2 offset;
	private Camera cam;
	
	private GameObject background;
	public float totalScreenHeights = 3f;
	private float backgroundIncrement;
	private Vector2 backgroundOffset;
	
	// Use this for initialization
	void Start () 
	{
		cam = gameObject.GetComponent<Camera>();
		float cameraHeight = cam.orthographicSize * 2f;
		float cameraWidth = cameraHeight * cam.aspect;
		cameraBox = new Rect(transform.position.x-(cameraWidth/2f),transform.position.y-(cameraHeight/2f), cameraWidth,cameraHeight);
				
		offset = new Vector2(transform.position.x, transform.position.y);
		
		/*
		background = GameObject.FindGameObjectWithTag("Background");
		backgroundOffset = new Vector2(background.transform.position.x - transform.position.x, background.transform.position.y - transform.position.y);
		float backgroundHeight = background.renderer.bounds.max.y - background.renderer.bounds.min.y;
		float excess = backgroundHeight - cameraBox.height;
		backgroundIncrement = excess / totalScreenHeights;
		*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(trackedObject != null)
		{
			//All the Mins and Maxes are relative to the camera position, which is at the center. 
			while(trackedObject.transform.position.x > cameraBox.xMax)
			{
				cameraX++;
				setCameraPosition();
			}
			while(trackedObject.transform.position.x < cameraBox.xMin)
			{
				cameraX--;
				setCameraPosition();
			}
			while(trackedObject.transform.position.y > cameraBox.yMax)
			{
				cameraY++;
				setCameraPosition();
			}
			while(trackedObject.transform.position.y < cameraBox.yMin)
			{
				cameraY--;
				setCameraPosition();
			}
		}
	}
	
	void setCameraPosition()
	{
		Vector3 cameraPosition = Vector2.zero;
		cameraPosition.x = cameraX * cameraBox.width + offset.x;
		cameraPosition.y = cameraY * cameraBox.height + offset.y;
		cameraPosition.z = -10f;
		transform.position = cameraPosition;
		
		cameraBox.x = cameraPosition.x - (cameraBox.width / 2f);
		cameraBox.y = cameraPosition.y - (cameraBox.height / 2f);
		
		//background.transform.position = new Vector3(cameraBox.x + backgroundOffset.x, cameraBox.y + backgroundOffset.y + (cameraY * backgroundIncrement), 100);
	}
}