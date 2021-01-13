using System;
using UnityEngine;
using Lean.Pool;
using Units;

public class Bullet : ShellBase
{   
    private float distance;

    void Start()
    {
    }
    
	// Use this for initialization
	void OnEnable ()
    {
        distance = 0.0f;
    }

    // Update is called once per frame
	void FixedUpdate ()
    {
        var diff = Time.deltaTime * Speed;
        distance += diff;
        transform.position += (Vector3)Direction * diff;

        if(distance > Range)
        {
            LeanPool.Despawn(gameObject);
        }
	}
}
