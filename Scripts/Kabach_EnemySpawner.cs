using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Experimental.GraphView;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kabach_EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Kabach_DamageReceiver player;
    public Texture crosshairTexture;
    public float spawnInterval = 4;// спавним противников каждые 4 сеукнды
    public int enemiesPerWave = 5; // как много противников в одной волне
    public Transform[] spawnPoints;

    float nextSpawnTime = 0;
    int waveNumber = 1;
    bool waitingForWave = true;
    float newWaveTimer = 0;
    int enemiesToEliminate;
    // как много врагов мы уже убили на этой волне
    int enemiesEliminated = 0;
    int totalEnemiesSpawned = 0;
    bool newWaveStart = false;

    // босс
    bool boss_wave = false; // флаг которые будет определять запущена ли волна с боссом
    public GameObject boss_prefab;



    void Start()
    {
        //лочим курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Ждём 10 секунд до начала нвоой волны
        newWaveTimer = 10;
        waitingForWave = true;
    }

    void Update()
    {
        
        if (waveNumber % 5 == 0 && waveNumber !=0)
        {
            boss_wave = true;
        }
        if(waitingForWave && !boss_wave)
        {
            if(newWaveTimer >= 0)
            {
                newWaveTimer -= Time.deltaTime;

            }
            else
            {
                //ининциализируем нвоую волну
                enemiesToEliminate = waveNumber * enemiesPerWave;
                enemiesEliminated = 0;
                totalEnemiesSpawned = 0;
                waitingForWave=false;
            }
        }
        else if (boss_wave && waitingForWave)
        {
            if (newWaveTimer >= 0)
            {
                newWaveTimer -= Time.deltaTime;
            }
            else
            {
                //Волна босса
                enemiesToEliminate = 1;
                enemiesEliminated = 0;
                totalEnemiesSpawned = 0;
                waitingForWave = false;
            }
            if(Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnInterval;
                newWaveStart = true;
                
                // спавн босса
                if(totalEnemiesSpawned < enemiesToEliminate)
                {
                    Transform randompoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                    GameObject boss = Instantiate(boss_prefab, randompoint.position, Quaternion.identity);
                    Kabach_NPCEnemy npc_ = boss.GetComponent<Kabach_NPCEnemy>();
                    npc_.playerTransform = player.transform;
                    npc_.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }
        else
        {
            if(Time.time > nextSpawnTime && !boss_wave)
            {
                nextSpawnTime = Time.time + spawnInterval;
                newWaveStart = true;

                //спавним врагов
                if(totalEnemiesSpawned < enemiesToEliminate)
                {
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length-1)];

                    GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
                    Kabach_NPCEnemy npc = enemy.GetComponent<Kabach_NPCEnemy>();
                    npc.playerTransform = player.transform;
                    npc.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }

        if(player.playerHP <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    void OnGUI()
    {
        
        GUI.Box(new Rect(Screen.width / 2 - 35, Screen.height - 35, 70, 25),
            player.weaponManager.selectedWeapon.bulletsPerMagazine.ToString()+ "/" + player.weaponManager.selectedWeapon.MaxAmmo.ToString());

        if(player.playerHP <= 0)
        {
            GUI.Box(new Rect(Screen.width / 2 - 85, Screen.height / 2 - 20, 200,80),
                "Игра окончена\n(Нажмите 'Пробел'\n чтобы начать заново)");
        }
        else
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);
        }
        if(!boss_wave && newWaveStart)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, 0, 200, 25), "Осталось противников: " + (enemiesToEliminate - enemiesEliminated).ToString());
        }
        if(boss_wave)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, 0, 200, 25), "Волна Босса! ");
        }
        

        if (!newWaveStart)
        {
            GUI.Box(new Rect(Screen.width / 2 - 120, Screen.height / 4 - 12, 260, 30),
            "Ожидание волны " + waveNumber.ToString() + " (" + ((int)newWaveTimer).ToString() + "секунд осталось...)");
        }
    }
    
    public void EnemyEliminated(Kabach_NPCEnemy enemy)
    {
        enemiesEliminated++;

        if(enemiesToEliminate - enemiesEliminated <= 0)
        {
            //начинаем новую волну
            newWaveStart = false;
            newWaveTimer = 10;
            waitingForWave = true;
            waveNumber++;
        }
    }

    public void EnemyEliminated(Kabach_NPCEnemy_Boss enemy_Boss)
    {
        enemiesEliminated++;
        if(enemiesEliminated - enemiesEliminated <= 0)
        {
            newWaveStart=false;
            newWaveTimer = 10;
            waitingForWave = true;
            waveNumber++;
        }
    }
}
