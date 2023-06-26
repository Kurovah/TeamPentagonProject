using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Battle pass", menuName = "Game Data/BattlePass")]
public class PassData : ScriptableObject
{
    public string passName;
    public List<PassItem> passItems = new List<PassItem>();
}
