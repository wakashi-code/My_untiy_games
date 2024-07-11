using System.Collections;
using UnityEngine;

public class Kabach_Bullet : MonoBehaviour
{
    public float bulletSpeed = 345;
    public float hitForce = 50f;
    public float destroyAfter = 3.5f;

    float currentTime = 0;
    Vector3 newPos;
    Vector3 oldPos;
    bool hasHit = false;

    float damagePoints;

    IEnumerator Start()
    {
        newPos = transform.position;
        oldPos = newPos;

        while(currentTime < destroyAfter && !hasHit)
        {
            Vector3 velocity = transform.forward * bulletSpeed;
            newPos += velocity * Time.deltaTime;
            Vector3 direction = newPos - oldPos;
            float distance = direction.magnitude;
            RaycastHit hit;

            //проверка попал ли во что-то
            if(Physics.Raycast(oldPos, direction, out hit, distance))
            {
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(direction * hitForce);

                    IEntity npc = hit.transform.GetComponent<IEntity>();
                    if(npc != null)
                    {
                        // применяем дамаг к нпс
                        npc.ApplyDamage(damagePoints);
                    }
                }
                newPos = hit.point; //регулируем новуюб позицию
                StartCoroutine(DestroyBullet());
            }
            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();

            transform.position = newPos;
            oldPos = newPos;
        }
        if(!hasHit)
        {
            StartCoroutine(DestroyBullet());
        }

    }

    IEnumerator DestroyBullet()
    {
        hasHit = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    //здесь устанавливаем как много урона нанесла пуля
    public void SetDamage(float points)
    {
        damagePoints = points;
    }


    void Update()
    {
        
    }
}
