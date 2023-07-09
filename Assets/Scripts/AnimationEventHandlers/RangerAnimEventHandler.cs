using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RangerAnimEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnityEvent showModelEvent, hideModelEvent, activateHB, deativateHB, returnToNormalEvent, playSlashEffect;

    public void ShowModel()
    {
        showModelEvent?.Invoke();
    }

    public void HideModel()
    {
        hideModelEvent?.Invoke();
    }

    public void ActivateAC()
    {
        activateHB?.Invoke();
    }
    public void DeactivateAC()
    {
        deativateHB?.Invoke();
    }
    public void BackToNormalState()
    {
        returnToNormalEvent?.Invoke();
    }

    public void PlaySlashEffect()
    {
        playSlashEffect?.Invoke();
    }
}
