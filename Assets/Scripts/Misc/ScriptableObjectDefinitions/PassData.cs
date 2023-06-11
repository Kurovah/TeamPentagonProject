using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassData : ScriptableObject
{
    public string passName;
    List<PassItem> passItems = new List<PassItem>();
}
