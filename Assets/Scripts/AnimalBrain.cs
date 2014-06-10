using UnityEngine;
using System.Collections;

public enum BehaviorState {idle, pursue, consuming, returnHome, pursueStash}

public class AnimalBrain : MonoBehaviour 
{
	public BehaviorState myState;
}
