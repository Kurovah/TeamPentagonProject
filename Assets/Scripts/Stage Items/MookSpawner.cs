using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MookSpawner : MonoBehaviour
{
    public GameObject mookObject;
    bool isActive;
    Coroutine spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MookSpawnerTimer(float secs)
    {
        yield return new WaitForSecondsRealtime(secs);
        SpawnMook();

        spawnTimer = StartCoroutine(MookSpawnerTimer(10));
    }

    void SpawnMook()
    {
        Instantiate(mookObject, transform.position, transform.rotation);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ranger")
        {
            isActive = true;
            spawnTimer = StartCoroutine(MookSpawnerTimer(5));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ranger")
        {
            isActive = false;
            StopCoroutine(spawnTimer);
        }
    }
}
