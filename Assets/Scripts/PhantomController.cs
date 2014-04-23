using UnityEngine;
using System.Collections;

public class PhantomController : MonoBehaviour 
{
	public bool isPossessing = false;
	public GameObject possessedCreature;
	private PlatformingPossesion animalController;
	private LevelMap tracker;
	public LayerMask faunaMask;
	
	void Start()
	{
		tracker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LevelMap>();
	}
	
	void Update()
	{
		CheckForCreature();
		
		if(isPossessing && possessedCreature.transform != null)
		{
			transform.position = possessedCreature.transform.position;
			renderer.enabled = false;
		}
		else
		{
			renderer.enabled = true;
		}
		
		if(isPossessing && Input.GetKey (KeyCode.R))
		{
			isPossessing = false;
			possessedCreature = null;
			animalController.enabled = false;
			animalController = null;
			tracker.trackedObject = null;
		}
	}
	
	void CheckForCreature()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.5f, faunaMask.value);
		GameObject potentialFauna = gameObject;
		if(hit.transform != null)
			potentialFauna = hit.transform.gameObject;
			
		if(!isPossessing && Input.GetKey (KeyCode.E) && potentialFauna.CompareTag("Fauna"))
		{
			Debug.Log ("Successful Collision!");
			possessedCreature = potentialFauna;
			animalController = possessedCreature.GetComponent<PlatformingPossesion>();
			animalController.enabled = true;
			isPossessing = true;
			tracker.trackedObject = possessedCreature;
		}
	}
}
