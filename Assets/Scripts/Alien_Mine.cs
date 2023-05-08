using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien_Mine : MonoBehaviour
{
    public ParticleSystem restingParticles, explosionSystem;
    public int life = 1;
    List<GameObject> hasHit = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ranger")
        {
            restingParticles.Stop();
            explosionSystem.Play();
            StartCoroutine(LifeEnd());
        }
    }

    IEnumerator LifeEnd()
    {
        var hits = Physics.OverlapSphere(transform.position, 1);

        foreach (var i in hits)
        {
            if(i.gameObject.tag == "Ranger" && !hasHit.Contains(i.gameObject))
            {
                i.GetComponent<RangerBehaviour>().GetHurt();
            }
        }
        yield return new WaitForSeconds(life);
    }
}
