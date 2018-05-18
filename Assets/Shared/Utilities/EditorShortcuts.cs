using UnityEditor;
using UnityEngine;

public class EditorShortcuts: MonoBehaviour
{
	//Distance from scene camera after pulling of the object
	static float DistanceFromCamera = 5f;
	static float TranslateStep = 0.01f;
	static Transform Target;
	static Vector3 TransformPositionCopy;
	static Quaternion TransformRotationCopy;
	static bool IsTransformCopied;


	#if UNITY_EDITOR_OSX
	[MenuItem("Edit/Shift/Deselect All #q", false, -101)]
	#else
	[MenuItem("Edit/Ctrl/Deselect All %q", false, -101)]
	#endif

	static void Deselect()
	{
		Selection.activeGameObject = null;
	}

	//*******************CTRL/CMD**********************
	[MenuItem("Edit/Ctrl/LockInspector %e")]
	public static void Lock()
	{
		ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
		ActiveEditorTracker.sharedTracker.ForceRebuild();
	}

	[MenuItem("Edit/Ctrl/Lock %e", true)]
	public static bool Valid()
	{
		return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
	}
		

	//*******************ALT**********************

	[MenuItem("Edit/Alt/Pull object &g", false, -102)]
	static void PullObject()
	{
		//Add object state to undo state
		RecordState("Teleport object");

		//Pulls object to the camera
		Vector3 sceneCameraPosition = SceneView.lastActiveSceneView.camera.transform.position;
		Vector3 sceneCameraDirection = SceneView.lastActiveSceneView.rotation * Vector3.forward * DistanceFromCamera;

		if (Selection.activeGameObject)
		{

			Target.position = sceneCameraPosition + sceneCameraDirection;
		}
	}

	[MenuItem("Edit/Alt/Reset Transform &r", false, -102)]
	static void ResetTransform()
	{
		//Add object state to undo state
		RecordState("Reset Transform");

		//Resets transform
		Target.transform.position = Vector3.zero;
		Target.rotation = Quaternion.identity;
	}

	[MenuItem("Edit/Alt/Copy Transform &c", false, -102)]
	static void CopyTransform()
	{
		//Copies transform to buffer
		IsTransformCopied = true;
		TransformPositionCopy = Selection.activeTransform.position;
		TransformRotationCopy = Selection.activeTransform.rotation;
	}

	[MenuItem("Edit/Alt/Paste Transform &v", false, -102)]
	static void PasteTransform()
	{
		RecordState("Paste Transform");

		//Copies transform to buffer
		if (IsTransformCopied)
		{
			Selection.activeTransform.position = TransformPositionCopy;
			Selection.activeTransform.rotation = TransformRotationCopy;
		}
	}


	//*******************HELPERS**********************

	static void MoveObject(Vector3 direction, string operationTitle)
	{
		//Add object state to undo state
		RecordState(operationTitle);

		Transform target = Selection.activeGameObject.transform;
		Undo.RecordObject(target, operationTitle);

		target.Translate(direction * TranslateStep);
	}

	static void RecordState(string operationTitle)
	{
		//Add object state to undo state
		Target = Selection.activeTransform;
		Undo.RecordObject(Target, operationTitle);
	}
}