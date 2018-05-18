using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class ExtensionMethods
{

	//	================================================
	//	===================================TRANSFORM======
	//	================================================
	public static Transform FindChildWithTag(this Transform transform, string tag)
	{
		Transform result = null;

		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);

			if (child.CompareTag(tag))
			{
				return child;
			}

			if (child.childCount != 0)
				result = child.FindChildWithTag(tag);

			if (result != null)
				return result;
		}

		return null;
	}

	//Get angle between transform forward and direction to target
	public static float AngleToFaceTarget(this Transform transform, Transform target)
	{
		Vector3 targetDir = target.position - transform.position;
		Vector3 forwardDir = transform.forward;
		return Vector3.Angle(forwardDir, targetDir);
	}

	//Get angles between transform forward and direction to target projections (projected on x and y axis)
	public static Vector2 AnglesToFaceTarget(this Transform transform, Transform target, Vector3 localForward)
	{
		Vector2 result;
		Vector3 targetDir = target.position - transform.position;
		Vector3 forwardDir = localForward;

		//		Debug.Log (transform.forward);

		var projection = Vector3.ProjectOnPlane(targetDir, transform.up);

		//Angle between target direction and it's projection onto XZ plane ("vertical" angle)
		result.x = Vector3.Angle(projection, targetDir);
		//Make angle negative if cross multiplication vector points left
		//		if (Vector3.Cross (projection, targetDir).x > 0)
		//			result.x *= -1;
		//Angle between transform's forward direction and projection of target direction onto XZ plane ("horizontal" angle)
		result.y = Vector3.Angle(projection, forwardDir);


		//Make angle negative if cross multiplication vector points down 
		Vector3 rightHandedVector = Vector3.Cross(projection, forwardDir);

		Matrix4x4 localSystem = Matrix4x4.LookAt(Vector3.zero, projection, transform.up);
		rightHandedVector = localSystem.MultiplyVector(rightHandedVector);

		if (rightHandedVector.y > 0)
			result.y *= -1;

		rightHandedVector = Vector3.Cross(projection, targetDir);
		rightHandedVector = localSystem.MultiplyVector(rightHandedVector);

		if (rightHandedVector.x < 0)
			result.x *= -1;

		return result;
	}

	public static Vector2 AnglesToFaceTarget(this Transform transform, Transform target)
	{
		return transform.AnglesToFaceTarget(target, transform.forward);
	}



	//	================================================
	//	===================================EVENTS======
	//	================================================

	public static void AddListener(this UnityEvent unityEvent, UnityAction call, bool removeAfterInvoke)
	{
		unityEvent.AddListener(call);

		if (removeAfterInvoke)
		{
			unityEvent.AddListener(() =>
			{
				unityEvent.RemoveListener(call);
			});
		}
	}

	public static void ClearAfterInvoke(this UnityEvent unityEvent, bool enabled)
	{
		if (enabled)
		{
			unityEvent.AddListener(unityEvent.ClearAllAndResubscribe);
		} else
		{
			unityEvent.RemoveListener(unityEvent.ClearAllAndResubscribe);
		}
	}
	//
	static void ClearAllAndResubscribe(this UnityEvent unityEvent)
	{
		unityEvent.RemoveAllListeners();
		unityEvent.AddListener(unityEvent.ClearAllAndResubscribe);
	}


	//	================================================
	//	===================================PHYSICS======
	//	================================================

	public static bool Contains(this LayerMask layerMask, int layer)
	{
		return layerMask == (layerMask | (1 << layer));
	}

	//	================================================
	//	===================================EDITOR======
	//	================================================


	//Draws serialized property for an asset, e.g. ScriptabeObject, Material etc.
	//Should be called in Editor class with this.DrawSerializedProperty(..) !!!
	public static bool DrawSerializedProperty(this Editor editor, string propertyName)
	{
		Editor _editor = null;
		editor.serializedObject.Update();

		//Asset serialization
		SerializedObject obj = new SerializedObject (editor.target); 
		var prop = obj.FindProperty(propertyName);

		if (prop == null)
			return false;

		Editor.CreateCachedEditor(prop.objectReferenceValue, null, ref _editor);
		if (_editor)
		{
			_editor.OnInspectorGUI();
		}


		editor.serializedObject.ApplyModifiedProperties();

		return true;
	}

}