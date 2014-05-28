using UnityEngine;
using System.Collections;

public abstract class AnimalBody : MonoBehaviour 
{
	public float moveSpeed;
	public virtual void AIMove(Vector2 target) {}	
	public virtual bool AIHop(Vector2 target) {return false;} //Returns true if you've passed the target	
}
