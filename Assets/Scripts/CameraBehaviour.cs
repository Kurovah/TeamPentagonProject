using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraBehaviour : MonoBehaviour
{
    public float zoom;
    public Transform target, vcam;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetCamPos();
    }

    void SetCamPos()
    {
        if (target == null)
            return;


        transform.position = target.position + offset;
        vcam.localPosition = new Vector3(0,0,-zoom);
        vcam.LookAt(target.position);
    }
}
