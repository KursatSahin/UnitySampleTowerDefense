using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public sealed class Waypoint : MonoBehaviour
{
    [SerializeField] private List<Waypoint> nextWaypoint;
    [SerializeField] private WaypointType wType = WaypointType.Mid;

    public WaypointType WType => wType;
    
    public Waypoint GetNextWaypoint()
    {
        if (nextWaypoint.Count <= 0)
            throw new Exception("next waypoint is missing");
        
        return nextWaypoint[Random.Range(0, nextWaypoint.Count)];
    }

    public enum WaypointType
    {
        Start,
        Mid,
        Finish
    }
}
