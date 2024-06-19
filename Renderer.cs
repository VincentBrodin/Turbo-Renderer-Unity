using UnityEngine;

namespace TurboRenderer{
    public class Renderer : MonoBehaviour{
        public string group;
        public MeshRenderer meshRenderer;
        private Matrix4x4 _matrix4X4;
        private Transform _transform;
        private int _id = -1;
        private bool _rendering;

        private void OnValidate(){
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start(){
            _transform = transform;
            _matrix4X4 = _transform.localToWorldMatrix;

            _id =RenderingManager.Singleton.Add(group, _matrix4X4);
            meshRenderer.enabled = false;
            _rendering = true;
        }

        private void Update(){
            if(!_rendering) return;
            _matrix4X4 = _transform.localToWorldMatrix;
            RenderingManager.Singleton.UpdateMatrix(group, _id, _matrix4X4);
        }
    }
}
