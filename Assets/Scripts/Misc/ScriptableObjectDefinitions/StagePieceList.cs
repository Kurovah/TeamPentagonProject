using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage Listing", menuName = "DevAssets/stageLayoutList")]
public class StagePieceList : ScriptableObject
{
    public GameObject startPiece;
    public GameObject endPiece;
    public List<GameObject> stageLayouts = new List<GameObject>();
    public List<GameObject> part1Layouts = new List<GameObject>();
    public List<GameObject> part2Layouts = new List<GameObject>();
    public List<GameObject> part3Layouts = new List<GameObject>();
    public List<GameObject> part4Layouts = new List<GameObject>();
}
