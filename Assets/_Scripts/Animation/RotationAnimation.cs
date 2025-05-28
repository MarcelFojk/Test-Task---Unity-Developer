using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class RotationAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _localObjectB;
        [SerializeField] private Transform _localObjectA;
        [SerializeField] private float _angularSpeed = 1f;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _lookRotationSpeed = 180f;

        private float _angle = 0f;

        private void Update()
        {
            _angle += _angularSpeed * Time.deltaTime;

            var offset = transform.position;
            offset.x = _radius * Mathf.Cos(_angle);
            offset.z = _radius * Mathf.Sin(_angle);
            offset.y = 0f;
            transform.position = offset + _localObjectB.parent.position;

            Vector3 direction = (_localObjectB.position - _localObjectA.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _localObjectA.rotation = 
                Quaternion.RotateTowards(_localObjectA.rotation, targetRotation, _lookRotationSpeed * Time.deltaTime);
        }
    }
}