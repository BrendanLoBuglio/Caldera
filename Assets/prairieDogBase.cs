using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class prairieDogBase : MonoBehaviour 
{
	public List<StoragePointSource> storagePoints;
	public bool hasEmptyStorage = true;
	
	void Awake () 
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			storagePoints.Add (transform.GetChild (i).GetComponent<StoragePointSource>());
		}
	}
	
	void Update ()
	{
		CheckForEmptyStorage();
	}
	
	public void CheckForEmptyStorage()
	{
		hasEmptyStorage = false; 
		int i = 0;
		while(i < storagePoints.Count && !hasEmptyStorage)
		{
			if(!storagePoints[i].isGrown)
				hasEmptyStorage = true;
			i++;
		}
	}
	
	public GameObject GetEmptyStoragePoint()
	{
		//Only call this if you know that this base has an empty storagePoint (check hasEmptyStorage first)
		for(int i = 0; i < storagePoints.Count; i++)
		{
			if(!storagePoints[i].isGrown)
			{
				CheckForEmptyStorage();
				return storagePoints[i].gameObject;
			}
		}
		Debug.Log ("Error! You called GetEmptyStoragePoint() when it didn't have an empty storage point! Get ready for some NULL REFERENCE ERRORS, baby!");
		return gameObject; //This line should pretty much crash the game if it's called, or at least nullreference it to hell.
	}
}
