using System.Collections;
using UnityEngine;

public class CamSlowMotionDelay : MonoBehaviour
{

	public float slowMotionTimeScale = .2f;
	GameSettings settings;

	void Start()
	{
		settings = Resources.Load("GameSettings", typeof(GameSettings)) as GameSettings;
	}

	public void StartSlowMotionDelay(float duration)
	{
		StopAllCoroutines();
		StartCoroutine(SlowMotionRoutine(duration));
	}

	//slow motion delay
	IEnumerator SlowMotionRoutine(float duration)
	{

		//set timescale
		Time.timeScale = slowMotionTimeScale;

		//wait a moment...
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < (startTime + duration))
		{
			yield return null;
		}
		
		//reset timescale

		if (settings != null)
		{ 
			Time.timeScale = settings.timeScale;
		} else
		{
			Time.timeScale = 1;
		}
	}
}
