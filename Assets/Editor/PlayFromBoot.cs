#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class PlayFromFirstScene
{
    static PlayFromFirstScene()
    {
        EditorApplication.playModeStateChanged += LoadFirstSceneOnPlay;
    }

    private static void LoadFirstSceneOnPlay(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // Get first scene in Build Settings
            string firstScene = EditorBuildSettings.scenes[0].path;
            if (!EditorSceneManager.GetActiveScene().path.Equals(firstScene))
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(firstScene);
            }
        }
    }
}
#endif
