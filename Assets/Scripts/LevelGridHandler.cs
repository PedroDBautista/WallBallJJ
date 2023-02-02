using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelGridHandler : MonoBehaviour
{
    public GameObject buttonPrefab;

    void Start()
    {
        string[] levels = GameManager.INSTANCE.levels;
        for(int i = 0; i < levels.Length; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, transform);
            newButton.name = (i+1).ToString();
            newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i+1).ToString();
            newButton.GetComponent<LevelSelector>().levelID = i;
        }
    }


}
