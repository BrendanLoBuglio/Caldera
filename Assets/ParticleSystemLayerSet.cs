using UnityEngine;
using System.Collections;

public class ParticleSystemLayerSet : MonoBehaviour 
{
	public string layerName;
	private ParticleSystem particles;
	
	void Start () 
	{
		particles = gameObject.GetComponent<ParticleSystem>();
		particles.renderer.sortingLayerName = layerName;
	}
}
