using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviourPunCallbacks
{
    public float initialVelocity;
    public float life;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BulletTimer());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * initialVelocity * Time.deltaTime;
    }

    IEnumerator BulletTimer()
    {
        yield return new WaitForSeconds(life);
        PhotonNetwork.Destroy(gameObject);
    }

    
}
