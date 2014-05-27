using UnityEngine;
using System.Collections;

public enum FlyingState{longRangeApproach, shortRangeApproach, idle}

public class FlyingAnimalBody : AnimalBody 
{
	public float longRangeMoveSpeed = 5f;
	public float shortRangeMoveSpeed = 6f;
	public float closeToFoodTurnSpeed = 15f;
	public float farFromFoodTurnSpeed = 5f;
	private float turnSpeed = 5f;
	public float heightAboveTarget = 8;
	private float targetDirection = 0f;
	private float direction = 90f;
	private AnimalBrain brain;
	public FlyingState myState = FlyingState.idle;
	private AnimalSensory sensory;
	private bool aiMoveWasCalled = false;
	
	void Start () 
	{
		brain = gameObject.GetComponent<AnimalBrain>();
		sensory = gameObject.GetComponent<AnimalSensory>();
	}
	
	void Update ()
	{	
		//Reset the direction to Up if you're not currently pursuing
		if(brain.myState != BehaviorState.pursue && brain.myState != BehaviorState.returnHome)
		{
			direction = 90f;
		}
		if(!aiMoveWasCalled)
		{
			myState = FlyingState.idle;
		}
		aiMoveWasCalled = false;
	}
	
	public override void AIMove(Transform target)
	{
		DetermineState(target);
		if(myState == FlyingState.longRangeApproach)
		{
			moveSpeed = longRangeMoveSpeed;
			turnSpeed = farFromFoodTurnSpeed;
			Vector2 targetEntryPoint = new Vector2 (target.position.x, target.position.y + heightAboveTarget);
			targetDirection = Mathf.Rad2Deg * Mathf.Atan2 (targetEntryPoint.y - transform.position.y, targetEntryPoint.x - transform.position.x);
			
			if(sensory.distanceFromGround <= 3f && Mathf.Sin (targetDirection * Mathf.Deg2Rad) < 0)
			{
				targetDirection = 90f;
			}			
		}
		else if(myState == FlyingState.shortRangeApproach)
		{
			moveSpeed = shortRangeMoveSpeed;
			turnSpeed = closeToFoodTurnSpeed;
			targetDirection = Mathf.Rad2Deg * Mathf.Atan2 (target.position.y - transform.position.y, target.position.x - transform.position.x); 
		}
		targetDirection = AccountForWalls(targetDirection);
		
		
		direction = Mathf.LerpAngle (direction, targetDirection, Time.deltaTime * turnSpeed);
		                                   
		float xMove = Mathf.Cos (direction*Mathf.Deg2Rad) * moveSpeed * Time.deltaTime;
		float yMove = Mathf.Sin (direction*Mathf.Deg2Rad) * moveSpeed * Time.deltaTime;
		Vector2 moveTo = new Vector2(xMove, yMove);
		transform.Translate (moveTo);
		
		aiMoveWasCalled = true;
	}
	
	void DetermineState(Transform target)
	//Determines whether the creature will approach with its Long-Range or Short-Range movement, based on how far away it is.
	//Long range moves toward a point above the target resource/animal's position, and has a slower turning speed.
	//Short range moves directly toward the target's position.
	{
		float distanceFrom = new Vector2 (target.position.x - transform.position.x, target.position.y - transform.position.y).magnitude;
		
		if(distanceFrom > heightAboveTarget + 1f && myState != FlyingState.shortRangeApproach)
		{
			myState = FlyingState.longRangeApproach;
		}
		else
		{
			myState = FlyingState.shortRangeApproach;
		}
	}
	
	float AccountForWalls(float targetDirectionIn)
	{
		bool tooCloseTop = false, tooCloseRight = false, tooCloseLeft = false;
		
		if(sensory.distanceFromCeiling <= 1.5)
			tooCloseTop = true;
		if(sensory.distanceFromRight <= 2 && sensory.distanceFromRight <= sensory.distanceFromLeft)
			tooCloseRight = true;
		else if(sensory.distanceFromLeft <= 2 && sensory.distanceFromRight > sensory.distanceFromLeft)
			tooCloseLeft = true;
		
		if(tooCloseTop && !tooCloseRight && !tooCloseLeft)
			targetDirectionIn = -90f;
		if(!tooCloseTop && tooCloseRight && !tooCloseLeft)
			targetDirectionIn = 135f;
		if(!tooCloseTop && !tooCloseRight && tooCloseLeft)
			targetDirectionIn = 45f;
		if(tooCloseTop && tooCloseRight && !tooCloseLeft)
			targetDirectionIn = -45f;
		if(tooCloseTop && !tooCloseRight && tooCloseLeft)
			targetDirectionIn = -135f;
			
		return targetDirectionIn;
	}
	
	void IfFoodTargetChanged() //Called by AnimalBrain when my food target is reselected (like when someone else eats it first)
	{
		if(myState == FlyingState.shortRangeApproach)
			myState = FlyingState.longRangeApproach;
	}
	
	/*public Vector2 ChooseWanderPointInAir()
	{
		Vector2 wanderPoint = (Vector2)transform.position + (Vector2.up * Random.Range(-2f, 2f)) + (Vector2.right * Random.Range (-2f, 2f));
		Vector2 distanceBetween = wanderPoint - (Vector2)transform.position;
		RaycastHit2D vectorOfTarget = Physics2D.Raycast(transform.position, distanceBetween.normalized, distanceBetween.magnitude, sensory.floorMask);
		if(Physics2D.Raycast(transform.position, distanceBetween.normalized, distanceBetween.magnitude, sensory.floorMask))
		{
			Debug.Log ("Collision at " + vectorOfTarget.point.x + ", " + vectorOfTarget.point.y + "!");
			wanderPoint = ChooseWanderPoint()InAir;
		}
		return wanderPoint; 
	}*/
}
