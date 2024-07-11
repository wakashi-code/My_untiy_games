using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammo_pickup : MonoBehaviour
{
    public Kabach_DamageReceiver player;

    public GameObject ammo_korobka;
    public GameObject ammo_korobka_clone;

    public Ammo_spawner spawned_ammo_box;

 
   

    public void Start()
    {
        ammo_korobka = GameObject.Find("Ammo_box");
        player = GameObject.Find("Player").GetComponent<Kabach_DamageReceiver>();
        spawned_ammo_box = FindAnyObjectByType<Ammo_spawner>();
       
    }

    public void getAmmo(int points)
    {
        player.weaponManager.selectedWeapon.MaxAmmo += points; ;
    }
   

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player.weaponManager.selectedWeapon.MaxAmmo < player.weaponManager.selectedWeapon.MaxAmmoInGun)
        {

            if (player.weaponManager.selectedWeapon.MaxAmmo > player.weaponManager.selectedWeapon.MaxAmmoInGun)
            {
                
                int not_enough_ammo = player.weaponManager.selectedWeapon.MaxAmmoInGun - player.weaponManager.selectedWeapon.MaxAmmo;
                getAmmo(not_enough_ammo);
                
               
                Destroy(ammo_korobka);
                Destroy(ammo_korobka_clone);
                spawned_ammo_box.ammo_box_spawned = false;


            }
            else
            {
                
                getAmmo(player.weaponManager.selectedWeapon.bulletsPerMagazineDefault);
               
                
                Destroy(ammo_korobka);
                Destroy(ammo_korobka_clone);
                spawned_ammo_box.ammo_box_spawned = false;

            }

        }
       
    }

    public void Update()
    {
        ammo_korobka_clone = GameObject.FindGameObjectWithTag("Ammo");


    }
}