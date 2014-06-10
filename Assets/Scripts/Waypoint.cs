using UnityEngine;
using System.Collections;

public enum AnimalNavigation {jump, jumpIfReturning, doNothing, jumpIfNotReturning}

public class Waypoint : MonoBehaviour 
{
	public AnimalNavigation leftNav;
	public Vector2 leftJump;
	public AnimalNavigation rightNav;
	public Vector2 rightJump;
	public AnimalNavigation returningNav = AnimalNavigation.doNothing;
	public Vector2 returningJump;

	public void Navigate(PlatformingAnimalBody body)
	{
		bool doRegularJump = true;
		
		if(body.gameObject.GetComponent<PrairieDogBrain>())
		{
			PrairieDogBrain pBrain = body.gameObject.GetComponent<PrairieDogBrain>();
			if(pBrain.pursueTarget != null && pBrain.pursueTarget.GetComponent<StoragePointSource>())
			{
				//Is the Prairie Dog returning home, and is his home base within 27 (the length of the screen's diagonal) Units?
				if(returningNav == AnimalNavigation.jumpIfReturning && Vector2.Distance((Vector2)body.transform.position, pBrain.pursueTarget.transform.position) <= 27f)
				{
					SendNavigateAction(returningNav, returningJump, body);
					doRegularJump = false;
				}
			}
		}		
		if(doRegularJump)
		{
			if(body.goingLeft)
				SendNavigateAction(leftNav, leftJump, body);
			if(body.goingRight)
				SendNavigateAction(rightNav, rightJump, body);		
		}
	}
	
	public void SendNavigateAction(AnimalNavigation nav, Vector2 jump, PlatformingAnimalBody body)
	{	
		if(nav == AnimalNavigation.jump || nav == AnimalNavigation.jumpIfReturning)
		{
			body.WaypointJump(jump);
		}
		
		if(nav == AnimalNavigation.jumpIfNotReturning && (body.GetComponent<AnimalBrain>().myState == BehaviorState.pursue || body.GetComponent<AnimalBrain>().myState == BehaviorState.pursueStash ))
		{
			body.WaypointJump(jump);
		}
	}
	
}
