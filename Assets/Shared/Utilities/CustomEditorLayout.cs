using UnityEditor;
using UnityEngine;



public static class CustomEditorLayout
{
	public static void CloneButton(Object target)
	{
		string targetName = target.GetType().ToString();
		if (GUILayout.Button("Clone " + targetName))
		{
			string assetPath = AssetDatabase.GetAssetPath(target);

			string assetName = target.name + ".asset";
			string newPath = assetPath.Remove(assetPath.Length - assetName.Length);
			string pathToClone = newPath + target.name + "Clone.asset";

			pathToClone = AssetDatabase.GenerateUniqueAssetPath(pathToClone);

			AssetDatabase.CopyAsset(assetPath, pathToClone);
		}
	}
}