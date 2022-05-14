using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageAnimTest : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
/*
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            anim.SetBool("waitPlay", true);
            anim.SetFloat("speed", 4);
            print("Excité");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            anim.SetBool("eyeTracked", true);
            anim.SetFloat("speed", 8);
            print("InterExcité");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3)){
            anim.SetBool("trigger", true);
            
            print("Declenché");
        }
        else
        {
            //enable just if you use scene with just an anim
            *//*//print(this.anim.GetCurrentAnimatorStateInfo(0).IsName("Stationnaire"));
            if (anim.GetBool("waitPlay") && anim.GetBool("eyeTracked") && anim.GetBool("trigger") && this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                print("stationnaire");
                anim.SetFloat("speed", 1);
                anim.SetBool("trigger", false);
                anim.SetBool("waitPlay", false);
                anim.SetBool("eyeTracked", false);
            }*//*
        }
        
       *//* if (anim.GetBool("waitPlay") && anim.GetBool("eyeTracked") && anim.GetBool("trigger"))
        {
           
        }*/

    }

    public void Idle()
    {
        //print("Idle");
        anim.SetFloat("speed", 1);
        anim.SetBool("trigger", false);
        anim.SetBool("waitPlay", false);
        anim.SetBool("eyeTracked", false);
    }

    public void WaitPlay()
    {
        anim.SetBool("waitPlay", true);
        anim.SetBool("eyeTracked", false);
        anim.SetFloat("speed", 4);
       // print("WaitPlay");
    }

    public void EyeTracked()
    {
        anim.SetBool("eyeTracked", true);
        anim.SetFloat("speed", 8);
       // print("EyeTracked");
    }

    public void Trigger()
    {
        anim.SetBool("trigger", true);

        //print("Trigger");
    }

    public void ResetAnim()
    {
        anim.SetBool("waitPlay", false);
        anim.SetBool("eyeTracked", false);
        anim.SetBool("trigger", false);
        anim.SetFloat("speed", 1);
    }


    public bool currentStateAnim(string name)
    {
        return this.anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
