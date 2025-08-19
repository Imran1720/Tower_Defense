using System.Collections.Generic;
using UnityEngine;

public class GameService : MonoBehaviour
{
    public static GameService instance;

    [SerializeField] private Transform levelContainer;
    [SerializeField] private Transform waypointContainer;
    [SerializeField] private List<Transform> waypointList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public List<Transform> GetWaypoints() => waypointList;
    public Transform GetLevelContainer() => levelContainer;
    public Transform GetWaypointContainer() => waypointContainer;
}
