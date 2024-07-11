using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHead_damage : MonoBehaviour
{

    private Kabach_DamageReceiver _kabach;
    private bool canTakeDamage = true;

    private void Start()
    {
        _kabach = GameObject.Find("Player").GetComponent<Kabach_DamageReceiver>();
    }



    IEnumerator ResetCanTakeDamage()
    {
        yield return new WaitForSeconds(0.3f);
        canTakeDamage = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Player"))
        {
            if (canTakeDamage)
            {
                _kabach.ApplyDamage(10);
                _kabach.ShowDamageScreen();
                canTakeDamage = false;
                StartCoroutine(ResetCanTakeDamage());
            }
        }
    }
}
    
    






