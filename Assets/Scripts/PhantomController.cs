using UnityEngine;
using System.Collections;

public class PhantomController : MonoBehaviour 
{
	public bool isPossessing = false;
	public bool isFollowing = false;
	private GameObject potentialFauna;
	public GameObject possessedCreature;
	private PossesionController animalController;
	private AnimalStateMachine stateMachine;
	public LayerMask faunaMask;
	private HUD hud;
	
	private ParticleSystem possessParticles;
	private ParticleSystem followParticles;
	
	void Start()
	{
		hud = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<HUD>();
		possessParticles = transform.FindChild("Particles_Possess").gameObject.GetComponent<ParticleSystem>();
		followParticles = transform.FindChild("Particles_Follow").gameObject.GetComponent<ParticleSystem>();
		
		possessParticles.enableEmission = false;
		followParticles.enableEmission = false;
	}
	
	void Update()
	{
		CheckForCreature();
		
		if(isPossessing && possessedCreature.transform != null)
		{
			transform.position = possessedCreature.transform.position;
			renderer.enabled = false;
			possessParticles.enableEmission = true;
		}
		else if(isFollowing && possessedCreature.transform != null)
		{
			transform.position = possessedCreature.transform.position;
			renderer.enabled = false;
			followParticles.enableEmission = true;
		}
		else
		{
			renderer.enabled = true;
			possessParticles.enableEmission = false;
			followParticles.enableEmission = false;
		}
		
		if((isPossessing || isFollowing) && Input.GetKey (KeyCode.E))
		{
			DetachActor();
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
			
		if(!isPossessing && !isFollowing && Input.GetKey (KeyCode.Q) && potentialFauna.CompareTag("Fauna"))
		{
			possessedCreature = potentialFauna;
			stateMachine = possessedCreature.GetComponent<AnimalStateMachine>();
			animalController = possessedCreature.GetComponent<PossesionController>();
			animalController.enabled = true;
			isPossessing = true;
		}
		
		if(!isPossessing && !isFollowing && Input.GetKey (KeyCode.W) && potentialFauna.CompareTag("Fauna"))
		{
			possessedCreature = potentialFauna;
			transform.position = potentialFauna.transform.position;
			renderer.enabled = false;
			isFollowing = true;
		}
	}
	
	public void DetachActor()
	{
		if(isPossessing)
		{
			animalController.enabled = false;
			animalController = null;
		}
		isFollowing = false;
		isPossessing = false;
		possessedCreature = null;
	}
}
