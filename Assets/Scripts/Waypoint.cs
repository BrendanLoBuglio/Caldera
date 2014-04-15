using UnityEngine;
using System.Collections;

public enum AnimalNavigation {jump, turnAround, doNothing}

public class Waypoint : MonoBehaviour 
{
	public AnimalNavigation leftNav;
	public Vector2 leftJump;
	public AnimalNavigation rightNav;
	public Vector2 rightJump;
	
	public void Navigate(PlatformingAnimalBody animalBody)
	{
		if(animalBody.goingLeft)
			SendNavigateAction(leftNav, leftJump, animalBody);
		if(animalBody.goingRight)
			SendNavigateAction(rightNav, rightJump, animalBody);		
	}
	
	public void SendNavigateAction(AnimalNavigation nav, Vector2 jump, PlatformingAnimalBody animalBody)
	{	
		if(nav == AnimalNavigation.jump)
		{
			animalBody.NewJump(jump);
		}
	}
	
}
