using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public static MatchManager instance;
    public Transform r1spawn, r2spawn, a1spawn, a2spawn;
    public GameObject rangerObject, alienObject;

    public int rangerHP = 6;
    public int alienResource = 0;
    public int MatchTime;

    Coroutine countdownCR;
    public TMP_Text timerText;
    public TMP_Text beforeCurrency, afterCurrency, beforeBP, afterBP;

    bool matchConcluded = false;

    [Header("OutcomeBanners")]
    public GameObject outcomeBanner;
    public GameObject loserText;
    public GameObject winnerText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SpawnPlayerChar();
        MatchTime = 120;
        countdownCR = StartCoroutine(Countdown());
        SoundManager.instance.PlaySong(1);
    }

    IEnumerator Countdown()
    {
        while(MatchTime > 0)
        {
            MatchTime--;
            timerText.text = MatchTime.ToString();
            yield return new WaitForSeconds(1);
        }

        yield return null;
        AlienWin();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void RangerWin()
    {

        if (matchConcluded)
            return;

        matchConcluded = true;
        outcomeBanner.SetActive(true);
        StopCoroutine(countdownCR);

        bool isWinner = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerTeam"] == 0;

        
        loserText.SetActive(!isWinner);
        winnerText.SetActive(isWinner);
        NetworkingManager.instance.SetReady(false);
        UpdateStats(isWinner);
        if (photonView.IsMine)
        {
            photonView.RPC("RangerWin", RpcTarget.OthersBuffered);
        }
    }

    [PunRPC]
    public void AlienWin()
    {
        if (matchConcluded)
            return;

        matchConcluded = true;

        outcomeBanner.SetActive(true);
        bool isWinner = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerTeam"] == 1;
        loserText.SetActive(!isWinner);
        winnerText.SetActive(isWinner);
        NetworkingManager.instance.SetReady(false);
        UpdateStats(isWinner);
        if (photonView.IsMine)
        {
            photonView.RPC("AlienWin", RpcTarget.OthersBuffered);
        }
    }

    void UpdateStats(bool winner)
    {
        beforeCurrency.text = GameManager.instance.playerData.medals.ToString();
        GameManager.instance.ChangeCurrency(winner ? 10 : 5);
        afterCurrency.text = GameManager.instance.playerData.medals.ToString();

        beforeCurrency.text = GameManager.instance.playerData.battlePassExp.ToString();
        GameManager.instance.ChangeBPExp(winner ? 10 : 5);
        afterCurrency.text = GameManager.instance.playerData.battlePassExp.ToString();

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
            photonView.RPC("ChangeAlienResource", RpcTarget.OthersBuffered, amount);
        }
        
    }
  

    void SpawnPlayerChar()
    {
        int t = NetworkingManager.instance.GetTeam();
        int headindex = (int)PhotonNetwork.LocalPlayer.CustomProperties["HeadItem"];
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
        if(t==0)
        {
            a.GetComponent<RangerBehaviour>().PlaceHeadGear(headindex);
            if (isOtherMember)
                a.GetComponent<RangerBehaviour>().GiveAbillity(1);
        }
        SceneManager.MoveGameObjectToScene(a, gameObject.scene);
    }

    public void BackToLobby()
    {
        SoundManager.instance.PlaySong(0);
        NetworkingManager.instance.BackToLobby();
    }
}
