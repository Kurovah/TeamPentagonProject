using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventExposer : MonoBehaviour
{

    public UnityEvent<Collider> triggerEnterEvents;
    public UnityEvent<Collider> triggerExitEvents;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        triggerEnterEvents?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        triggerExitEvents?.Invoke(other);
    }
}
