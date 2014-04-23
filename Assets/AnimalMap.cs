using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AnimalType {prairieDog, basicBird, eagle}

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
			if (!prairieDogList[i].GetComponent<PlatformingAnimalBody>())
			{
				prairieDogList.RemoveAt(i);
				i--;
			}
		}
	}
	
	public GameObject FindClosestAnimal(AnimalType animal, Vector2 myPosition)
	{
		List<GameObject> animalList = new List<GameObject>();
		if(animal == AnimalType.prairieDog)
			animalList = prairieDogList;
		
		GameObject closest = animalList[0];
		
		for (int i = 0; i < animalList.Count; i++)
		{
			float currentDistance = Vector2.Distance(closest.transform.position, myPosition);
			float newDistance = Vector2.Distance(animalList[i].transform.position, myPosition);
			if(newDistance < currentDistance)
				closest = animalList[i];
		}
		
		return closest;
	}
}
