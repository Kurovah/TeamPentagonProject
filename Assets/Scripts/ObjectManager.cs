using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectManager : MonoBehaviourPunCallbacks
{
    public Vector3 place;
    public GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(place, 1);
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            PhotonNetwork.Instantiate(objectToSpawn.name, place, Quaternion.identity); base.OnJoinedRoom();
    }
}
