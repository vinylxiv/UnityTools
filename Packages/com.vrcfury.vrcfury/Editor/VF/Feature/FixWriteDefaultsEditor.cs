using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using VF.Builder;
using VF.Feature.Base;
using VF.Inspector;
using VF.Model.Feature;
using VRC.SDK3.Avatars.Components;

namespace VF.Feature {
    public class FixWriteDefaultsEditor : FeatureBuilder<FixWriteDefaults> {
        public override string GetEditorTitle() {
            return "Fix Write Defaults";
        }

        public override VisualElement CreateEditor(SerializedProperty prop) {
            var container = new VisualElement();
            container.Add(VRCFuryEditorUtils.Info(
                "This feature attempt to fix an avatar with a broken mix of Write Defaults."));
            container.Add(VRCFuryEditorUtils.Prop(prop.FindPropertyRelative("mode"), "Fix Mode"));
            container.Add(VRCFuryEditorUtils.Info(
                "Auto - Will force all states to on or off, whichever requires the fewest changes to the existing avatar\n" +
                "Force On - Forces all states to WD on\n" +
                "Force Off - Forces all states to WD off\n" +
                "Disabled - Don't try to fix anything and don't warn even if it looks broken"));
            
            container.Add(VRCFuryEditorUtils.Debug(refreshMessage: () => {
                var avatar = avatarObject.GetComponent<VRCAvatarDescriptor>();
                if (avatar == null) return "No avatar descriptor";
                var avatarControllers = VRCAvatarUtils.GetAllControllers(avatar)
                    .Where(c => c.controller != null)
                    .Select(c => Tuple.Create(c.type, c.controller));
                var analysis = FixWriteDefaultsBuilder.DetectExistingWriteDefaults(avatarControllers);

                var output = new List<string>();
                output.Add($"Auto mode = {(analysis.shouldBeOnIfWeAreInControl ? "WD on" : "WD off")}");
                output.Add($"Disabled compliance mode = {(analysis.shouldBeOnIfWeAreNotInControl ? "WD on" : "WD off")}");
                output.Add($"");
                output.Add($"Debug info: {analysis.debugInfo}");
                if (analysis.isBroken) {
                    output.Add("");
                    output.Add("Avatar base has broken mixed write defaults!");
                    output.Add("Here are the states that don't match:");
                    output.Add(string.Join("\n", analysis.weirdStates));
                }
                return string.Join("\n", output);
            }));
            
            return container;
        }

        public override bool AvailableOnProps() {
            return false;
        }
        
        public override bool OnlyOneAllowed() {
            return true;
        }
    }
}
