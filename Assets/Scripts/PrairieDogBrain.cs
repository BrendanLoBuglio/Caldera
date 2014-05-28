using UnityEngine;
using System.Collections;

public class PrairieDogBrain : GatherBrain 
{
	public  float conversationRange = 3f;
	public float converseTime = 6f;
	public bool closeOrFarConversationAlternator = true; //randomly assigned to true or false

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
			Destroy(gameObject);
		}
	}
}
