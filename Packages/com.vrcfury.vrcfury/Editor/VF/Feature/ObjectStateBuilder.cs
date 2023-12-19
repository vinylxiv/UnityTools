using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VF.Builder;
using VF.Feature.Base;
using VF.Injector;
using VF.Inspector;
using VF.Model.Feature;

namespace VF.Feature {
    public class ObjectStateBuilder : FeatureBuilder<ObjectState> {
        [VFAutowired] private readonly RestingStateBuilder restingState;
        
        [FeatureBuilderAction(FeatureOrder.ForceObjectState)]
        public void Apply() {
            foreach (var state in model.states) {
                VFGameObject obj = state.obj;
                if (!obj) continue;
                switch (state.action) {
                    case ObjectState.Action.DELETE:
                        obj.Destroy();
                        break;
                    case ObjectState.Action.ACTIVATE:
                        var clip = new AnimationClip();
                        clipBuilder.Enable(clip, obj);
                        restingState.ApplyClipToRestingState(clip);
                        break;
                    case ObjectState.Action.DEACTIVATE:
                        var clip2 = new AnimationClip();
                        clipBuilder.Enable(clip2, obj, false);
                        restingState.ApplyClipToRestingState(clip2);
                        break;
                }
            }
        }
        
        public override string GetEditorTitle() {
            return "Force Object State";
        }

        public override VisualElement CreateEditor(SerializedProperty prop) {
            var content = new VisualElement();

            content.Add(VRCFuryEditorUtils.Info(
                "This feature will activate, deactivate, or delete the specified objects during upload."));

            content.Add(VRCFuryEditorUtils.List(prop.FindPropertyRelative("states"), (i, el) => {
                var c = new VisualElement();
                c.style.flexDirection = FlexDirection.Row;
                var a = VRCFuryEditorUtils.Prop(el.FindPropertyRelative("obj"));
                a.style.flexBasis = 0;
                a.style.flexGrow = 1;
                var b = VRCFuryEditorUtils.Prop(el.FindPropertyRelative("action"));
                b.style.flexBasis = 0;
                b.style.flexGrow = 1;
                c.Add(a);
                c.Add(b);
                return c;
            }));

            return content;
        }
    }
}