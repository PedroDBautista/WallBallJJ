using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WinState : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (col.gameObject.tag == "Ball")
        {
            Debug.Log("Finished");
            SceneManager.LoadScene("MainMenu");
        }

        
    }
}
