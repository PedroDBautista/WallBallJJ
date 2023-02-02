using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public int levelID = 0;

    public void OpenLevel()
    {
        GameManager.OpenLevel(levelID);
    }

}
