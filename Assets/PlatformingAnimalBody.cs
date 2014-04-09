using UnityEngine;
using System.Collections;

public class PlatformingAnimalBody : AnimalBody 
{

	void Start()
	{
		moveSpeed = 5f; // The amount of unity units I move each frame
	}
	
	
	public void AIMove(Transform target)
	{
		float direction = Mathf.Atan2 (target.position.y - transform.position.y, target.position.x - transform.position.x);
		float xMove = Mathf.Cos (direction) * moveSpeed * Time.deltaTime;
		float yMove = Mathf.Sin (direction) * moveSpeed * Time.deltaTime;
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
	}
}
