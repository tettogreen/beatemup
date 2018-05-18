using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace EckTechGames
{
	[InitializeOnLoad]
	public class AutoSaveExtension
	{
		const float AUTOSAVETIME = 600f;

		static float lastTimeSaved = Time.realtimeSinceStartup;

		// Static constructor that gets called when unity fires up.
		static AutoSaveExtension ()
		{
			EditorApplication.playModeStateChanged += AutoSaveWhenPlaymodeStarts;
			EditorApplication.update += Update;
		}

		private static void AutoSaveWhenPlaymodeStarts(PlayModeStateChange obj)
		{
			// If we're about to run the scene...
			if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			{
				// Save the scene and the assets.
				Save();
			}
		}

		private static void Save()
		{
			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), "Assets/temp/autosave.unity", true);
			AssetDatabase.SaveAssets();
		}

		private static void Update()
		{
			if ((Time.realtimeSinceStartup - lastTimeSaved) > AUTOSAVETIME)
			{
				Save();
				lastTimeSaved = Time.realtimeSinceStartup;
			}
		}
	}
}