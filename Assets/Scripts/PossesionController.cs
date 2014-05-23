using UnityEngine;
using System.Collections;

public class PossesionController : MonoBehaviour 
{
	[HideInInspector]public AnimalSensory sensory;
	[HideInInspector]public AnimalBody body;
	[HideInInspector]public AnimalBrain brain;
	[HideInInspector]public PhantomController possesor;
	[HideInInspector]public AnimalStateMachine stateMachine;
	public bool isInitialized = false;
	public float moveSpeed;
	[HideInInspector]public SmartJump jumpController;
	
	void Start () 
	{
		InitializeMe();
	}
	
	public void InitializeMe()
	{
		sensory = gameObject.GetComponent<AnimalSensory>();
		body = gameObject.GetComponent<AnimalBody>();
		brain = gameObject.GetComponent<AnimalBrain>();
		stateMachine = gameObject.GetComponent<AnimalStateMachine>();
		possesor = GameObject.FindGameObjectWithTag("Player").GetComponent<PhantomController>();
		moveSpeed = body.moveSpeed;
		isInitialized = true;
		
		if(stateMachine.myType == AnimalType.prairieDog)
			jumpController = gameObject.GetComponent<SmartJump>();
	}
	
	void OnEnable () 
	{
		if(!isInitialized)
			InitializeMe();
		
		if(stateMachine.myType == AnimalType.prairieDog && gameObject.GetComponent<GatherBrain>().pursueTarget.GetComponent<GatherBrain>())
		{
			gameObject.GetComponent<PrairieDogBrain>().ClearFriends();
		}
		body.enabled = false;
		brain.enabled = false;
		if(stateMachine.myType == AnimalType.prairieDog)
			jumpController.enabled = false;
	}
	void OnDisable ()
	{
		body.enabled = true;
		brain.enabled = true;
		if(stateMachine.myType == AnimalType.prairieDog)
			jumpController.enabled = true;
	}
	
}
