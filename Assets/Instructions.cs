using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour 
{
	private Renderer renderer;
	private float alpha = 0;
	
	void Start () 
	{
		renderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	
	void Update () 
	{
		if(Input.GetKey (KeyCode.I))
			alpha += 1f * Time.deltaTime;
		else
			alpha -= 1f * Time.deltaTime;
		
		alpha = Mathf.Clamp(alpha, 0, 1f);
		
		renderer.material.color = new Color(255f,255f,255f, alpha);
	}
}
