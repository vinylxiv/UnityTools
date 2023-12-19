using System;
using UnityEngine;
using UnityEngine.Serialization;
using VF.Component;

namespace VF.Model.StateAction {
    [Serializable]
    public class Action {
    }
    
    [Serializable]
    public class ObjectToggleAction : Action {
        public GameObject obj;
    }
    
    [Serializable]
    public class BlendShapeAction : Action {
        public string blendShape;
        public float blendShapeValue = 100;
    }
    
    [Serializable]
    public class MaterialAction : Action {
        public GameObject obj;
        public int materialIndex = 0;
        public GuidMaterial mat = null;
    }

    [Serializable]
    public class SpsOnAction : Action {
        public VRCFuryHapticPlug target;
    }
    
    [Serializable]
    public class FxFloatAction : Action {
        public string name;
        public float value = 1;
    }
    
    [Serializable]
    public class AnimationClipAction : Action {
        public GuidAnimationClip clip;
    }

    [Serializable]
    public class ShaderInventoryAction : Action {
        public Renderer renderer;
        public int slot = 1;
    }

    [Serializable]
    public class PoiyomiUVTileAction : Action {
        public Renderer renderer;
        public int row = 0;
        public int column = 0;
        public bool dissolve = false;
        public string renamedMaterial = "";
    }
    
    [Serializable]
    public class MaterialPropertyAction : Action {
        public Renderer renderer;
        public bool affectAllMeshes;
        public string propertyName;
        public float value;
    }
    
    [Serializable]
    public class FlipbookAction : Action {
        public GameObject obj;
        public int frame;
    }
    
    [Serializable]
    public class ScaleAction : Action {
        public GameObject obj;
        public float scale = 1;
    }

}
