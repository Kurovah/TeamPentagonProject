using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableComponent : MonoBehaviour
{
    public bool isGrabbed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 0;
            Vector3 Worldpos = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = Vector3.Lerp(transform.position, Worldpos, 0.25f);
        }
    }
}
