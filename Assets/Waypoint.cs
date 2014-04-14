using UnityEngine;
using System.Collections;

public enum AnimalNavigation {jump, turnAround, doNothing}

public class Waypoint : MonoBehaviour 
{
	public AnimalNavigation leftNav;
	public Vector2 leftJump;
	public AnimalNavigation rightNav;
	public Vector2 rightJump;
	public AnimalNavigation upNav;
	public Vector2 upJump;
	public AnimalNavigation downNav;
	public Vector2 downJump;
	
	public void Navigate(PlatformingAnimalBody animalBody)
	{
		if(animalBody.goingLeft)
			SendNavigateAction(leftNav, leftJump, animalBody);
		if(animalBody.goingRight)
			SendNavigateAction(rightNav, rightJump, animalBody);
		if(animalBody.goingUp)
			SendNavigateAction(upNav, upJump, animalBody);
		if(animalBody.goingDown)
			SendNavigateAction(downNav, downJump, animalBody);
		
	}
	
	public void SendNavigateAction(AnimalNavigation nav, Vector2 jump, PlatformingAnimalBody animalBody)
	{
		//Scale down the jump velocities by Time.deltaTime;
		jump = new Vector2(jump.x * Time.deltaTime, jump.y * Time.deltaTime);
	
		if(nav == AnimalNavigation.jump)
		{
			animalBody.Jump(jump);
		}
	}
	
}
