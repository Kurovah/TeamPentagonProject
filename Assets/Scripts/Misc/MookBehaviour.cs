using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MookBehaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    List<GameObject> targets = new List<GameObject>();
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
}
