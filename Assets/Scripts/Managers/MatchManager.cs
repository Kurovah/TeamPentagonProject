using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public Transform r1spawn, r2spawn, a1spawn, a2spawn;
    public GameObject rangerObject, rangerCam, alienObject, alienCam;
    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayerChar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlayerChar()
    {
        int t = NetworkingManager.instance.team;
        if (t == 1)
        { //alien
            PhotonNetwork.Instantiate(alienObject.name, r1spawn.position, r1spawn.rotation);
        }
        else
        {
            PhotonNetwork.Instantiate(rangerObject.name, a1spawn.position, a1spawn.rotation);
        }

    }
}
