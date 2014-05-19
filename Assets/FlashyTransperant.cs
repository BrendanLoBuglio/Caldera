﻿using UnityEngine;
using System.Collections;

public class FlashyTransperant : MonoBehaviour {

	public Renderer renderer;
	public float alpha = 0f;
	public float sinCounter = 0f;
	
	
	void Start () 
	{
		renderer = gameObject.GetComponent<Renderer>();
	}
	
	void Update () 
	{
		sinCounter += Time.deltaTime;
		alpha = .75f + .25f * Mathf.Sin (sinCounter);
		
		renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
	}
}
