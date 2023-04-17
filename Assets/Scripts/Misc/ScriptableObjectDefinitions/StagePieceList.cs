using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Listing", menuName = "DevAssets/stageLayoutList")]
public class StagePieceList : ScriptableObject
{
    public GameObject startPiece;
    public GameObject endPiece;
    public List<GameObject> stageLayouts = new List<GameObject>();
}
