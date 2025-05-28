using MeshGeneration;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ObjectA : MonoBehaviour
    {
        [SerializeField] private MeshGenerator _meshGenerator;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField, Range(1, 1000)] private int _resolution = 1;
        [SerializeField, Range(0.2f, 20f)] private float _size = 1f;

        [SerializeField] private Transform _objectBLocalTransform;
        [SerializeField] private Material _objectAMaterial;

        private void Awake()
        {
            _meshFilter.mesh = _meshGenerator.GetCubeSphere(_resolution, _size);
        }

        private void Update()
        {
            _objectAMaterial.SetVector("_ObjectAForward", transform.forward);
            _objectAMaterial.SetVector("_VectorAToB", (transform.position - _objectBLocalTransform.position).normalized);
        }
    }
}