using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wintrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ranger")
        {
            MatchManager.instance.RangerWinCall();
        }
    }
}
