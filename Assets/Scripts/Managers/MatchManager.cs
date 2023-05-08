using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public static MatchManager instance;
    public Transform r1spawn, r2spawn, a1spawn, a2spawn;
    public GameObject rangerObject, rangerCam, alienObject, alienCam;

    public int rangerHP = 6;
    public int alienResource = 0;

    [Header("OutcomeBanners")]
    public GameObject outcomeBanner;
    public GameObject loserText;
    public GameObject winnerText;
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
        
        outcomeBanner.SetActive(true);


        bool isWinner = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerTeam"] == 0;

        GameManager.instance.ChangeCurrency(isWinner? 10: 5);
        loserText.SetActive(!isWinner);
        winnerText.SetActive(isWinner);
        NetworkingManager.instance.SetReady(false);
    }
    public void AlienWin()
    {
        outcomeBanner.SetActive(true);
        bool isWinner = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerTeam"] == 1;

        GameManager.instance.ChangeCurrency(isWinner ? 10 : 5);
        loserText.SetActive(!isWinner);
        winnerText.SetActive(isWinner);
        NetworkingManager.instance.SetReady(false);
    }

    public void ChangeRangerHP(int amount)
    {
        rangerHP += amount;
        CheckRangersDead();
        if (photonView.IsMine)
        {
            photonView.RPC("ChangeRangerHPRPC", RpcTarget.OthersBuffered);
        }
        
    }
    void CheckRangersDead()
    {
        if(rangerHP == 0)
        {
            AlienWin();
        }
    }

    [PunRPC]
    public void ChangeAlienResource(int amount)
    {
        alienResource += amount;
        if (photonView.IsMine)
        {
            photonView.RPC("ChangeAlienResource", RpcTarget.OthersBuffered);
        }
        
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

        var a = PhotonNetwork.Instantiate(g.name, tr.position, tr.rotation);
        if(t==0 && isOtherMember)
        {
            a.GetComponent<RangerBehaviour>().GiveAbillity(1);
        }
        SceneManager.MoveGameObjectToScene(a, gameObject.scene);
    }

    public void BackToLobby()
    {
        NetworkingManager.instance.BackToLobby();
    }
}
