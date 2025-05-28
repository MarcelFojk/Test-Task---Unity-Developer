using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class LissajousAnimation : MonoBehaviour
    {
        private float _amplitudeX;
        private float _amplitudeY;
        private float _frequencyX;
        private float _frequencyY;

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

        private bool _isPlaying = true;

        private void Awake()
        {
            _offsetX = Random.Range(0.0f, 1000.0f);
            _offsetY = Random.Range(0.0f, 1000.0f);
        }

        public void PauseAnimation()
        {
            _isPlaying = false;
        }

        public void ContinueAnimation()
        {
            _isPlaying = true;
        }

        private void Update()
        {
            if (!_isPlaying) return;

            _timeForNewParameters -= Time.deltaTime;
            float t = Mathf.Clamp01(1f - _timeForNewParameters / _goalTime);

            _amplitudeX = Mathf.Lerp(_amplitudeXstart, _amplitudeXgoal, t);
            _amplitudeY = Mathf.Lerp(_amplitudeYstart, _amplitudeYgoal, t);
            _frequencyX = Mathf.Lerp(_frequencyXstart, _frequencyXgoal, t);
            _frequencyY = Mathf.Lerp(_frequencyYstart, _frequencyYgoal, t);

            _phaseX += _frequencyX * Time.deltaTime;
            _phaseY += _frequencyY * Time.deltaTime;

            if (_timeForNewParameters <= 0)
            {
                _amplitudeXgoal = Random.Range(0.5f, 1.5f);
                _amplitudeYgoal = Random.Range(0.5f, 1.5f);
                _frequencyXgoal = Random.Range(0.3f, 1f);
                _frequencyYgoal = Random.Range(0.3f, 1f);

                _amplitudeXstart = _amplitudeX;
                _amplitudeYstart = _amplitudeY;
                _frequencyXstart = _frequencyX;
                _frequencyYstart = _frequencyY;

                _timeForNewParameters = Random.Range(0.5f, 5f);
                _goalTime = _timeForNewParameters;
            }

            Vector3 nextPosition = transform.localPosition;
            nextPosition.x = _amplitudeX * Mathf.Cos(_phaseX + _offsetX);
            nextPosition.y = _amplitudeY * Mathf.Sin(_phaseY + _offsetY);
            transform.localPosition = nextPosition;
        }
    }
}
