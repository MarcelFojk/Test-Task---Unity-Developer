using UnityEngine;

namespace Components
{
    public class LissajousComponent : MonoBehaviour
    {
        private float _amplitudeXgoal;
        private float _amplitudeYgoal;
        private float _frequencyXgoal;
        private float _frequencyYgoal;

        private float _amplitudeXstart;
        private float _amplitudeYstart;
        private float _frequencyXstart;
        private float _frequencyYstart;

        private float _offsetX;
        private float _offsetY;

        private float _timeForNewParameters;
        private float _goalTime;

        private float _phaseX;
        private float _phaseY;

        private Transform _transform;

        private void Awake()
        {
            _offsetX = Random.Range(0.0f, 1000.0f);
            _offsetY = Random.Range(0.0f, 1000.0f);

            _transform = transform;
        }

        public void UpdateMovement(float deltaTime)
        {
            _timeForNewParameters -= deltaTime;
            float t = Mathf.Clamp01(1f - _timeForNewParameters / _goalTime);

            float amplitudeX = Mathf.Lerp(_amplitudeXstart, _amplitudeXgoal, t);
            float amplitudeY = Mathf.Lerp(_amplitudeYstart, _amplitudeYgoal, t);
            float frequencyX = Mathf.Lerp(_frequencyXstart, _frequencyXgoal, t);
            float frequencyY = Mathf.Lerp(_frequencyYstart, _frequencyYgoal, t);

            _phaseX += frequencyX * deltaTime;
            _phaseY += frequencyY * deltaTime;

            if (_timeForNewParameters <= 0)
            {
                _amplitudeXgoal = Random.Range(0.7f, 2f);
                _amplitudeYgoal = Random.Range(0.7f, 2f);
                _frequencyXgoal = Random.Range(0.4f, 1f);
                _frequencyYgoal = Random.Range(0.4f, 1f);

                _amplitudeXstart = amplitudeX;
                _amplitudeYstart = amplitudeY;
                _frequencyXstart = frequencyX;
                _frequencyYstart = frequencyY;

                _timeForNewParameters = Random.Range(0.5f, 5f);
                _goalTime = _timeForNewParameters;
            }

            Vector3 nextPosition = _transform.localPosition;
            nextPosition.x = amplitudeX * Mathf.Cos(_phaseX + _offsetX);
            nextPosition.y = amplitudeY * Mathf.Sin(_phaseY + _offsetY);
            _transform.localPosition = nextPosition;
        }
    }
}
