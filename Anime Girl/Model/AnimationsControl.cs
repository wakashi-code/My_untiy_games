using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsControl : MonoBehaviour
{
    private Animator _Jump;
    private Animator _Run;
    private Animator _Dance;
    private Animator _Reload_rifle;
    public Kabach_DamageReceiver player;


    void Start()
    {
        _Jump = GetComponent<Animator>();
        _Run = GetComponent<Animator>();
        _Dance = GetComponent<Animator>();
        _Reload_rifle = GetComponent<Animator>();
        _Reload_rifle= GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Kabach_DamageReceiver>();

    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            _Dance.SetBool("DanceAnimation", true);
        }
        else
        {
            _Dance.SetBool("DanceAnimation", false);
        }



        if (Input.GetKey(KeyCode.Space))
        {
            _Jump.SetBool("JumpAnimation", true);
        }
        else
        {
            _Jump.SetBool("JumpAnimation", false);
        }


        if (Input.GetKey (KeyCode.LeftShift))
        {
            _Run.SetBool("RunAnimation", true);
        }
        else
        {
            _Run.SetBool("RunAnimation", false);
        }

        if (Input.GetKey(KeyCode.R) && player.weaponManager.selectedWeapon == player.weaponManager.secondaryWeapon)
        {
            _Reload_rifle.SetBool("Reload_rifle_flag", true);
        }
        else
        {
            _Reload_rifle.SetBool("Reload_rifle_flag", false);
        }


    }


}
