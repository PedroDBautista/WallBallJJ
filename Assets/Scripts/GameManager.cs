using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;
    
    public string[] levels = {"MovingWallTestScene","Demo"};
    public int currentLevel = 0;
    public int currentUnlockedLevel = 0;

    //Keep track of what level we just played and Unlocked

    void Awake()
    {
        if(GameManager.INSTANCE == null)
        {
            INSTANCE = this;
            DontDestroyOnLoad(gameObject);
        }else if(GameManager.INSTANCE != this){
            Destroy(gameObject);
        }
    }

    public static void NextLevel()
    {
        INSTANCE.currentLevel += 1;
        if(INSTANCE.currentLevel == INSTANCE.currentUnlockedLevel+1)
        {
            GameManager.UnlockNextLevel();
        }
        if(INSTANCE.currentLevel < INSTANCE.levels.Length)
        {
            SceneManager.LoadScene(INSTANCE.levels[INSTANCE.currentLevel]);
        }
    }

    public static void ResetLevel()
    {
        SceneManager.LoadScene(INSTANCE.levels[INSTANCE.currentLevel]);
    }

    public static void UnlockNextLevel()
    {
        INSTANCE.currentUnlockedLevel += 1;
    }

    public static void OpenLevel(int levelID)
    {
        if(levelID >= INSTANCE.levels.Length) return;

        SceneManager.LoadScene(INSTANCE.levels[levelID]);
    }
}
