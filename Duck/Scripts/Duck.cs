using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    private Animator duck;
    private bool CapsLockOn = false;
    public GameObject ground;
    // Start is called before the first frame update
    void Start()
    {
        duck = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (duck.GetCurrentAnimatorStateInfo(0).IsName("swim"))
        {
            ground.GetComponent<MeshRenderer>().enabled = false;
            duck.SetBool("walk", false);
        }
        if (duck.GetCurrentAnimatorStateInfo(0).IsName("lay"))
        {
            duck.SetBool("idletolay", false);
            duck.SetBool("laytoidle", false);
            ground.GetComponent<MeshRenderer>().enabled = true;
        }
        if (duck.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            ground.GetComponent<MeshRenderer>().enabled = true;
            duck.SetBool("lay", false);
            duck.SetBool("walk", false);
        }
        if (duck.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            ground.GetComponent<MeshRenderer>().enabled = true;
            duck.SetBool("swim", false);
        }
        if (duck.GetCurrentAnimatorStateInfo(0).IsName("run"))
        {
            ground.GetComponent<MeshRenderer>().enabled = true;
            duck.SetBool("idle", false);
            duck.SetBool("lay", false);
        }
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            CapsLockOn = !CapsLockOn;
        }
        if ((Input.GetKeyUp(KeyCode.W))||(Input.GetKeyUp(KeyCode.E)))
        {
            duck.SetBool("idle", true);
            duck.SetBool("walk", false);
            duck.SetBool("swim", false);
            duck.SetBool("lay", true);
            duck.SetBool("eat", false);
        }
        if ((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.D)))
        {
            duck.SetBool("walkleft", false);
            duck.SetBool("walkright", false);
            duck.SetBool("walk", true);
        }
        if (((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.D))))
        {
            duck.SetBool("runleft", false);
            duck.SetBool("idle", true);
            duck.SetBool("runright", false);
            duck.SetBool("run", true);
            duck.SetBool("turnleft", false);
            duck.SetBool("turnright", false);
            duck.SetBool("swimleft", false);
            duck.SetBool("swimright", false);
            duck.SetBool("swim", true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            duck.SetBool("walk", true);
            duck.SetBool("idle", false);
            duck.SetBool("run", false);
            duck.SetBool("swim", true);
            duck.SetBool("lay", false);
            duck.SetBool("swimleft", false);
            duck.SetBool("swimright", false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            duck.SetBool("runleft", true);
            duck.SetBool("run", false);
            duck.SetBool("walkleft", true);
            duck.SetBool("walk", false);
            duck.SetBool("turnleft", true);
            duck.SetBool("idle", false);
            duck.SetBool("swimleft", true);
            duck.SetBool("swim", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            duck.SetBool("runright", true);
            duck.SetBool("run", false);
            duck.SetBool("walkright", true);
            duck.SetBool("walk", false);
            duck.SetBool("turnright", true);
            duck.SetBool("idle", false);
            duck.SetBool("swimright", true);
            duck.SetBool("swim", false);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            duck.SetBool("idle", false);
            duck.SetBool("run", true);
            duck.SetBool("walk", false);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            duck.SetBool("idle", false);
            duck.SetBool("idletolay", true);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            duck.SetBool("lay", false);
            duck.SetBool("laytoidle", true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            duck.SetBool("idle", false);
            duck.SetBool("eat", true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            duck.SetBool("idle", false);
            duck.SetBool("backward", true);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            duck.SetBool("idle", true);
            duck.SetBool("backward", false);
        }
    }
}
