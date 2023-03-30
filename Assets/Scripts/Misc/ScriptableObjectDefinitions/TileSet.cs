using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTileSets", menuName = "DevAssets")]
public class TileSet : ScriptableObject
{
    public List<GameObject> tileObjects = new List<GameObject>();
}
