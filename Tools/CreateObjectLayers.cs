// Vinyl 2023.

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
namespace Vinyl
{
    public class CreateVRCObjectLayers : EditorWindow
    {
        public enum VRCObjectLayer
        {
            Interactive,
            Player,
            PlayerLocal,
            Environment,
            UiMenu,
            Pickup,
            PickupNoEnvironment,
            StereoLeft,
            StereoRight,
            Walkthrough,
            MirrorReflection,
            reserved2,
            reserved3,
            reserved4,
            PostProcessing,
        }

        private GUIContent StatusContent = new GUIContent("Waiting...");
        private string StatusText = "Waiting...";
        private Vector2 ScrollPosition = Vector2.zero;

        [MenuItem("Tools/Vinyl/Create VRC Object Layers", false, 0)]
        public static void OpenWindow()
        {
            CreateVRCObjectLayers window = GetWindow<CreateVRCObjectLayers>();
            window.titleContent = new GUIContent("Create VRC Object Layers");
        }

        private void OnGUI()
        {
            GUI.enabled = true;

            if (GUILayout.Button("Create VRC Object Layers"))
            {
                StatusText = "Creating layers..." + System.Environment.NewLine;

                VRCObjectLayer[] LayerArray = System.Enum.GetValues(typeof(VRCObjectLayer)) as VRCObjectLayer[];

                foreach (VRCObjectLayer Layer in LayerArray)
                {
                    CreateLayer(Layer.ToString());
                }

                StatusText += System.Environment.NewLine;
                StatusText += System.Environment.NewLine;
                StatusText += "Done!";
            }

            StatusContent.text = StatusText;
            EditorStyles.label.wordWrap = true;

            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, false, true);
            GUILayout.Label(StatusContent);
            GUILayout.EndScrollView();
        }

        private Dictionary<string, int> GetAllLayers()
        {
            SerializedObject TagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty Layers = TagManager.FindProperty("layers");
            int LayerSize = Layers.arraySize;

            Dictionary<string, int> LayerDictionary = new Dictionary<string, int>();

            for (int Index = 0; Index < LayerSize; Index++)
            {
                SerializedProperty Element = Layers.GetArrayElementAtIndex(Index);
                string LayerName = Element.stringValue;

                if (!string.IsNullOrEmpty(LayerName))
                {
                    LayerDictionary.Add(LayerName, Index);
                }
            }

            return LayerDictionary;
        }

        private void CreateLayer(string Name)
        {
            bool bSuccess = false;
            Dictionary<string, int> LayerDictionary = GetAllLayers();

            if (!LayerDictionary.ContainsKey(Name))
            {
                SerializedObject TagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
                SerializedProperty Layers = TagManager.FindProperty("layers");

                for (int Index = 0; Index < 31; Index++)
                {
                    SerializedProperty Element = Layers.GetArrayElementAtIndex(Index);

                    if (string.IsNullOrEmpty(Element.stringValue) && Index >= 8)
                    {
                        Element.stringValue = Name;

                        TagManager.ApplyModifiedProperties();
                        bSuccess = true;

                        StatusText += System.Environment.NewLine;
                        StatusText += "Created Layer: " + Name + " (" + Index + ")";

                        break;
                    }
                }

                if (!bSuccess)
                {
                    StatusText += System.Environment.NewLine;
                    StatusText += "Failed to create layer!";
                    Debug.Log("Failed to create layer: " + Name + "!");
                }
            }
            else
            {
                StatusText += System.Environment.NewLine;
                StatusText += "Layer already exists: " + Name;
                Debug.Log("Layer already exists: " + Name);
            }
        }
    }
}
#endif // UNITY_EDITOR
