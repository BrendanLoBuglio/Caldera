using UnityEngine;
using System.Collections;

public class DayNightManager : MonoBehaviour 
{
	public Renderer daySky;
	public Renderer sunsetSky;
	public Renderer nightSky;	
	
	public float timeCounter = 0f;
	public float dayLength = 90f;
	public float dayTimeLength = 36f;
	public float sunsetLength = 9f;
	public float nightLength = 36f;
	public float transitionLength = 3f;
	
	
	private float daySkyAlpha = 1f;
	private float sunsetAlpha = 0f;
	
	void Start()
	{
		daySky = gameObject.transform.FindChild("PhotoBackground_DaySky").gameObject.renderer;
		sunsetSky = gameObject.transform.FindChild("PhotoBackground_SunsetSky").gameObject.renderer;
		nightSky = gameObject.transform.FindChild("PhotoBackground_NightSky").gameObject.renderer;
	}
	
	void Update()
	{
		timeCounter += Time.deltaTime;
		float timeOfDay = timeCounter % dayLength;
		
		
		if(timeOfDay >= 0 && timeOfDay <= dayTimeLength)
		{
			daySkyAlpha = Mathf.Lerp (daySkyAlpha, 1, Time.deltaTime / transitionLength);
		}
		else
		{
			daySkyAlpha = Mathf.Lerp (daySkyAlpha, 0, Time.deltaTime / transitionLength);
		}
		
		if( (timeOfDay >= dayTimeLength && timeOfDay <= dayTimeLength + sunsetLength) || (timeOfDay >= dayLength - sunsetLength && timeOfDay <= dayLength) )
		{
			sunsetAlpha = Mathf.Lerp (sunsetAlpha, 1, Time.deltaTime / (1f * transitionLength));
		}
		else
		{
			sunsetAlpha = Mathf.Lerp (sunsetAlpha, 0, Time.deltaTime / (1f * transitionLength));
		}		
		
		daySky.material.color = new Color(daySky.material.color.r,daySky.material.color.g,daySky.material.color.b, daySkyAlpha);
		sunsetSky.material.color = new Color(sunsetSky.material.color.r,sunsetSky.material.color.g,sunsetSky.material.color.b, sunsetAlpha);
	}
}