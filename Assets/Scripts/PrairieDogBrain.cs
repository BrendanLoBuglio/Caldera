using UnityEngine;
using System.Collections;

public class PrairieDogBrain : GatherBrain 
{
	public override void ConversationIdleDecide() 
	{
		//Checkneeds will ususally change myState, so I have to check if I'm idle again again:
		if(myState == BehaviorState.idle && animalMap.CountIdleAnimals(AnimalType.prairieDog) >= 2 && stateMachine.myType == AnimalType.prairieDog) 
		{
			//Find a buddy to talk to
			pursueTarget = animalMap.FindClosestAnimal(AnimalType.prairieDog, gameObject, true, closeOrFarConversationAlternator);;
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
		if (pursueTarget.transform != null && pursueTarget.GetComponent<GatherBrain>() && pursueTarget.GetComponent<AnimalStateMachine>().myType == AnimalType.prairieDog)
		{
			animator.SetTrigger ("Converse");
			if(consumeTimer >= converseTime)
			{
				consumeTimer = 0f;
				myState = BehaviorState.idle;
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
