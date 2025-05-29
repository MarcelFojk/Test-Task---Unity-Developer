using Components;
using Interfaces;
using MeshGeneration;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ObjectA : MonoBehaviour, IObject
    {
        [Header("Mesh")]
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField, Range(1, 1000)] private int _resolution = 1;
        [SerializeField, Range(0.2f, 20f)] private float _size = 1f;

        [Header("Components")]
        [SerializeField] private LissajousComponent _lissajousAnimation;
        [SerializeField] private RotationComponent _rotationAnimation;
        [SerializeField] private ShaderComponent _shaderComponent;

        private bool _isRotationAround = true;
        private bool _lissajousAnimationEnabled = true;

        private void Awake()
        {
            _meshFilter.mesh = MeshGenerator.GetCubeSphere(_resolution, _size);
        }

        private void Update()
        {
            UpdateObjectsComponents(Time.deltaTime);
        }

        private void UpdateObjectsComponents(float deltaTime)
        {
            _rotationAnimation.RotationUpdate(_isRotationAround, deltaTime);
            if (_lissajousAnimationEnabled) _lissajousAnimation.UpdateMovement(deltaTime);
            _shaderComponent.UpdateObjectsShader();
        }

        public void SetActiveAnimation(bool active)
        {
            _isRotationAround = active;
            _lissajousAnimationEnabled = active;
        }
    }
}