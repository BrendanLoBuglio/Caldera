using UnityEngine;
using System.Collections;

public class PrairieDogBrain : GatherBrain 
{
	public  float conversationRange = 3f;
	public float converseTime = 6f;
	public bool closeOrFarConversationAlternator = true; //randomly assigned to true or false
	public prairieDogBase myBase;
	public GameObject carriedFoodPrefab;
	private GameObject carriedFood;

	public override void PrairieBrainStart()
	{
		closeOrFarConversationAlternator = (Random.value > 0.5f);
	}

	public override void ConversationIdleDecide() 
	{
		//Checkneeds will ususally change myState, so I have to check if I'm idle again again:
		animalMap.PopulateIdlePrairieDogList();
		if(myState == BehaviorState.idle && animalMap.idlePrairieDogList.Count >= 2 && stateMachine.myType == AnimalType.prairieDog) 
		{
			//Find a buddy to talk to
			pursueTarget = animalMap.FindClosestAnimal(AnimalType.prairieDog, gameObject, true, closeOrFarConversationAlternator);;
			if(pursueTarget.GetComponent<AnimalBrain>().myState != BehaviorState.idle)
				Debug.Log("PursueTarget gave me a busy Prairie Dog!");
			pursueTarget.GetComponent<PrairieDogBrain>().FriendRequest(gameObject);
			myState = BehaviorState.pursue;
			closeOrFarConversationAlternator = !closeOrFarConversationAlternator;
		}
	}
	
	public override void ConversationPursueCheck()
	{	
		//Check if you're within friendlyRange of your friendyFriend
		if(pursueTarget.GetComponent<GatherBrain>())
		{
			if(Vector2.Distance (transform.position, pursueTarget.transform.position) <= conversationRange || Mathf.Abs(transform.position.x - pursueTarget.transform.position.x) <= 0.2f)
			{
				myState = BehaviorState.consuming;
				consumeTimer = 0f;
			}
		}
	}
	
	public override void ConversationConsume()
	{
		if (pursueTarget.transform != null && pursueTarget.GetComponent<PrairieDogBrain>())
		{
			animator.SetTrigger ("Converse");
			if(consumeTimer >= converseTime)
			{
				consumeTimer = 0f;
				idleTimer = 0f;
				myState = BehaviorState.idle;
				pursueTarget.GetComponent<PrairieDogBrain>().ClearFriends();
				ClearFriends();
			}
		}
	}
	
	public void FriendRequest(GameObject potentialFriend)
	{
		if(potentialFriend.GetComponent<AnimalBrain>().myState == BehaviorState.idle)
		{
			CheckNeeds();
			if(myState == BehaviorState.idle)
			{
				closeOrFarConversationAlternator = !closeOrFarConversationAlternator;
				pursueTarget = potentialFriend;
				myState = BehaviorState.pursue;
			}
			else
			{
				Debug.Log ("I got a request, but I'm not squeaking idle!");
				potentialFriend.GetComponent<PrairieDogBrain>().ClearFriends();
			}
		}
		else
		{
			Debug.Log ("I got a request, but my buddy isn't squeaking idle!");
			potentialFriend.GetComponent<PrairieDogBrain>().ClearFriends();
		}
	}
	
	public override void StashFood()
	{
		FindTarget(ResourceType.food, BehaviorState.pursueStash);
	}
	
	public override void StashCollisionCheck(GameObject other)
	{
		if(myState == BehaviorState.pursueStash && other == pursueTarget && other.GetComponent<Resource>() != null)
		{
			Resource resource = other.GetComponent<Resource>();
			resource.Stash(stateMachine);
			carriedFood = (GameObject)Instantiate(carriedFoodPrefab, transform.position, Quaternion.identity);
			pursueTarget = myBase.GetEmptyStoragePoint();
			myState = BehaviorState.returnHome;
		}
	}
	
	public override void ReturnStash()
	{
		if(myState == BehaviorState.returnHome)
		{
			if(pursueTarget.GetComponent<StoragePointSource>().isGrown == true)
			{
				if(myBase.hasEmptyStorage)
					pursueTarget = myBase.GetEmptyStoragePoint();
				else
				{
					Destroy(carriedFood);
					myState = BehaviorState.idle;
				}
			}
			carriedFood.transform.position = (Vector2)transform.position + Vector2.up;
			animator.SetTrigger ("Pursue");
			body.AIMove(pursueTarget.transform.position);
		}
	}
	
	public void HomeCollision(GameObject other)
	{
		if(myState == BehaviorState.returnHome && other == pursueTarget)
		{
			pursueTarget.GetComponent<StoragePointSource>().isGrown = true;
			pursueTarget = null;
			myState = BehaviorState.idle;
			Destroy(carriedFood);
			idleTimer = 0f;
			//Fill up the Prairie Dog's Tummy, so he doesn't eat the food he just stored. He only chooses to "store" when he's idle (and therefore 'full'), so this shouldn't have too warping an effect
			Mathf.Clamp (stateMachine.nutrition, (stateMachine.maximumNutrition * stateMachine.eatThreshold) + 10f, stateMachine.maximumNutrition);			
		}
	} 
	
	public void ClearFriends()
	{
		myState = BehaviorState.idle;
		idleTimer = 0f;
		pursueTarget = null;
	}
	
	public override void ActorDeath()
	{
		if(pursueTarget != null && pursueTarget.GetComponent<PrairieDogBrain>())
		{
			pursueTarget.GetComponent<PrairieDogBrain>().ClearFriends();
		}
		if(carriedFood != null)
			Destroy(carriedFood);
		Destroy(gameObject);
	}
}
