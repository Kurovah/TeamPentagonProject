using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPodium : MonoBehaviour
{
    public enum ERangerAbilities:int
    {
        tele,
        warp,
    }

    public ERangerAbilities heldAbility;
    bool isActive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ranger" && isActive)
        {
            other.gameObject.GetComponent<RangerBehaviour>().GiveAbillity((int)heldAbility);
            isActive = false;
        }
    }
}
