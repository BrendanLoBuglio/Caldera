using UnityEngine;
using System.Collections;

public class PhantomController : MonoBehaviour 
{
	public bool isPossessing = false;
	public bool isFollowing = true;
	private GameObject potentialFauna;
	public GameObject possessedCreature;
	private PossesionController animalController;
	private AnimalStateMachine stateMachine;
	private LevelMap tracker;
	public LayerMask faunaMask;
	private HUD hud;
	
	void Start()
	{
		tracker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelMap>();
		hud = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<HUD>();
	}
	
	void Update()
	{
		CheckForCreature();
		
		if(  (isPossessing && possessedCreature.transform != null))
		{
			transform.position = possessedCreature.transform.position;
			renderer.enabled = false;
		}
		else if(Input.GetKey (KeyCode.Q))
		{
			renderer.enabled = false;
		}
		
		else
		{
			renderer.enabled = true;
		}
		
		if(isPossessing && Input.GetKey (KeyCode.R))
		{
			DetachActor();
		}
		
		if(isFollowing && !Input.GetKey (KeyCode.Q))
		{
			tracker.trackedObject = null;
		}
		
		if(isPossessing && stateMachine != null)
		{
			hud.nutritionRatio = stateMachine.nutrition / stateMachine.maximumNutrition;
			hud.hydrationRatio = stateMachine.hydration / stateMachine.maximumHydration;
		}
	}
	
	void CheckForCreature()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.5f, faunaMask.value);
		potentialFauna = gameObject;
		if(hit.transform != null)
			potentialFauna = hit.transform.gameObject;
			
		if(!isPossessing && Input.GetKey (KeyCode.E) && potentialFauna.CompareTag("Fauna"))
		{
			Debug.Log ("Successful Collision!");
			possessedCreature = potentialFauna;
			stateMachine = possessedCreature.GetComponent<AnimalStateMachine>();
			animalController = possessedCreature.GetComponent<PossesionController>();
			animalController.enabled = true;
			isPossessing = true;
			tracker.trackedObject = possessedCreature;
		}
		
		if(!isPossessing && Input.GetKey (KeyCode.Q) && potentialFauna.CompareTag("Fauna"))
		{
			transform.position = potentialFauna.transform.position;
			renderer.enabled = false;
			isFollowing = true;
			tracker.trackedObject = potentialFauna;
		}
	}
	
	public void DetachActor()
	{
		isPossessing = false;
		possessedCreature = null;
		animalController.enabled = false;
		animalController = null;
		tracker.trackedObject = null;
	}
}
