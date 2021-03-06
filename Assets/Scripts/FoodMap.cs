﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodMap : MonoBehaviour {
	
	public List<GameObject> foodList;
	public List<GameObject> waterList;
	// Use this for initialization
	void Awake () 
	{
		foodList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
		waterList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Water"));
	}
	
	// Update is called once per frame
	void Update () 
	{
		foodList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Food"));
		waterList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Water"));
		
		//remove all "ungrown" food sources from foodlist:
		for (int i = 0; i < foodList.Count; i++)
		{
			FoodSource foodSource = foodList[i].GetComponent<FoodSource>();
			if (!foodSource.isGrown)
			{
				foodList.RemoveAt(i);
				i--;
			}
		}
		
		//remove all "ungrown" food sources from foodlist:
		for (int i = 0; i < waterList.Count; i++)
		{
			if(waterList[i].GetComponent<DepletableWaterSource>())
			{
				DepletableWaterSource water = waterList[i].GetComponent<DepletableWaterSource>();
				if (water.drinksRemaining <= 0)
				{
					waterList.RemoveAt(i);
					i--;
				}
			}
		}
	}
	
	public GameObject FindClosestResource (ResourceType resource, Vector2 myPosition, AnimalType myType)
	{
		List<GameObject> resourceList = new List<GameObject>();
		if(resource == ResourceType.food)
			resourceList = foodList;
		if(resource == ResourceType.water)
			resourceList = waterList;
		
		GameObject closest = resourceList[0];
		if(myType != AnimalType.prairieDog && resourceList[0].GetComponent<StoragePointSource>())
		{
			for(int i = 0; i < resourceList.Count; i++)
			{
				if(!resourceList[i].GetComponent<StoragePointSource>())
					closest = resourceList[i];
					break;
			}
		}
		
		for (int i = 0; i < resourceList.Count; i++)
		{
			float currentDistance = Vector2.Distance(closest.transform.position, myPosition);
			float newDistance = Vector2.Distance(resourceList[i].transform.position, myPosition);
			if(newDistance < currentDistance)
			{
				if(myType == AnimalType.prairieDog || !resourceList[i].GetComponent<StoragePointSource>())
				{
					closest = resourceList[i];
				}
			}
		}
		
		return closest;
	}
}
