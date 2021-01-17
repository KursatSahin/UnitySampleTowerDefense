using Common;
using UnityEngine;

namespace Shells
{
    public class Rocket : ShellBase
    {
        [SerializeField] private Transform shadow;
        
        public float inertia;
        public float initialAcceleration;
        public Vector2 shadowOffset;

        private Vector2 _velocity;
        private float _acceleration;

        private float _startTime;

        #region Unity Events

        void OnEnable ()
        {
            shadow.position = transform.position;
            _acceleration = initialAcceleration;
            _startTime = 1.0f;
        }
    
        void FixedUpdate ()
        {
            if (_startTime >= 0)
            {
                _startTime -= Time.deltaTime;
                transform.rotation = gunBarrel.rotation;
                base.direction = gunBarrel.up;
                _velocity = base.direction.normalized;
                return;
            }

            if (target == null || !target.activeSelf)
            {
                Explode();
            }

            var direction = target.transform.position - transform.position;
            var angle = Utils.Angle(direction, transform.up) * Time.deltaTime * inertia * Mathf.Pow(_velocity.sqrMagnitude, 0.5f);

            _velocity = _velocity.Rotate(angle);
            _acceleration *= 0.95f;
            _velocity += _velocity * _acceleration * Time.deltaTime;
        
            transform.position += (Vector3)_velocity;
            transform.up = _velocity;

            shadow.position = transform.position + (Vector3)shadowOffset * (initialAcceleration - _acceleration) / initialAcceleration;
            shadow.rotation = transform.rotation;
        }
    
        #endregion
    }
}
