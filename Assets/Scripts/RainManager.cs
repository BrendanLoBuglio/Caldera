using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour {
	
	public bool isRaining = false;
	private float rainTimer;
	public float rainTimeMin = 90f;
	public float rainTimeMax = 180f;
	public float rainDurationMin = 10f;
	public float rainDurationMax = 30f;
	public float rainAngleRange = 45f;
	public float rainYOffset = 5f;
	private float rainAngle = 0f;
	
	
	private Vector2 originalPosition;
	public GameObject rain;
	private ParticleSystem particles;
	private GameObject rainHolder;
	public Vector2 rainSpeed = new Vector2(-1f, 2f);
	private float baseColumnNumber = 50f;
	private float columnVariance = 5f;
	
	private float rainWidth;
	private float rainHeight;
	private LevelMap cam;
	
	void Start () 
	{
		rainTimer = Random.Range (rainTimeMin, rainTimeMax);
		rainWidth = rain.renderer.bounds.max.x - rain.renderer.bounds.min.x;
		rainHeight = rain.renderer.bounds.max.y - rain.renderer.bounds.min.y;
		cam = gameObject.GetComponent<LevelMap>();
		rainHolder = transform.FindChild("RainHolder").gameObject;
		particles = rainHolder.GetComponent<ParticleSystem>();
		
		originalPosition = rainHolder.transform.position;
	}
	
	void Update () 
	{
		Debug.DrawLine (originalPosition, originalPosition - Vector2.up*0.1f, Color.magenta);
		
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
	private void InstantiateRainRow (float numOfColumns)
	{
		for(float column = 0; column <= numOfColumns; column++)
		{
			Vector3 rainPosition = new Vector3(transform.position.x - cam.cameraBox.width/2f + column * (cam.cameraBox.width / numOfColumns),transform.position.y + cam.cameraBox.height/2f - rainHeight/2f, 0);
			GameObject createdObject = (GameObject)Instantiate(rain, rainPosition, Quaternion.identity);
			createdObject.transform.parent = rainHolder.transform;
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
			rainHolder.transform.position = new Vector2(originalPosition.x + (rainYOffset * Mathf.Cos (rainAngle)), originalPosition.y + (rainYOffset * Mathf.Sin (rainAngle)));
			rainHolder.transform.LookAt (originalPosition);
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
