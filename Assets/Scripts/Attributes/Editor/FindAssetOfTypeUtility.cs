using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Attributes.Editor
{
    [InitializeOnLoad]
    public static class FindAssetOfTypeUtility
    {
        static FindAssetOfTypeUtility()
        {
            // Subscribe to editor events
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            EditorApplication.quitting += Clear;
        }

        // Trigger the search when play mode changes or hierarchy changes
        private static void OnPlayModeChanged(PlayModeStateChange state) => FindAndAssignAssets();

        private static void OnHierarchyChanged()
        {
            if (!Application.isPlaying)
            {
                FindAndAssignAssets();
            }
        }

        private static void FindAndAssignAssets()
        {
            // Process all MonoBehaviour objects in the scene
            var allMonoBehaviours = Object.FindObjectsOfType<MonoBehaviour>();
            foreach (var monoBehaviour in allMonoBehaviours)
            {
                AssignAssetsToFields(monoBehaviour);
            }

            // Process all ScriptableObject assets in the project
            var allScriptableObjects = FindAllScriptableObjects();
            foreach (var scriptableObject in allScriptableObjects)
            {
                AssignAssetsToFields(scriptableObject);
            }
        }

        // Find all ScriptableObject assets in the project
        private static ScriptableObject[] FindAllScriptableObjects()
        {
            var guids = AssetDatabase.FindAssets("t:ScriptableObject");
            return guids.Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                        .ToArray();
        }

        // Assign assets to fields in MonoBehaviour or ScriptableObject
        private static void AssignAssetsToFields(Object targetObject)
        {
            // Get all fields marked with [FindAssetOfType]
            var fields = targetObject.GetType()
                                     .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                     .Where(f => f.GetCustomAttribute<FindAssetOfTypeAttribute>() != null);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<FindAssetOfTypeAttribute>();
                AssignAssetToField(targetObject, field, attribute);
            }
        }

        private static void AssignAssetToField(Object targetObject, FieldInfo field, FindAssetOfTypeAttribute attribute)
        {
            // Use AssetDatabase to find assets of the specified type in the entire project
            var assetType = attribute.AssetType;
            var assetGuids = AssetDatabase.FindAssets($"t:{assetType.Name}");

            if (assetGuids.Length > 0)
            {
                // Load the first asset found (you could refine this to handle multiple assets)
                var assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[0]);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType);

                // Assign the asset to the field
                field.SetValue(targetObject, asset);
            }
            else
            {
                Debug.LogWarning($"No asset of type {assetType.Name} found in the project.");
            }
        }

        // Clear the event listeners when quitting
        private static void Clear()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.quitting -= Clear;
        }
    }
}
