using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BaseRanger : MonoBehaviourPunCallbacks
{
    public Rigidbody playerRB;
    public Vector3 velocity;
    public GameObject bulletPrefab;
    public GameObject baton;
    public Animator animator;

    //public float jumpheight;
    public float  movespeed, gravity;
    float lastYVel;
    
    public Transform meshTransform;
    

    public enum CharacterStates
    {
        normal,
        usingAbility,
        attacking,
        hurt
    }

    public CharacterStates state = CharacterStates.normal;

    IRangerAbility abilityComponent;

    // Start is called before the first frame update
    void Start()
    {
        playerRB= GetComponent<Rigidbody>();
        abilityComponent = GetComponentInChildren<IRangerAbility>();
        HideModel();
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            CheckGround();
            grounded = hitList.Count > 0;

            if (grounded && velocity.y < 0)
            {
                velocity.y = 0;

                //snap to floor
                transform.position = new Vector3(
                    transform.position.x,
                    GetMaxHeight(),
                    transform.position.z
                    );
            }

            switch (state)
            {
                case CharacterStates.normal:
                    StateNormal();
                    break;
                case CharacterStates.usingAbility:
                    StateUsingAbility();
                    break;
                case CharacterStates.attacking:
                    StateAttacking();
                    break;
            }



            velocity.y -= gravity;
            playerRB.velocity = velocity;
            UpdateAnim();
        }
        
    }

    void StateNormal()
    {
        Vector3 rightVec = Vector3.Cross(-Camera.main.transform.forward, Vector3.up);
        Vector3 forwardVec = Vector3.Cross(rightVec, Vector3.up);


        
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        lastYVel = velocity.y;
        velocity = rightVec * inputAxis.x + forwardVec * inputAxis.y;

        moveFloat = Mathf.Abs(inputAxis.magnitude);

        velocity = velocity.normalized * movespeed;
        velocity.y = lastYVel;


        //JUMPING NOT NEEDED FOR NOW
        //if (grounded)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        velocity.y = jumpheight;
        //    }
        //}

        if (inputAxis.magnitude > 0)
            SetModelFacing(new Vector3 (inputAxis.x, 0, inputAxis.y));


        //attacking
        if (Input.GetKeyDown(KeyCode.K))
        {
            //PhotonNetwork.Instantiate(bulletPrefab.name, transform.position + Vector3.up, meshTransform.rotation);
            BeginAttack();
        }
            //using  ability
        if (Input.GetKeyDown(KeyCode.J))
        {
            BeginAbility();
        }
    }
    void StateUsingAbility()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            state= CharacterStates.normal;
            abilityComponent.OnAbilityEnd();
            abilityBool = false;
        }
    }
    void StateAttacking()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
        {
            CheckHit();
        }

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            HideModel();
            state = CharacterStates.normal;
        }
    }
    void StateHurt()
    {

    }
    void SetModelFacing(Vector3 _facing)
    {
        _facing.y = 0;
        meshTransform.forward = Vector3.Slerp(meshTransform.forward, _facing, 0.1f);
    }


    float moveFloat;
    bool abilityBool;
    private void UpdateAnim()
    {
        animator.SetFloat("Move", moveFloat);
        animator.SetBool("UsingAbility", abilityBool);
    }

    [Header("for ground check")]
    public bool grounded;
    public float rayStartHeight;
    public List<RaycastHit> hitList = new List<RaycastHit>();
    public float checkSpread;
    public float checkDistance;

    void CheckGround()
    {
        
        hitList.Clear();
        RaycastHit h1, h2, h3, h4, h5;
        
        if(Physics.Raycast(transform.position + new Vector3(0,rayStartHeight,0), Vector3.down, out h1,checkDistance,LayerMask.GetMask("Solid"), QueryTriggerInteraction.Ignore))
        {
            hitList.Add(h1);
        }

        //xpread
        if (Physics.Raycast(transform.position + new Vector3(checkSpread, rayStartHeight, 0), Vector3.down, out h2, checkDistance, LayerMask.GetMask("Solid"), QueryTriggerInteraction.Ignore))
        {
            hitList.Add(h2);
        }

        if (Physics.Raycast(transform.position + new Vector3(-checkSpread, rayStartHeight, 0), Vector3.down, out h3, checkDistance, LayerMask.GetMask("Solid"), QueryTriggerInteraction.Ignore))
        {
            hitList.Add(h3);
        }

        //zspread
        if (Physics.Raycast(transform.position + new Vector3(0, rayStartHeight, checkSpread), Vector3.down, out h4, checkDistance, LayerMask.GetMask("Solid"), QueryTriggerInteraction.Ignore))
        {
            hitList.Add(h4);
        }

        if (Physics.Raycast(transform.position + new Vector3(0, rayStartHeight, -checkSpread), Vector3.down, out h5, checkDistance, LayerMask.GetMask("Solid"), QueryTriggerInteraction.Ignore))
        {
            hitList.Add(h5);
        }
    }

    void BeginAttack()
    {
        animator.SetTrigger("Attack");
        ShowModel();
        state = CharacterStates.attacking;
    }

    void CheckHit()
    {

    }
    void BeginAbility()
    {
        velocity.x = velocity.z = 0;
        state = CharacterStates.usingAbility;
        abilityComponent.OnAbilityStart();
        abilityBool = true;
    }
    public void HideModel()
    {
        baton.SetActive(false);
    }
    public void ShowModel()
    {
        baton.SetActive(true);
    }
    private void OnDrawGizmos()
    {
        //central
        Gizmos.DrawLine(transform.position + new Vector3(0, rayStartHeight, 0),
            transform.position + new Vector3(0, rayStartHeight, 0) + Vector3.down * checkDistance);


        //x spread
        Gizmos.DrawLine(transform.position + new Vector3(checkSpread, rayStartHeight, 0),
            transform.position + new Vector3(checkSpread, rayStartHeight, 0) + Vector3.down * checkDistance);

        Gizmos.DrawLine(transform.position + new Vector3(-checkSpread, rayStartHeight, 0),
            transform.position + new Vector3(-checkSpread, rayStartHeight, 0) + Vector3.down * checkDistance);

        //z spread
        Gizmos.DrawLine(transform.position + new Vector3(0, rayStartHeight, checkSpread),
            transform.position + new Vector3(0, rayStartHeight, checkSpread) + Vector3.down * checkDistance);

        Gizmos.DrawLine(transform.position + new Vector3(0, rayStartHeight, -checkSpread),
            transform.position + new Vector3(0, rayStartHeight, -checkSpread) + Vector3.down * checkDistance);
    }

    float GetMaxHeight()
    {
        if(hitList.Count > 0)
        {
            float h = hitList[0].point.y;
            foreach(var hit in hitList)
            {
                if (hit.point.y > h) { h = hit.point.y; }
            }

            return h;
        }
        
        return 0;
    }
}
