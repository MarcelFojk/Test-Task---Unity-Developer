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

        private void Awake()
        {
            _meshFilter.mesh = _meshGenerator.GetCubeSphere(_resolution, _size);
        }
    }
}