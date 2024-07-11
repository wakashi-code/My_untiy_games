using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kabach_WeaponManager : MonoBehaviour
{
    public Camera playerCamera;
    public Kabach_Weapon primaryWeapon;
    public Kabach_Weapon secondaryWeapon;
    public Kabach_Weapon max_ammo;
    public GameObject ammo;
  
    

   
    public Kabach_Weapon selectedWeapon;

    
    void Start()
    {
        primaryWeapon.ActiveWeapon(true);
        secondaryWeapon.ActiveWeapon(false);
        selectedWeapon = primaryWeapon;
        primaryWeapon.manager = this;
        secondaryWeapon.manager = this;
 

    }

   
    void Update()
    {
        ammo = GameObject.Find("Ammo_box");

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            primaryWeapon.ActiveWeapon(false);
            secondaryWeapon.ActiveWeapon(true);
            selectedWeapon = secondaryWeapon;

           
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            primaryWeapon.ActiveWeapon(true);
            secondaryWeapon.ActiveWeapon(false);
            selectedWeapon = primaryWeapon;
        }
/*
        if (mv > 0)
        {
            primaryWeapon.ActiveWeapon(false);
            secondaryWeapon.ActiveWeapon(true);
            selectedWeapon = primaryWeapon;
        }
        else
        {
            primaryWeapon.ActiveWeapon(true);
            secondaryWeapon.ActiveWeapon(false);
            selectedWeapon = secondaryWeapon;
        }*/
    }
}
