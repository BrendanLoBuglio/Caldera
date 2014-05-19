using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour {
	
	public bool isRaining = false;
	public float rainTimer;
	public float rainTimeMin = 90f;
	public float rainTimeMax = 180f;
	public float rainDurationMin = 10f;
	public float rainDurationMax = 30f;
	public float rainAngleRange = 45f;
	public float rainYOffset = 5f;
	private float rainAngle = 0f;
	
	
	private Vector3 originalPosition;
	public GameObject rain;
	private ParticleSystem particles;
	private GameObject rainHolder;
	
	void Start () 
	{
		rainTimer = Random.Range (rainTimeMin, rainTimeMax);
		rainHolder = transform.FindChild("RainHolder").gameObject;
		particles = rainHolder.GetComponent<ParticleSystem>();
		
		originalPosition = rainHolder.transform.localPosition;
		particles.renderer.sortingLayerName = "Weather";
	}
	
	void Update () 
	{
		Debug.DrawLine ((Vector2)originalPosition, (Vector2)originalPosition - Vector2.up*0.1f, Color.magenta);
		HandleRain();
		if (isRaining)
		{
			particles.enableEmission = true;
		}
		else
		{
			particles.enableEmission = false;
		}
	}
	private void HandleRain()
	{
		rainTimer -= Time.deltaTime;
		if(rainTimer <= 0 && !isRaining)
		{
			isRaining = true;
			rainTimer = Random.Range (rainDurationMin, rainDurationMax);
			rainAngle = (90f + Random.Range (-rainAngleRange, rainAngleRange)) * Mathf.Deg2Rad;
			rainHolder.transform.localPosition = new Vector3(originalPosition.x + (rainYOffset * Mathf.Cos (rainAngle)), originalPosition.y + (rainYOffset * Mathf.Sin (rainAngle)), originalPosition.z);
			rainHolder.transform.LookAt ((Vector2)originalPosition + (Vector2)transform.position); //Reminder: the second transform is Main Camera's Transform
			rainHolder.transform.Rotate(0, 0, 90f);
			particles.startRotation = -rainAngle;
		}
		
		if(rainTimer <= 0 && isRaining)
		{
			isRaining = false;
			rainTimer = Random.Range (rainTimeMin, rainTimeMax);
		}
	}
}
