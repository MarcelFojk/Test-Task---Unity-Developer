using MeshGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class ObjectB : MonoBehaviour
    {
        [SerializeField] private MeshGenerator _meshGenerator;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField, Range(1, 1000)] private int _mainAngleResolution = 100;
        [SerializeField, Range(1, 1000)] private int _sectionResolution = 30;
        [SerializeField, Range(0.2f, 20f)] private float _tubeRadius = 0.4f;
        [SerializeField, Range(0.2f, 20f)] private float _mainRadius = 1f;

        private void Awake()
        {
            _meshFilter.mesh = _meshGenerator.GetTorus(_mainAngleResolution, _sectionResolution, _tubeRadius, _mainRadius);
        }
    }
}