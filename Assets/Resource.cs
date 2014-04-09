﻿using UnityEngine;
using System.Collections;

public enum ResourceType {food, water}

public abstract class Resource : MonoBehaviour {

	public ResourceType myType;
	public virtual void Consume(AnimalBrain consumer)
	{
		//Whatever happens when I'm eaten	
	}
}
