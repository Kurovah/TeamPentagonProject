using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public static MatchManager instance;
    public Transform r1spawn, r2spawn, a1spawn, a2spawn;
    public GameObject rangerObject, rangerCam, alienObject, alienCam;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SpawnPlayerChar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RangerWin()
    {

    }

    void SpawnPlayerChar()
    {
        int t = NetworkingManager.instance.GetTeam();
        bool isOtherMember  = false;
        //check if you are the second player for a group
        foreach(Player p in PhotonNetwork.PlayerList)
        {
            if((int)p.CustomProperties["playerTeam"] == NetworkingManager.instance.GetTeam())
            {
                //if the first player on your team is not you then you are the other player
                isOtherMember = p != PhotonNetwork.LocalPlayer;
                break;
            }
        }

        GameObject g;
        Transform tr;
        if(t == 1)
        {
            g = alienObject;
            if (isOtherMember)
            {
                tr = a2spawn;
            } else
            {
                tr = a1spawn;
            }
        } else
        {
            g = rangerObject;
            if (isOtherMember)
            {
                tr = r2spawn;
            }
            else
            {
                tr = r1spawn;
            }
        }

        PhotonNetwork.Instantiate(g.name, tr.position, tr.rotation);
    }
}
