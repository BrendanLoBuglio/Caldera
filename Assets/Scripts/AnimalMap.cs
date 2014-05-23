using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AnimalType {prairieDog, basicBird, eagle, turkey}

public class AnimalMap : MonoBehaviour 
{
	public List<GameObject> prairieDogList;
	void Start () 
	{
		PopulatePrairieDogList();
	}
	
	void Update () 
	{
		PopulatePrairieDogList();
	}
	
	void PopulatePrairieDogList()
	{
		prairieDogList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Fauna"));
		
		for (int i = 0; i < prairieDogList.Count; i++)
		{
			if (prairieDogList[i].GetComponent<AnimalStateMachine>().myType != AnimalType.prairieDog)
			{
				prairieDogList.RemoveAt(i);
				i--;
			}
		}
	}
	
	public GameObject FindClosestAnimal(AnimalType animal, GameObject searchingAnimal, bool idleOnly, bool checkFurthest)
	{
		PopulatePrairieDogList();
		Vector2 myPosition = searchingAnimal.transform.position;
		
		List<GameObject> animalList = new List<GameObject>();
		if(animal == AnimalType.prairieDog)
			animalList = prairieDogList;
		
		GameObject closest = animalList[0];
		
		for (int i = 0; i < animalList.Count; i++)
		{
			if(animalList[i] != searchingAnimal)
			{
				float currentDistance = Vector2.Distance(closest.transform.position, myPosition);
				float newDistance = Vector2.Distance(animalList[i].transform.position, myPosition);
				if((!checkFurthest && newDistance < currentDistance) || (checkFurthest && newDistance > currentDistance))
				{
					if(!idleOnly || animalList[i].GetComponent<AnimalBrain>().myState == BehaviorState.idle)
						closest = animalList[i];
				}
			}
		}
		return closest;
	}
	
	public float CountIdleAnimals(AnimalType animal)
	{
		PopulatePrairieDogList();
		float idleAnimalCount = 0;
		
		List<GameObject> animalList = new List<GameObject>();
		if(animal == AnimalType.prairieDog)
			animalList = prairieDogList;
		
		for (int i = 0; i < animalList.Count; i++)
		{
			if(animalList[i].GetComponent<AnimalBrain>().myState == BehaviorState.idle);
				idleAnimalCount++;	
		}
		
		return idleAnimalCount;
	}
}
