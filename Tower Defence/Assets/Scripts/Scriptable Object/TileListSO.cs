using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence.Tile
{
    [CreateAssetMenu(fileName = "TileTypeList", menuName = "SO/TileTypeList")]
    public class TileListSO : ScriptableObject
    {
        public List<TileTypeData> tileTypesList;
    }
}
