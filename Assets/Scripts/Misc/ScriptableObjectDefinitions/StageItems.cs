using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="stageItems", menuName = "DevAssets/stageItemList")]
public class StageItems : ScriptableObject
{
    public List<GameObject> stageObject = new List<GameObject>();
}
