using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
    public UnityEvent pressedAction, releasedAction;
    public enum ReleaseType
    {
        immmediateRelease,
        timedRelease,
        noRelease
    }
    public ReleaseType releaseType;
    List<GameObject> objectsOnButton = new List<GameObject>();
    public Transform buttonTransform;
    public int depressionTime;
    Coroutine depressionTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButtonDown()
    {
        buttonTransform.localPosition = new Vector3(0, -0.17f, 0);
    }

    public void PushButtonUp()
    {
        buttonTransform.localPosition = new Vector3(0, 0.15f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Rigidbody>() != null && objectsOnButton.Count == 0)
        {
            pressedAction?.Invoke();
            if (depressionTimer != null)
                StopCoroutine(depressionTimer);
        }
    }

    IEnumerator DepressionTimer()
    {
        yield return new WaitForSecondsRealtime(depressionTime);
        releasedAction?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (objectsOnButton.Contains(other.gameObject))
        {
            objectsOnButton.Remove(other.gameObject);
        }

        if(objectsOnButton.Count <= 0)
        {
            Debug.Log("Release");
            switch (releaseType)
            {
                case ReleaseType.immmediateRelease:
                    releasedAction?.Invoke();
                    break;
                case ReleaseType.timedRelease:
                    depressionTimer = StartCoroutine(DepressionTimer());
                    break;
            }
        }
    }
}
