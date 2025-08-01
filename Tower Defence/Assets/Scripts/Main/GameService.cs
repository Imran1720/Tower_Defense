using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    public static GameService instance;

    [SerializeField] private Transform levelContainer;
    [SerializeField] private Transform waypointContainer;
    [SerializeField] private List<Transform> waypointList = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public Transform GetLevelContainer() => levelContainer;
    public Transform GetWaypointContainer() => waypointContainer;

    public void SetWaypoints(List<Transform> waypoints)
    {
        waypointList = waypoints;
    }

    public List<Transform> GetWaypoints() => waypointList;
}
