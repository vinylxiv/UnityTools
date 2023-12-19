using System.Collections.Generic;
using System.Linq;
using VF.Builder;
using VF.Builder.Exceptions;
using VF.Feature.Base;
using VF.Injector;
using VF.Utils;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;
using AnimatorStateExtensions = VF.Builder.AnimatorStateExtensions;

namespace VF.Feature {
    /**
     * This builder is responsible for scanning the generated controllers, and complaining if you've done something bad
     * (like using two separate locomotion controllers as inputs).
     * It also handles other controller merge conflict issues, like making VRCPlayableLayerControl only affect
     * the layers from the controller that triggered it.
     */
    public class ControllerConflictBuilder : FeatureBuilder {
        [VFAutowired] private readonly AnimatorLayerControlOffsetBuilder animatorLayerControlManager;
        
        [FeatureBuilderAction(FeatureOrder.ControllerConflictCheck)]
        public void Apply() {

            var singleOwnerTypes = new HashSet<VRCAvatarDescriptor.AnimLayerType>() {
                VRCAvatarDescriptor.AnimLayerType.Base,
                VRCAvatarDescriptor.AnimLayerType.TPose,
                VRCAvatarDescriptor.AnimLayerType.IKPose,
                VRCAvatarDescriptor.AnimLayerType.Sitting
            };
            
            foreach (var controller in manager.GetAllUsedControllers()) {
                var type = controller.GetType();
                if (!singleOwnerTypes.Contains(type)) continue;
                var uniqueOwners = new HashSet<string>();
                foreach (var layer in controller.GetLayers()) {
                    if (!controller.IsOwnerBaseAvatar(layer)) {
                        uniqueOwners.Add(controller.GetLayerOwner(layer));
                    }
                }
                if (uniqueOwners.Count > 1) {
                    throw new VRCFBuilderException(
                        "Your avatar contains multiple locomotion implementations." +
                        " You can only use one of these:\n\n" +
                        "Layer type: " + VRCFEnumUtils.GetName(type) + "\n" +
                        "Sources:\n" + string.Join("\n", uniqueOwners)
                    );
                }
                if (uniqueOwners.Count > 0) {
                    foreach (var layer in controller.GetLayers()) {
                        if (controller.IsOwnerBaseAvatar(layer)) {
                            controller.RemoveLayer(layer);
                        }
                    }
                }
            }

            var ownersByController = new Dictionary<VRCAvatarDescriptor.AnimLayerType, ISet<string>>();
            foreach (var controller in manager.GetAllUsedControllers()) {
                var type = controller.GetType();
                var uniqueOwners = new HashSet<string>();
                foreach (var layer in controller.GetLayers()) {
                    // Ignore empty layers (bask mask, junk layers, etc)
                    if (layer.stateMachine.defaultState == null) continue;
                    uniqueOwners.Add(controller.GetLayerOwner(layer));
                }
                ownersByController[type] = uniqueOwners;
            }

            foreach (var controller in manager.GetAllUsedControllers()) {
                foreach (var layer in controller.GetLayers()) {
                    var layerOwner = controller.GetLayerOwner(layer);
                    AnimatorIterator.ForEachBehaviourRW(layer, (b, add) => {
                        if (b is VRCPlayableLayerControl playableControl) {
                            var drivesTypeName = VRCFEnumUtils.GetName(playableControl.layer);
                            var drivesType = VRCFEnumUtils.Parse<VRCAvatarDescriptor.AnimLayerType>(drivesTypeName);
                            
                            // In theory, this should probably work for all types of controllers, but for some reason it doesn't.
                            // (see Hailey avatar, Gesture Controller, SB_FX Weight layer)
                            // For now, only worry about things driving action
                            if (drivesType != VRCAvatarDescriptor.AnimLayerType.Action) {
                                return true;
                            }

                            if (!ownersByController.TryGetValue(drivesType, out var uniqueOwnersOnType)) {
                                // They're driving a controller that doesn't exist?
                                // uhh... keep it I guess
                                return true;
                            }
                            if (!uniqueOwnersOnType.Contains(layerOwner)) return false;
                            if (uniqueOwnersOnType.Count == 1) return true;

                            var drivesController = manager.GetController(drivesType);
                            var drivesLayers = drivesController.GetLayers()
                                .Where(l => drivesController.GetLayerOwner(l) == layerOwner)
                                .ToList();
                            foreach (var drivesLayer in drivesLayers) {
                                var layerControl = (VRCAnimatorLayerControl)add(typeof(VRCAnimatorLayerControl));
                                layerControl.playable =
                                    VRCFEnumUtils.Parse<VRC_AnimatorLayerControl.BlendableLayer>(drivesTypeName);
                                layerControl.goalWeight = playableControl.goalWeight;
                                layerControl.blendDuration = 0;
                                layerControl.debugString = playableControl.debugString;
                                animatorLayerControlManager.Register(layerControl, drivesLayer);
                            }
                            return false;
                        }

                        return true;
                    });
                }
            }
            
            if (ownersByController.ContainsKey(VRCAvatarDescriptor.AnimLayerType.Action)
                && ownersByController[VRCAvatarDescriptor.AnimLayerType.Action].Count > 1) {
                var action = manager.GetController(VRCAvatarDescriptor.AnimLayerType.Action);
                // Make sure there's nothing on the base layer, since we won't be able to change its weight
                action.EnsureEmptyBaseLayer();
                var enableLayer = action.NewLayer("VRCF Force Enable", hasOwner: false);
                var enable = enableLayer.NewState("Enable");
                var enableControl = enable.GetRaw().VAddStateMachineBehaviour<VRCPlayableLayerControl>();
                enableControl.layer = VRC_PlayableLayerControl.BlendableLayer.Action;
                enableControl.goalWeight = 1;
                var i = 0;
                foreach (var layer in action.GetLayers()) {
                    var layerNum = i++;
                    if (layerNum != 0) layer.weight = 0;
                }
            }
            
            // TODO: Deal with conflicts when multiple owners:
            // * turn on/off locomotion
            // * turn on/off tracking
            // * turn on/off pose space
            // * re-enable the defaults layer on action when action is used?
        }
    }
}
