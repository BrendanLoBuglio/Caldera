using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AnimalType {prairieDog, basicBird, eagle, turkey}

public class AnimalMap : MonoBehaviour 
{
	public List<GameObject> prairieDogList;
	public List<GameObject> idlePrairieDogList;
	void Start () 
	{
		PopulatePrairieDogList();
		PopulateIdlePrairieDogList();
	}
	
	void Update () 
	{
		PopulatePrairieDogList();
		PopulateIdlePrairieDogList();
	}
	
	public void PopulatePrairieDogList()
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
	public void PopulateIdlePrairieDogList()
	{
		idlePrairieDogList = new List<GameObject>();
		
		PopulatePrairieDogList();		
		
		for (int i = 0; i < prairieDogList.Count; i++)
		{
			if(prairieDogList[i].GetComponent<PrairieDogBrain>().myState == BehaviorState.idle)
				idlePrairieDogList.Add(prairieDogList[i]);
		}
	}
	
	public GameObject FindClosestAnimal(AnimalType animal, GameObject searchingAnimal, bool idleOnly, bool checkFurthest)
	{
		PopulatePrairieDogList();
		PopulateIdlePrairieDogList();
		Vector2 myPosition = searchingAnimal.transform.position;
		
		List<GameObject> animalList = new List<GameObject>();
		if(animal == AnimalType.prairieDog && !idleOnly)
			animalList = prairieDogList;
		if(animal == AnimalType.prairieDog && idleOnly)
			animalList = idlePrairieDogList;
			
		GameObject closest;
		if(animalList[0] != searchingAnimal)
			closest = animalList[0];
		else
			closest = animalList[1];
		
		for (int i = 0; i < animalList.Count; i++)
		{
			if(animalList[i] != searchingAnimal)
			{
				float currentDistance = Vector2.Distance(closest.transform.position, myPosition);
				float newDistance = Vector2.Distance(animalList[i].transform.position, myPosition);
				if((!checkFurthest && newDistance < currentDistance) || (checkFurthest && newDistance > currentDistance))
					closest = animalList[i];
			}
		}
		return closest;
	}
}
