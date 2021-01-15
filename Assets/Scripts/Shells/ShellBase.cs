using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public abstract class ShellBase : MonoBehaviour
{
    [HideInInspector] public float speed;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public GameObject target;
    [HideInInspector] public float range;
    [HideInInspector] public float damage;
    [HideInInspector] public Transform gunBarrel;

    public virtual void Explode()
    {
        LeanPool.Despawn(gameObject);
    }
}