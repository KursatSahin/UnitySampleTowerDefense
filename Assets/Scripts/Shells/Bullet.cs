using System;
using UnityEngine;
using Lean.Pool;
using Units;

public class Bullet : ShellBase
{   
    private float _distance;

    #region Unity Events
    
	void OnEnable ()
    {
        _distance = 0.0f;
    }

	void FixedUpdate ()
    {
        var diff = Time.deltaTime * speed;
        _distance += diff;
        transform.position += (Vector3)direction * diff;

        if(_distance > range)
        {
            Explode();
        }
	}
    #endregion
}
