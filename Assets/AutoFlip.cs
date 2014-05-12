using UnityEngine;
using System.Collections;

public class AutoFlip : MonoBehaviour 
{
	private Vector2 oldPosition = Vector2.zero;
	private Vector2 newPosition = Vector2.zero;

	void Start () 
	{
		oldPosition = transform.position;
		newPosition = transform.position;
	}
	
	void Update () 
	{
		newPosition = transform.position;
		Vector2 difference = newPosition - oldPosition;
		
		if(difference.x > 0)
		{
			transform.localScale = new Vector3 (1f, transform.localScale.y, transform.localScale.z);
		}
		if(difference.x < 0)
		{
			transform.localScale = new Vector3 (-1f, transform.localScale.y, transform.localScale.z);
		}
		
		oldPosition = newPosition;
		
	}
}
