using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Attributes.Editor
{
    [InitializeOnLoad]
    public static class FindAllOfInterfaceUtility
    {
        static FindAllOfInterfaceUtility()
        {
            // Subscribe to the events instead of using the update loop
            EditorApplication.hierarchyChanged += TriggerPopulateFields;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.quitting += ClearEvents;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange playModeState)
        {
            TriggerPopulateFields();
        }

        private static void TriggerPopulateFields()
        {
            if (Application.isPlaying) return;

            // Process all MonoBehaviours in the scene
            var allMonoBehaviours = Object.FindObjectsOfType<MonoBehaviour>();

            foreach (var monoBehaviour in allMonoBehaviours)
            {
                PopulateFields(monoBehaviour);
            }

            // Process all ScriptableObjects in the project
            var allScriptableObjects = FindAllScriptableObjects();

            foreach (var scriptableObject in allScriptableObjects)
            {
                PopulateFields(scriptableObject);
            }
        }

        // Populates fields in both MonoBehaviours and ScriptableObjects
        private static void PopulateFields(Object targetObject)
        {
            // Get all fields with the FindAllOfInterface attribute
            var fields = targetObject.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => f.GetCustomAttribute<FindAllOfInterfaceAttribute>() != null);

            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<FindAllOfInterfaceAttribute>();
                var interfaceType = attribute.InterfaceType;

                // Ensure the field is a list and that it is generic
                if (!field.FieldType.IsGenericType ||
                    field.FieldType.GetGenericTypeDefinition() != typeof(List<>)) continue;
                var listType = field.FieldType.GetGenericArguments()[0];

                // Ensure the list type implements the specified interface
                if (!interfaceType.IsAssignableFrom(listType)) continue;
                // Find all components that implement the interface (for MonoBehaviours)
                var components = FindComponentsWithInterface(interfaceType);

                // Find all ScriptableObjects that implement the interface (for ScriptableObjects)
                var scriptableObjects = FindScriptableObjectsWithInterface(interfaceType);

                // Create a list and add the components and ScriptableObjects to it
                var typedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType)) as IList;

                foreach (var component in components)
                {
                    typedList?.Add(component);
                }

                foreach (var scriptableObject in scriptableObjects)
                {
                    typedList?.Add(scriptableObject);
                }

                // Set the field's value
                field.SetValue(targetObject, typedList);
            }
        }

        // Finds all components in the scene that implement the given interface
        private static IEnumerable<Component> FindComponentsWithInterface(Type interfaceType)
        {
            var allObjects = Object.FindObjectsOfType<MonoBehaviour>();
            return allObjects.Where(interfaceType.IsInstanceOfType);
        }

        // Finds all ScriptableObjects in the project that implement the given interface
        private static IEnumerable<ScriptableObject> FindScriptableObjectsWithInterface(Type interfaceType)
        {
            var guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);

                if (interfaceType.IsInstanceOfType(asset))
                {
                    yield return asset;
                }
            }
        }

        // Finds all ScriptableObjects in the project
        private static IEnumerable<ScriptableObject> FindAllScriptableObjects()
        {
            var guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
                yield return asset;
            }
        }

        // Unsubscribe from events when Unity is quitting
        private static void ClearEvents()
        {
            EditorApplication.hierarchyChanged -= TriggerPopulateFields;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.quitting -= ClearEvents;
        }
    }
}