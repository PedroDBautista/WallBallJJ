using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "OuterBounds")
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}