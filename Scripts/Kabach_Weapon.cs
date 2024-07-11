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
    public int AmmoToBack; // ����������� ���������� ��������,������� ���� ������� ��� ����������� ���� � ������ �������� �������
    public int MaxAmmoInGun; // ��������� ������������ ���������� �������� � �����(������� ��������)
    public int MaxAmmo; // ����������� ������������ ���������� �������� ��� �����������


    bool isFiring = false; // ����������� ������ �� ������� �������� � ���������� �������� ���� ��



    [HideInInspector]
    public Kabach_WeaponManager manager;



    void Start()
    {
        bulletsPerMagazineDefault = bulletsPerMagazine;
        bulletsPerMagazine = bulletsPerMagazine;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = true;
        //������ ���� 3d
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


        // ���������, ������ �� ����� ������ ����
        if (Input.GetButton("Fire1") && !singleFire && bulletsPerMagazine > 0 && MaxAmmo >= 0)
        {
            if (!isFiring) // ���� �� ��� �� ��������
            {
                // ��������� �������� ��� �������������� ��������
                //StartCoroutine(automatic_fire());
                StartCoroutine(automatic_fire());

            }
        }
        else
        {
            // ���� ������ ���� �� ������, ���������� ��������
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
                    //������ ��������� �� ����� ������
                    Vector3 firePointPointerPosition = manager.playerCamera.transform.position + manager.playerCamera.transform.forward * 100;
                    RaycastHit hit;
                    if (Physics.Raycast(manager.playerCamera.transform.position, manager.playerCamera.transform.forward, out hit, 100))
                    {
                        firePointPointerPosition = hit.point;
                    }

                    firePoint.LookAt(firePointPointerPosition);
                    //��������
                    GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Kabach_Bullet bullet = bulletObject.GetComponent<Kabach_Bullet>();
                    // ������������� ���� ���� ������ �� ����� ������
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


                    // ���� �����, ��������������� �������� ��������


                    //������ ��������� �� ����� ������
                    Vector3 firePointPointerPosition = manager.playerCamera.transform.position + manager.playerCamera.transform.forward * 100;
                    RaycastHit hit;
                    if (Physics.Raycast(manager.playerCamera.transform.position, manager.playerCamera.transform.forward, out hit, 100))
                    {
                        firePointPointerPosition = hit.point;
                    }

                    firePoint.LookAt(firePointPointerPosition);
                    //��������
                    GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Kabach_Bullet bullet = bulletObject.GetComponent<Kabach_Bullet>();
                    // ������������� ���� ���� ������ �� ����� ������
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

            isFiring = false; // ������������� ��������� �������� � false ����� ����������� ��������

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








    // �������� �� ��������� ������

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

