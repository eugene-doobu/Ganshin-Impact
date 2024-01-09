using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GanShinEditorAssembly
{
    public static class SceneSavedManager
    {
        [InitializeOnLoad]
        private static class EditorSceneManagerSceneSaved
        {
            static EditorSceneManagerSceneSaved()
            {
                EditorSceneManager.sceneSaving += OnSceneSaving;
            }

            private static void OnSceneSaving(Scene scene, string path)
            {
                Debug.Log($"Saving scene '{scene.name}' to {path}");

                CleaningEventSystem();
            }

            private static void CleaningEventSystem()
            {
                var eventTriggers = Object.FindObjectsOfType<EventSystem>();
                foreach (var eventTrigger in eventTriggers)
                {
                    Debug.LogWarning($"Removed EventSystem GameObject: {eventTrigger.gameObject.name}");
                    Object.DestroyImmediate(eventTrigger.gameObject);
                }
            }
        }
    }
}