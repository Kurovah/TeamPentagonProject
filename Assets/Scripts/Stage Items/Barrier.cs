using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject Mesh;
    public ParticleSystem destroyEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BreakBarrier()
    {
        Mesh.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
        if(destroyEffect != null)
        {
            destroyEffect.Play();
        }
    }
}
