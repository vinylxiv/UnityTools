using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace VF.Utils {
    public class AnimationWindowUtils {
        private static Type animationWindowState = ReflectionUtils.GetTypeFromAnyAssembly("UnityEditorInternal.AnimationWindowState");
        
        private static PropertyInfo isRecordingProperty = animationWindowState.GetProperty(
            "recording",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );

        public static bool IsRecording() {
            return Resources.FindObjectsOfTypeAll(animationWindowState)
                .Any(window => (bool)isRecordingProperty.GetValue(window, null));
        }
    }
}
