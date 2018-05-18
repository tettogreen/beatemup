using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationOnEnable : MonoBehaviour
{
	[MinMaxAttribute(-180, 180)] public Vector2 minMaxRotation = new Vector2 (-90, 90);


	void OnEnable()
	{
		float randomZRotaion = Random.Range(minMaxRotation.x, minMaxRotation.y);
		transform.rotation = Quaternion.Euler(0, 0, randomZRotaion);
	}
}
