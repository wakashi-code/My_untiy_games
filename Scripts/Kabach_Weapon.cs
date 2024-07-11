using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;


[RequireComponent(typeof(AudioSource))]

public class Kabach_Weapon : MonoBehaviour
{
    public AudioClip fireAudio;
    public AudioClip reloadAudio;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject ammo_box;
    AudioSource audioSource;


    public bool singleFire = false;
    public float fireRate = 0.1f;
    public float firerate_automatic = 0.1f;
    public float timeToReload = 1.5f;
    public float weaponDamage = 15;
    float nextFireTime = 0;
    public bool canFire = true;
    private int bulletsToRefill = 0;

    public int bulletsPerMagazine = 30;
    public int bulletsPerMagazineDefault;
    public int AmmoToBack; // отслеживаем количество патронов,которое уйдёт обратно при перезарядке если в обойме остались патроны
    public int MaxAmmoInGun; // впринципе максимальное количество патронов в пушке(потолок патронов)
    public int MaxAmmo; // отслеживаем максимальное количество патронов для перезарядки


    bool isFiring = false; // отселижваем зажата ли клавиша стрельбы и продолжаем стрельбу если да



    [HideInInspector]
    public Kabach_WeaponManager manager;



    void Start()
    {
        bulletsPerMagazineDefault = bulletsPerMagazine;
        bulletsPerMagazine = bulletsPerMagazine;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = true;
        //делаем звук 3d
        audioSource.spatialBlend = 1f;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && singleFire)
        {
            if (bulletsPerMagazine > 0 && MaxAmmo >= 0)
                single_Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && MaxAmmo > 0 || bulletsPerMagazine == 0)
        {
            StartCoroutine(Reload());
        }
        if (Input.GetMouseButtonDown(0) && bulletsPerMagazine == 0)
        {
            audioSource.clip = reloadAudio;
            audioSource.Play();
            StartCoroutine(Reload());
            
        }


        if (Input.GetKeyDown(KeyCode.B) && !singleFire ) {
            singleFire = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && singleFire && manager.selectedWeapon)
        {
            singleFire = false;
        }

        if (manager.selectedWeapon == manager.secondaryWeapon)
        {
            singleFire = true;
        }


        ammo_box = GameObject.Find("Ammo_box");


        // Проверяем, нажата ли левая кнопка мыши
        if (Input.GetButton("Fire1") && !singleFire && bulletsPerMagazine > 0 && MaxAmmo >= 0)
        {
            if (!isFiring) // Если мы еще не стреляем
            {
                // Запускаем корутину для автоматической стрельбы
                //StartCoroutine(automatic_fire());
                StartCoroutine(automatic_fire());

            }
        }
        else
        {
            // Если кнопка мыши не нажата, прекращаем стрельбу
            isFiring = false;
        }

        


    }

    void single_Fire()
    {
        if (canFire)
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + fireRate;

                if (bulletsPerMagazine > 0)
                {
                    //ставим указатель на центр камеры
                    Vector3 firePointPointerPosition = manager.playerCamera.transform.position + manager.playerCamera.transform.forward * 100;
                    RaycastHit hit;
                    if (Physics.Raycast(manager.playerCamera.transform.position, manager.playerCamera.transform.forward, out hit, 100))
                    {
                        firePointPointerPosition = hit.point;
                    }

                    firePoint.LookAt(firePointPointerPosition);
                    //стрельба
                    GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Kabach_Bullet bullet = bulletObject.GetComponent<Kabach_Bullet>();
                    // устанавливаем урон пули исходя из урона оружия
                    bullet.SetDamage(weaponDamage);



                    bulletsPerMagazine--;
                    audioSource.clip = fireAudio;
                    audioSource.Play();
                }
                else
                {
                    StartCoroutine(Reload());

                }
            }
        }
    }

    IEnumerator automatic_fire()
    {
        isFiring = true;
        if (canFire)
        {

            while (Input.GetButton("Fire1"))
            {
                if (bulletsPerMagazine > 0)

                {


                    // Ждем время, соответствующее скорости стрельбы


                    //ставим указатель на центр камеры
                    Vector3 firePointPointerPosition = manager.playerCamera.transform.position + manager.playerCamera.transform.forward * 100;
                    RaycastHit hit;
                    if (Physics.Raycast(manager.playerCamera.transform.position, manager.playerCamera.transform.forward, out hit, 100))
                    {
                        firePointPointerPosition = hit.point;
                    }

                    firePoint.LookAt(firePointPointerPosition);
                    //стрельба
                    GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Kabach_Bullet bullet = bulletObject.GetComponent<Kabach_Bullet>();
                    // устанавливаем урон пули исходя из урона оружия
                    bullet.SetDamage(weaponDamage);


                    bulletsPerMagazine--;
                    audioSource.clip = fireAudio;
                    audioSource.Play();


                }
                else
                {
                    StartCoroutine(Reload());

                }


                yield return new WaitForSeconds(1f / firerate_automatic);

            }

            isFiring = false; // Устанавливаем состояние стрельбы в false после прекращения стрельбы

        }
    }



    IEnumerator Reload()
    {
        canFire = false;

        audioSource.clip = reloadAudio;
        audioSource.Play();

        yield return new WaitForSeconds(timeToReload);

        bulletsToRefill = bulletsPerMagazineDefault - bulletsPerMagazine;

        if (MaxAmmo >= bulletsToRefill)
        {
            bulletsPerMagazine += bulletsToRefill;
            MaxAmmo -= bulletsToRefill;
        }
        else
        {

            bulletsPerMagazine += MaxAmmo;
            MaxAmmo = 0;
        }
        //yield return new WaitForSeconds(timeToReload);
        canFire = true;
    }








    // вызываем из менеджера оружия

    public void ActiveWeapon(bool active)
    {
        StopAllCoroutines();
        canFire = true;
        gameObject.SetActive(active);
    }

    public void GetAmmo(int points)
    {
        MaxAmmo += points;
        
    }
}

