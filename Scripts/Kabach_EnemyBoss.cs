
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]

public class Kabach_NPCEnemy_Boss : MonoBehaviour, IEntity
{
    public float attackDistance_boss = 3f;
    public float movementspeed_boss = 5f;
    public float npcHp_boss = 100;
    public float npcDamage_boss = 5;
    public float attackRate_boss = 0.5f;
    public Transform firePoint_boss;
    public Transform firepoint_head_boss;
    public GameObject npcDeadPrefab_boss;
    public Kabach_DamageReceiver damageReceiver_boss;


    [HideInInspector]
    public Transform playerTransform_boss_script;
    [HideInInspector]
    public Kabach_EnemySpawner es_boss;
    NavMeshAgent agent_boss;
    float nextAttackTime_boss = 0;


    void Start()
    {

        agent_boss = GetComponent<NavMeshAgent>();
        agent_boss.stoppingDistance = attackDistance_boss;
        agent_boss.speed = movementspeed_boss;
        playerTransform_boss_script = GameObject.Find("Player").GetComponent<Transform>();

        //устанавливаем ригидбоду в кимнематик чтобы избежать бага хит
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }


    void Update()
    {
        if (agent_boss.remainingDistance - attackDistance_boss < 0.01f)
        {
            if (Time.time > nextAttackTime_boss)
            {
                nextAttackTime_boss = Time.time + attackRate_boss;

                //Атака
                RaycastHit hit;
                if (Physics.Raycast(firePoint_boss.position, firePoint_boss.forward, out hit, attackDistance_boss))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint_boss.position, firePoint_boss.position + firePoint_boss.forward * attackDistance_boss, Color.cyan);

                        IEntity player = hit.transform.GetComponent<IEntity>();
                        player.ApplyDamage(npcDamage_boss);
                    }
                }
            }
        }
        // Двигаем к player
        agent_boss.destination = playerTransform_boss_script.position;
        //Всегда смотрим на игрока
        transform.LookAt(new Vector3(playerTransform_boss_script.transform.position.x, transform.position.y, playerTransform_boss_script.position.z));

    }

    public void ApplyDamage(float points)
    {
        npcHp_boss -= points;
        if (npcHp_boss <= 0)
        {
            //Уничтожаем нпс
            GameObject npcDead = Instantiate(npcDeadPrefab_boss, transform.position, transform.rotation);
            // чуть-чуть подбрасываем мёртвого нпс
            npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform_boss_script.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            Destroy(npcDead, 10);
            es_boss.EnemyEliminated(this);
            Destroy(gameObject);

        }
    }
}
