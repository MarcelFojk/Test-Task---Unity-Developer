using Components;
using Interfaces;
using MeshGeneration;
using UnityEngine;

namespace Entities
{
    public class ObjectB : MonoBehaviour, IObject
    {
        [Header("Mesh")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField, Range(1, 1000)] private int _mainAngleResolution = 100;
        [SerializeField, Range(1, 1000)] private int _sectionResolution = 30;
        [SerializeField, Range(0.2f, 20f)] private float _tubeRadius = 0.4f;
        [SerializeField, Range(0.2f, 20f)] private float _mainRadius = 1f;

        [Header("Components")]
        [SerializeField] private LissajousComponent _lissajousAnimation;

        private bool _lissajousAnimationEnabled = true;

        private void Awake()
        {
            _meshFilter.mesh = MeshGenerator.GetTorus(_mainAngleResolution, _sectionResolution, _tubeRadius, _mainRadius);
        }

        private void Update()
        {
            UpdateObjectsComponents(Time.deltaTime);
        }

        private void UpdateObjectsComponents(float deltaTime)
        {
            if (_lissajousAnimationEnabled) _lissajousAnimation.UpdateMovement(deltaTime);
        }

        public void SetActiveAnimation(bool active)
        {
            _lissajousAnimationEnabled = active;
        }
    }
}