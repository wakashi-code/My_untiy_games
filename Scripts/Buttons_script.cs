using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons_script : MonoBehaviour
{
    public void Play_button_pressed()
    {
        SceneManager.LoadScene("Game_difficulty_scene");
    }

    public void Exit_button_pressed()
    {
        Application.Quit();
        Debug.Log("Нажата кнопка выхода из игры");
    }

    public void Control_button_pessed()
    {
        SceneManager.LoadScene("ControlScene");
    }

    public void req_button_pressed()
    {
        SceneManager.LoadScene("ReqScene");
    }

    public void BackToMenu_pressed()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
