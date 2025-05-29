using UnityEngine;

namespace Components
{
    public class RotationComponent : MonoBehaviour
    {
        [SerializeField] private Transform _objectAFollowTransform;
        [SerializeField] private Transform _objectBTransform;
        [SerializeField] private Transform _objectATransform;
        [SerializeField] private float _angularSpeed = 1f;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _lookRotationSpeed = 180f;

        private float _angle = 0f;

        public void RotationUpdate(bool isRotationAround, float deltaTime)
        {
            if (isRotationAround)
            {
                RotateAround(deltaTime);
            }

            LookRotate(deltaTime);
        }

        private void RotateAround(float deltaTime)
        {
            _angle += _angularSpeed * deltaTime;

            var offset = _objectAFollowTransform.position;
            offset.x = _radius * Mathf.Cos(_angle);
            offset.z = _radius * Mathf.Sin(_angle);
            offset.y = 0f;
            _objectAFollowTransform.position = offset + _objectBTransform.parent.position;
        }

        private void LookRotate(float deltaTime)
        {
            Vector3 direction = (_objectBTransform.position - _objectATransform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _objectATransform.rotation =
                Quaternion.RotateTowards(_objectATransform.rotation, targetRotation, _lookRotationSpeed * deltaTime);
        }
    }
}