using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpAbility : MonoBehaviourPunCallbacks, IRangerAbility
{
    public float warpTime;
    Transform playerTransform;
    bool abilityActive = false;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponentInParent<BaseRanger>().transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (abilityActive)
        {
            Vector2 InputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            OnInput(InputAxis);
            transform.localPosition += transform.forward * 10 * Time.deltaTime;
        }
    }

    public void OnAbilityStart()
    {
        Debug.Log("Starting ability");
        abilityActive = true;
        StartCoroutine(WarpHoldTimer());
            
    }


    public void OnAbilityEnd()
    {
        if(abilityActive)
        {
            abilityActive = false;
            SetUserPosition(transform.position - Vector3.up / 2);
            transform.position = playerTransform.position + Vector3.up / 2;
        }
    }

    
    public void OnInput(Vector2 input)
    {
        Vector3 rightVec = Vector3.Cross(-Camera.main.transform.forward, Vector3.up );
        Vector3 forwardVec = Vector3.Cross( rightVec, Vector3.up );
        Vector3 dirVec = rightVec * input.x + forwardVec * input.y;
        transform.forward = Vector3.Lerp(transform.forward, dirVec, 0.5f);
    }

    [PunRPC]
    void SetUserPosition(Vector3 newPos)
    {
        playerTransform.position = newPos;
        if (photonView.IsMine)
        {
            photonView.RPC("SetUserPosition", RpcTarget.OthersBuffered, newPos);
        }
    }

    IEnumerator WarpHoldTimer()
    {
        yield return new WaitForSeconds(warpTime);
        OnAbilityEnd();
    }
}
