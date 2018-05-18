using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class SharedFilesManager : EditorWindow
{
	const string UtilityFolder_LibraryPath = "/Volumes/Media/Library/Shared/Utilities";
	const string PluginsFolder_LibraryPath = "/Volumes/Media/Library/Shared/Plugins";

	static string utilityFolder_ProjectPath;
	static string pluginsFolder_ProjectPath;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Shared Folders Manager")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		SharedFilesManager window = (SharedFilesManager)EditorWindow.GetWindow(typeof(SharedFilesManager));
		window.Show();

		utilityFolder_ProjectPath = Application.dataPath + "/Shared/Utilities";
		pluginsFolder_ProjectPath = Application.dataPath + "/Shared/Plugins";


//		var script = MonoScript.FromScriptableObject(window);
//		utilityFolder_ProjectPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
//		utilityFolder_ProjectPath = Directory.GetParent(utilityFolder_ProjectPath).ToString();
	}


	void OnGUI()
	{
		var style = new GUIStyle (GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
		EditorGUILayout.LabelField("Utility Folder", style);

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save"))
		{
			CopyAll(utilityFolder_ProjectPath, UtilityFolder_LibraryPath);
		}


		if (GUILayout.Button("Load"))
		{
			CopyAll(UtilityFolder_LibraryPath, utilityFolder_ProjectPath);
		}
		GUILayout.EndHorizontal();

		EditorGUILayout.LabelField("Plugins Folder", style);


		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save"))
		{
			CopyAll(pluginsFolder_ProjectPath, PluginsFolder_LibraryPath);
		}


		if (GUILayout.Button("Load"))
		{
			CopyAll(PluginsFolder_LibraryPath, pluginsFolder_ProjectPath);
		}
		GUILayout.EndHorizontal();
	}

	public static void CopyAll(string source, string target)
	{
		Debug.Log(source);
		var _source = new DirectoryInfo (source);
		var _target = new DirectoryInfo (target);
		CopyAll(_source, _target);
	}

	public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
	{
		if (source.FullName.ToLower() == target.FullName.ToLower())
		{
			return;
		}

		// Check if the target directory exists, if not, create it.
		if (Directory.Exists(target.FullName) == false)
		{
			Directory.CreateDirectory(target.FullName);
		}

		// Copy each file into it's new directory.
		foreach (FileInfo fi in source.GetFiles())
		{
			if (fi.Extension != ".meta")
			{
				Debug.Log("Copying " + target.FullName + " to " + fi.Name);
				fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
			}
		}

		// Copy each subdirectory using recursion.
		foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
		{
			DirectoryInfo nextTargetSubDir =
				target.CreateSubdirectory(diSourceSubDir.Name);
			CopyAll(diSourceSubDir, nextTargetSubDir);
		}
	}
}
