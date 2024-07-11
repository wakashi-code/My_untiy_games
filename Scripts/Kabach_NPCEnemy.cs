

using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class Kabach_NPCEnemy : MonoBehaviour, IEntity
{
    public float attackDistance = 3f;
    public float movementspeed = 5f;
    public float npcHp = 100;
    public float npcDamage = 5;
    public float attackRate = 0.5f;
    public Transform firePoint;
    public Transform firepoint_head;
    public GameObject npcDeadPrefab;
    public Kabach_DamageReceiver damageReceiver;


    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public Kabach_EnemySpawner es;
    NavMeshAgent agent;
    float nextAttackTime = 0;


    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementspeed;
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();

        //устанавливаем ригидбоду в кимнематик чтобы избежать бага хит
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }


    void Update()
    {
        if (agent.remainingDistance - attackDistance < 0.01f)
        {
            if(Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;

                //Атака
                RaycastHit hit;
                if(Physics.Raycast(firePoint.position,firePoint.forward, out hit, attackDistance)) {
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

                            IEntity player = hit.transform.GetComponent<IEntity>();
                        player.ApplyDamage(npcDamage);
                    }
                }
            }
        }
        // Двигаем к player
        agent.destination = playerTransform.position;
        //Всегда смотрим на игрока
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));

    }

    public void ApplyDamage(float points)
    {
        npcHp -= points;
        if(npcHp <= 0){
            //Уничтожаем нпс
            GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);
            // чуть-чуть подбрасываем мёртвого нпс
            npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            Destroy(npcDead, 10);
            es.EnemyEliminated(this);
            Destroy(gameObject);

        }
    }
}
