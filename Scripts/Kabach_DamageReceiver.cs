using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Kabach_DamageReceiver : MonoBehaviour, IEntity, Heal_methods
{
    //Этот скрипт отслеживает HP ИГРОКА
    public float playerHP = 100;
    public float maxPlayerHp = 100;

    public Kabach_CharacterController playerController;
    public Kabach_WeaponManager weaponManager;
    public GameObject aptechka;
    public GameObject aptechka_clone;
    public Medkit_spawn medkit_spawned;

    public Image FillImage;
    

  

    public bool takeDamage = false;
    public GameObject damageScreen;
    public float screenTime = 1f;
    private bool isShowingDamage = false;

    public float enoughHp;

    public void Start()
    {
        aptechka = GameObject.Find("Little_medkit");
        medkit_spawned = FindAnyObjectByType<Medkit_spawn>();
        

    }

    public void ApplyDamage(float points)
    {
        
        playerHP -= points;
        FillImage.fillAmount = playerHP / 100;
        takeDamage = true;
        
        if (playerHP <= 0)
        {
            //умер
            playerController.canMove = false;
            playerHP = 0;
        }
        else if (!isShowingDamage && takeDamage)
        {
            StartCoroutine(ShowDamageScreen());
        }
    }

    public System.Collections.IEnumerator ShowDamageScreen()
    {
        isShowingDamage = true;
        damageScreen.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        damageScreen.SetActive(false);
        isShowingDamage = false;
    }

    public void Heal_player(float points)
    {
        
            if(playerHP >= 90)
            {
                enoughHp = maxPlayerHp - playerHP;
                playerHP += enoughHp;
            

        }
            else
            {
                playerHP += points;
            

        }
        FillImage.fillAmount = playerHP / 100;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Medkit"))
        {
            if (playerHP < maxPlayerHp)
            {
                Heal_player(10);
                Destroy(other.gameObject);
                medkit_spawned.medkit_spawned = false;
            }
        }
    }

    public void Update()
    {
        aptechka_clone = GameObject.FindGameObjectWithTag("Medkit");
        

        screenTime -= Time.deltaTime;
        if (screenTime <= 0)
        {
            takeDamage = false;
            screenTime = 1f;
            
        }

        
    }
}