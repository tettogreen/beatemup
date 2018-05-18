using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class DespawnTrigger : MonoBehaviour
{

	void OnTriggerEnter2D(Collider2D other)
	{
		LeanPool.Despawn(other.attachedRigidbody.gameObject);
	}
}
