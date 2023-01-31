using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float maxVelocity = 5f;
    public float acceleration = 5f;
    public float movementDuration = 4f;

    private float velocity = 0f;
    private float durationCount = 0f;
    private bool counting = false;
    private int dir = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(counting)
        {
            durationCount += Time.deltaTime;
            if(durationCount >= movementDuration)
            {
                durationCount = 0f;
                counting = false;
                dir *= -1;
            }
        }
        if(!counting){
            velocity += acceleration*Time.deltaTime*dir;
            if(Mathf.Abs(velocity) > Mathf.Abs(maxVelocity))
            {
                counting = true;
                velocity = maxVelocity * dir;
            }
        }
        transform.Translate(velocity*Time.deltaTime,0f,0f);
        
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Floor") return;
        c.gameObject.transform.parent = transform;
    }
    void OnTriggerExit2D(Collider2D c)
    {
        if(c.gameObject.tag == "Floor") return;
        c.gameObject.transform.parent = null;
    }
}
