using UnityEngine;
using System.Collections;

public enum BehaviorState {idle, pursue, consuming, returnHome}

public class AnimalBrain : MonoBehaviour 
{
	public BehaviorState myState;
}
