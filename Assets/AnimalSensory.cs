using UnityEngine;
using System.Collections;

public class AnimalSensory : MonoBehaviour 
{

	[HideInInspector] public float height;
	public bool isGrounded = true;
	public float distanceFromGround = 0f;
	public float distanceFromLeft = 0f;
	public float distanceFromRight = 0f;
	public float distanceFromCeiling = 0f;
	public LayerMask resourceMask; //Layermask used to discriminate against all but resource collisions
	public LayerMask waypointMask; //Layermask used to discriminate against all but waypoint collisions
	public LayerMask floorMask; //LayerMask used to discriminate against all but collisions with the ground
	public LayerMask actorMask; //LayerMask used to discriminate against all but collisions with other actors
	
	public GameObject objectAbove;
	
	void Start () 
	{
		height = Mathf.Abs (renderer.bounds.min.y - renderer.bounds.max.y);
	}
	
	
	void Update () 
	{
		CheckResourceCollision();
		CheckWaypointCollision();
		CheckActorCollision();
		CheckIfGrounded();
		CheckDistanceFromGround();
		CheckDistanceFromWalls();
		CheckDistanceFromCeiling();
	}
	
	void CheckResourceCollision()
	{
		//Cast a ray to determine whether I've hit my targeted resource:
		Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + (height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, height, resourceMask.value);
		Vector2 drawHelper = new Vector2(rayOrigin.x, rayOrigin.y - height);
		Debug.DrawLine (rayOrigin,drawHelper);
		
		if(hit.transform != null)
		{
			GameObject other = hit.transform.gameObject;
			gameObject.SendMessage ("ResourceCollision", other, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void CheckWaypointCollision()
	{
		//Cast a ray to determine whether I've hit a waypoint on the map:
		Vector2 rayOrigin = new Vector2(transform.position.x + 0.05f, transform.position.y + (height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, height, waypointMask.value);
		Vector2 drawHelper = new Vector2(rayOrigin.x, rayOrigin.y - height);
		Debug.DrawLine (rayOrigin,drawHelper, Color.blue);
		
		if(hit.transform != null)
		{
			GameObject other = hit.transform.gameObject;
			gameObject.SendMessage ("WaypointCollision", other, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void CheckActorCollision()
	{
		//Cast a ray to determine whether I've hit an actor:
		Vector2 rayOrigin = new Vector2(transform.position.x - 0.05f, transform.position.y + (height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, height, actorMask.value);
		Vector2 drawHelper = new Vector2(rayOrigin.x, rayOrigin.y - height);
		Debug.DrawLine (rayOrigin,drawHelper, Color.green);
		
		if(hit.transform != null)
		{
			GameObject other = hit.transform.gameObject;
			gameObject.SendMessage ("ActorCollision", other, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	public void CheckIfGrounded()
	{
		//Cast a ray to determine whether I'm standing on the ground
		Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, (height / 20f), floorMask.value);
		Vector2 drawHelper = new Vector2(rayOrigin.x, rayOrigin.y - (height / 20f));
		Debug.DrawLine (rayOrigin,drawHelper, Color.red);
		
		if(hit.transform != null)
			isGrounded = true;
		else
			isGrounded = false;
	}
	
	public void CheckDistanceFromGround()
	{
		//Cast a ray to measure the distance between me and whatever I'm suspended above
		Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (height/2.0f));
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, 100f, floorMask.value);
		
		if(hit.transform != null)
		{
			Debug.DrawLine (rayOrigin,hit.point, Color.yellow);
			distanceFromGround = Vector2.Distance(rayOrigin, hit.point);
			objectAbove = hit.transform.gameObject;
		}
		else
		{
			Debug.DrawLine (rayOrigin, new Vector2(rayOrigin.x, rayOrigin.y - 100f), Color.yellow);
			distanceFromGround = 100f;
		}
	}
	
	public void CheckDistanceFromCeiling()
	{
		//Cast a ray to measure the distance between me and whatever's above me
		Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up, 6f, floorMask.value);
		
		if(hit.transform != null)
		{
			Debug.DrawLine (rayOrigin,hit.point, Color.yellow);
			distanceFromCeiling = Vector2.Distance(rayOrigin, hit.point);
		}
		else
		{
			Debug.DrawLine (rayOrigin, new Vector2(rayOrigin.x, rayOrigin.y + 6f), Color.yellow);
			distanceFromCeiling = 6f;
		}
	}
	
	public void CheckDistanceFromWalls()
	{
		//Cast a ray to measure the distance between me and the walls to my left and right
		
		//RIGHT RAY:
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right, 6f, floorMask.value);
		if(hit.transform != null)
		{
			Debug.DrawLine (transform.position,hit.point, Color.yellow);
			distanceFromRight = Vector2.Distance(transform.position, hit.point);
		}
		else
		{
			Debug.DrawLine (transform.position, new Vector2(transform.position.x + 6f, transform.position.y), Color.yellow);
			distanceFromRight = 6f;
		}
		
		//LEFT RAY:
		hit = Physics2D.Raycast (transform.position, -Vector2.right, 6f, floorMask.value);
		if(hit.transform != null)
		{
			Debug.DrawLine (transform.position,hit.point, Color.yellow);
			distanceFromLeft = Vector2.Distance(transform.position, hit.point);
		}
		else
		{
			Debug.DrawLine (transform.position, new Vector2(transform.position.x - 6f, transform.position.y), Color.yellow);
			distanceFromLeft = 6f;
		}
	}
}
