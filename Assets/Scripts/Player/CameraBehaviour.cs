using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
public class CameraBehaviour : MonoBehaviour
{
    public float zoom;
    public Transform target;
    CinemachineVirtualCamera vcamera;


    // Start is called before the first frame update
    void Start()
    {
        vcamera = GetComponent<CinemachineVirtualCamera>();
        vcamera.LookAt = target;
        vcamera.Follow = target;
    }
}
