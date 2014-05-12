using UnityEngine;
using System.Collections;

public class TurkeyBrain : GatherBrain 
{
	public override void ResourceCollision(GameObject other) //Depends on a SendMessage Call from AnimalSensory
	{
		if(myState == BehaviorState.pursue && other == pursueTarget && other.GetComponent<Resource>() != null)
		{
			Resource resource = other.GetComponent<Resource>();
			resource.Consume(stateMachine, true);
			consumeTimer = 0f;
			myState = BehaviorState.consuming;
		}
	}

}
