using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ColourList", menuName ="CustomisationLists/Colours")]
public class ColourList : ScriptableObject
{
    public List<ColorListing> colors = new List<ColorListing>();
}
