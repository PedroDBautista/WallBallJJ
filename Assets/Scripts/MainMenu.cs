using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //public Gameobject gameManager; 

    private void Start()
    {
        //gameManager = GameObject.Find("GameManager");
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Demo");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelSelect()
    {
        //;   
    }

    public void RestartDemo()
    {
        SceneManager.LoadScene("Demo");
    }

}