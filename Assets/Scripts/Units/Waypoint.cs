using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public sealed class Waypoint : MonoBehaviour
{
    [SerializeField] private List<Waypoint> _nextWaypoint;
    [SerializeField] private WaypointType _wType = WaypointType.Mid;

    public WaypointType WType => _wType;
    
    public Waypoint GetNextWaypoint()
    {
        if (_nextWaypoint.Count <= 0)
            throw new Exception("next waypoint is missing");
        
        return _nextWaypoint[Random.Range(0, _nextWaypoint.Count)];
    }

    public enum WaypointType
    {
        Start,
        Mid,
        Finish
    }
}
