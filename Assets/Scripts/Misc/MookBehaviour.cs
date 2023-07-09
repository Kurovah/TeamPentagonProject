using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.AI;

public class MookBehaviour : MonoBehaviourPunCallbacks
{
    public NavMeshAgent agent;
    List<GameObject> targets = new List<GameObject>();
    public GameObject deathEffect, hitEffect;
    public Collider hurtBox;
    public ParticleSystem DeathDust;
    bool isDead;

    public AudioSource hitAudioSource, deathAudioSouce;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.enabled = true;
        if (targets.Count > 0)
        {
            
            Vector3 des = targets[0].transform.position;

            foreach(var target in targets)
            {
                if( Vector3.Distance(transform.position, target.transform.position) < Vector3.Distance(transform.position, target.transform.position))
                {
                    des = target.transform.position;
                }
            }

            agent.destination = des;
        }
    }

    public void SearchTriggerEntered(Collider other)
    {
        if (other.gameObject.CompareTag("Ranger"))
            targets.Add(other.gameObject);
    }

    public void SearchTriggerExit(Collider other)
    {
        if (targets.Contains(other.gameObject))
            targets.Remove(other.gameObject);
    }

    public void HitTriggerEntered(Collider other)
    {
        if (other.gameObject.CompareTag("Ranger"))
            other.gameObject.GetComponent<RangerBehaviour>().GetHurt();
    }

    public void HurtTriggerEntered(Collider other)
    {
        Vector3 vel = transform.position - other.gameObject.transform.position;
        vel.y = 0;
        if (other.gameObject.CompareTag("Hit") && !isDead)
        {
            hitAudioSource.Play();
            DestroyEnemy(vel.normalized * 30);
        }
            
    }

    [PunRPC]
    public void DestroyEnemy(Vector3 velocity)
    {
        Debug.Log("Hit");
        StartCoroutine(SlideAndDeath(velocity));
        if (photonView.IsMine)
        {
            photonView.RPC("DestroyEnemy", RpcTarget.OthersBuffered, velocity);
        }
    }

    IEnumerator SlideAndDeath(Vector3 velocity)
    {
        hurtBox.enabled = false;
        agent.enabled = false;
        DeathDust.Play();
        GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
        yield return new WaitForSeconds(.3f);
        DeathDust.gameObject.transform.parent = null;
        deathAudioSouce.Play();
        PhotonNetwork.Instantiate("Effects/" + deathEffect.name, transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(gameObject);
    }
}
