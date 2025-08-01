using System.Collections.Generic;
using UnityEngine;
namespace TowerDefence.Level
{
    [CreateAssetMenu(fileName = "WaypointList", menuName = "SO/WaypointList")]
    public class WaypointSO : ScriptableObject
    {
        public List<Transform> wapointList;
    }
}
