using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien_Mine : MonoBehaviourPunCallbacks
{
    public ParticleSystem restingParticles, explosionSystem;
    public int life = 1;
    List<GameObject> hasHit = new List<GameObject>();
    bool hasExploded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasExploded && !explosionSystem.IsAlive() && PhotonNetwork.MasterClient == PhotonNetwork.LocalPlayer)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ranger" && !hasExploded)
        {
            Explode();
        }
    }

    [PunRPC]
    public void Explode()
    {
        restingParticles.Stop();
        explosionSystem.Play();
        var hits = Physics.OverlapSphere(transform.position, 1);
        hasExploded = true;

        foreach (var i in hits)
        {
            if (i.gameObject.tag == "Ranger" && !hasHit.Contains(i.gameObject))
            {
                i.GetComponent<RangerBehaviour>().GetHurt();
            }
        }

        if (photonView.IsMine)
        {
            photonView.RPC("Explode", RpcTarget.OthersBuffered);
        }
    }
}
