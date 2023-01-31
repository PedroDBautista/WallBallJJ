using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoseState : MonoBehaviour
{
    public Rigidbody2D rb;

    //Number of bounces on the ground
    private int bounceLimit = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (col.gameObject.tag == "Floor")
        {
            bounceLimit++;
            if (bounceLimit > 1)
            {
                //If the GameObject has the same tag as specified, output this message in the console
                Debug.Log("Lost");
                SceneManager.LoadScene("GameOver");
            }
            Debug.Log("Bounce Limit:" + bounceLimit);
        }

        if (col.gameObject.tag == "Wall")
        {
                bounceLimit = 0;
                //If the GameObject has the same tag as specified, output this message in the console
                Debug.Log("Resetted Bounce Limit");
            Debug.Log("Bounce Limit:" + bounceLimit);
        }

        if (col.gameObject.tag == "Blockade")
            rb.velocity = new Vector3(0,0,0);

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "OuterBounds")
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
