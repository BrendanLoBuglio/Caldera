using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HUD : MonoBehaviour 
{
	private float currentNutrition = 100f, currentHydration = 100f, karma = 100f;
	public float nutritionRatio = 1f, hydrationRatio = 1f, karmaRatio = 1f;
	public float spaceBetweenBars = 5f;
	public float barMargin = 5f;
	public Vector2 barSizeScreenFraction;
	
	private Rect karmaBarRect;
	private Rect foodBarRect;
	private Rect waterBarRect;
	void Update () 
	{
		karmaBarRect = new Rect(Screen.width - (barMargin + (Screen.width / barSizeScreenFraction.x)), spaceBetweenBars,
			 karmaRatio * Screen.width / barSizeScreenFraction.x, Screen.height / barSizeScreenFraction.y);
		foodBarRect = new Rect(Screen.width - (Screen.width / barSizeScreenFraction.x) - barMargin, 2f * spaceBetweenBars + (Screen.height / barSizeScreenFraction.y),
		     nutritionRatio * Screen.width / barSizeScreenFraction.x, Screen.height / barSizeScreenFraction.y);
		waterBarRect = new Rect(Screen.width - (Screen.width / barSizeScreenFraction.x) - barMargin, 3f * spaceBetweenBars + 2f * (Screen.height / barSizeScreenFraction.y),
		     hydrationRatio * Screen.width / barSizeScreenFraction.x, Screen.height / barSizeScreenFraction.y);
	}
	
	void OnGUI()
	{
		if(Input.GetKey (KeyCode.G))
		{
			GUI.Box(karmaBarRect, "This is the Karma Bar");
			GUI.Box(foodBarRect, "This is the Food bar");
			GUI.Box(waterBarRect, "This is the Water bar");
		}
	}
}
