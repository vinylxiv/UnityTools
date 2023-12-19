using UnityEditor;
using UnityEngine;
using VF.Builder;
using VF.Component;
using VF.Inspector;

namespace VF.Menu {
    public class HapticsMenuItem {

        private const string DialogTitle = "VRCFury Haptics";

        public static void Create(bool plug) {
            var newObj = GameObjects.Create(plug ? "Haptic Plug" : "Haptic Socket", Selection.activeTransform);

            if (plug) {
                newObj.AddComponent<VRCFuryHapticPlug>();
            } else {
                newObj.AddComponent<VRCFuryHapticSocket>();
            }

            Tools.pivotRotation = PivotRotation.Local;
            Tools.pivotMode = PivotMode.Pivot;
            Tools.current = Tool.Move;
            Selection.SetActiveObjectWithContext(newObj, newObj);
            //SceneView.FrameLastActiveSceneView();
            
            EditorUtility.DisplayDialog(DialogTitle,
                $"{(plug ? "Plug" : "Socket")} created!\n\nDon't forget to attach it to an appropriate bone on your avatar and rotate it so it faces the correct direction!", "Ok");
            
            SceneView sv = EditorWindow.GetWindow<SceneView>();
            if (sv != null) sv.drawGizmos = true;
        }

        public static void RunBake() {
            var ok = EditorUtility.DisplayDialog(DialogTitle,
                "This utility will convert the selected VRCFury haptic component into plain VRChat colliders, so that this prefab can be distributed without the client needing VRCFury. This is intended for avatar artists only.",
                "I am an avatar artist distributing this package, Continue",
                "Cancel"
            );
            if (!ok) return;
            
            ok = EditorUtility.DisplayDialog(DialogTitle,
                "BEWARE that baked components can only send haptic triggers, not receive. Thus after baking, this will trigger haptics on other users, but not receive haptics for the owner." +
                " Users can still easily re-add full support themselves by simply running the VRCFury 'Upgrade Legacy Haptics' tool on their avatar (which will convert this bake back to a component).",
                "I understand the limitations, bake now",
                "Cancel"
            );
            if (!ok) return;

            if (!Selection.activeGameObject) {
                EditorUtility.DisplayDialog(DialogTitle,"No object selected", "Ok");
                return;
            }

            var plug = Selection.activeGameObject.GetComponent<VRCFuryHapticPlug>();
            var socket = Selection.activeGameObject.GetComponent<VRCFuryHapticSocket>();
            if (!plug && !socket) {
                EditorUtility.DisplayDialog(DialogTitle,"No haptic components found on selected object", "Ok");
                return;
            }

            if (plug) {
                VRCFuryHapticPlugEditor.Bake(plug, onlySenders:true);
                Object.DestroyImmediate(plug);
            }
            if (socket) {
                VRCFuryHapticSocketEditor.Bake(socket, onlySenders:true);
                Object.DestroyImmediate(socket);
            }
        }
    }
}
