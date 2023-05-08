using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ranger")
        {
            other.gameObject.GetComponent<RangerBehaviour>().Respawn();
        }
    }
}
