using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlienCamera : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetPos();
    }

    void SetPos() 
    {
        if (target == null)
            return;


        transform.position = target.position;
    }
}
