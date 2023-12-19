// Vinyl 2023.

using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Vinyl
{
    public class SkinnedMeshUpdater : EditorWindow
    {
        private SkinnedMeshRenderer TargetMesh;
        private Transform RootBone;
        private bool bIncludeInactive;

        private GUIContent StatusContent = new GUIContent("Waiting...");
        private string StatusText = "Waiting...";
        private Vector2 ScrollPosition = Vector2.zero;

        [MenuItem("Tools/Vinyl/Skinned Mesh Updater", false, 0)]
        public static void OpenWindow()
        {
            SkinnedMeshUpdater Window = GetWindow<SkinnedMeshUpdater>();
            Window.titleContent = new GUIContent("Skinned Mesh Updater");
        }

        private void OnGUI()
        {
            // Init properties.
            TargetMesh = EditorGUILayout.ObjectField("Target", TargetMesh, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
            RootBone = EditorGUILayout.ObjectField("RootBone", RootBone, typeof(Transform), true) as Transform;

            bIncludeInactive = EditorGUILayout.Toggle("Include Inactive", bIncludeInactive);

            // Check if properties are set.
            GUI.enabled = (TargetMesh != null && RootBone != null);
            if (!GUI.enabled)
            {
                StatusText = "Add a target SkinnedMeshRenderer and a root bone.";
            }

            if (GUILayout.Button("Update Skinned Mesh Renderer"))
            {
                ProcessBones();
            }

            StatusContent.text = StatusText;
            EditorStyles.label.wordWrap = true;

            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, false, true);
            GUILayout.Label(StatusContent);
            GUILayout.EndScrollView();
        }

        private void ProcessBones()
        {
            StatusText = "Processing bones..." + System.Environment.NewLine;

            string RootName = TargetMesh.rootBone != null ? TargetMesh.rootBone.name : "";
            Transform NewRoot = null;

            Transform[] NewBones = new Transform[TargetMesh.bones.Length];
            Transform[] ExistingBones = RootBone.GetComponentsInChildren<Transform>(bIncludeInactive);
            int MissingBones = 0;

            // Reassign new bones
            for (int Index = 0; Index < TargetMesh.bones.Length; Index++)
            {
                if (TargetMesh.bones[Index] == null)
                {
                    StatusText += System.Environment.NewLine;
                    StatusText += "WARNING: Do not delete the old bones before the skinned mesh is processed!";
                    Debug.Log("WARNING: Do not delete the old bones before the skinned mesh is processed!");

                    MissingBones++;

                    continue;
                }

                string BoneName = TargetMesh.bones[Index].name;
                bool bFound = false;

                foreach (Transform NewBone in ExistingBones)
                {
                    if (NewBone.name == RootName)
                    {
                        NewRoot = NewBone;
                    }

                    if (NewBone.name == BoneName)
                    {
                        StatusText += System.Environment.NewLine;
                        StatusText += "Found bone: " + NewBone.name;

                        NewBones[Index] = NewBone;
                        bFound = true;
                    }
                }

                if (!bFound)
                {
                    StatusText += System.Environment.NewLine;
                    StatusText += "Missing bone: " + BoneName;
                    Debug.Log("Missing bone: " + BoneName);

                    MissingBones++;
                }
            }

            TargetMesh.bones = NewBones;
            StatusText += System.Environment.NewLine;
            StatusText += System.Environment.NewLine;
            StatusText += "Done! Missing bones: " + MissingBones;

            if (NewRoot != null)
            {
                StatusText += System.Environment.NewLine;
                StatusText += System.Environment.NewLine;
                StatusText += "Setting " + RootName + " as root bone.";

                TargetMesh.rootBone = NewRoot;
            }
        }
    }
}
#endif // UNITY_EDITOR
