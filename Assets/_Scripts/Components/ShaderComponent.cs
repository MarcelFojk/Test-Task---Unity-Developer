using UnityEngine;
using UnityEngine.UIElements;

namespace Components
{
    public class ShaderComponent : MonoBehaviour
    {
        private static readonly int ObjectAForward = Shader.PropertyToID("_ObjectAForward");
        private static readonly int VectorAToB = Shader.PropertyToID("_VectorAToB");

        [SerializeField] private Transform _objectBTransform;
        [SerializeField] private Renderer _objectRenderer;

        private MaterialPropertyBlock _materialPropertyBlock;
        private Transform _transform;

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _transform = transform;
        }

        public void UpdateObjectsShader()
        {
            _objectRenderer.GetPropertyBlock(_materialPropertyBlock);

            _materialPropertyBlock.SetVector(ObjectAForward, _transform.forward);
            _materialPropertyBlock.SetVector(VectorAToB, (_transform.position - _objectBTransform.position).normalized);

            _objectRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}