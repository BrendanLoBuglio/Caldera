using UnityEngine;
using System.Collections;

public enum ResourceType {food, water}

public abstract class Resource : MonoBehaviour {

	public ResourceType myType;
	public virtual void Consume(AnimalStateMachine consumer, bool witherResource)
	{
		//Whatever happens when I'm eaten	
	}
	public virtual void Stash(AnimalStateMachine consumer)
	{
		//Whatever happens when an animal decides to bring me home. Only meaningful for food.
	}
}
