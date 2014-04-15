using UnityEngine;
using System.Collections;

public class LeftRightBump : MonoBehaviour 
{
	public float bump = 20f;
	//Applies a small "Bump" when a collision occurs from the left or right.
	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log ("Collision!");
		for(int i = 0; i < other.contacts.Length; i++)
		{
			if(other.contacts[i].normal == Vector2.right)
			{
				Debug.Log ("Force to the right!");
				rigidbody2D.AddForce(new Vector2(bump, 0));
			}
			if(other.contacts[i].normal == -Vector2.right)
			{
				Debug.Log ("Force to the left!");
				rigidbody2D.AddForce(new Vector2(-bump, 0));
			}
		}
	}
}
