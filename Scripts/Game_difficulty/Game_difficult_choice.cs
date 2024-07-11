using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_difficult_choice : MonoBehaviour
{
    public Kabach_NPCEnemy enemy;
    public Kabach_EnemySpawner spawner;



    public void Return_to_menu_button_pressed()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Easy_difficult_button_pressed()
    {
        enemy.movementspeed = 2f;
        enemy.npcHp = 50f;
        spawner.enemiesPerWave = 5;
        spawner.spawnInterval = 5;
        SceneManager.LoadScene("GameScene");
    }

    public void Middile_difficult_button_pressed()
    {
        enemy.movementspeed = 4f;
        enemy.npcHp = 100f;
        spawner.enemiesPerWave = 10;
        spawner.spawnInterval = 2;
        SceneManager.LoadScene("GameScene");
    }

    public void Hard_difficult_button_pressed()
    {
        enemy.movementspeed = 6f;
        enemy.npcHp = 150f;
        spawner.enemiesPerWave = 15;
        spawner.spawnInterval = 2;
        SceneManager.LoadScene("GameScene");
    }

    public void Unreal_difficult_button_pressed()
    {
        enemy.movementspeed = 7f;
        enemy.npcHp = 200f;
        spawner.enemiesPerWave = 30;
        spawner.spawnInterval = 1;
        SceneManager.LoadScene("GameScene");
    }
}
