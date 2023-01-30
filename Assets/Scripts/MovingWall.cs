using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public float velocity = 50f;
    public float maxDistance = 100f;
    private float distance = 0f;

    public bool stopAfterMaxDistance = false;


    // Update is called once per frame
    void Update()
    {
        if(velocity == 0f) return;

        transform.Translate(velocity*Time.deltaTime,0f,0f);
        distance += velocity*Time.deltaTime;
        if(stopAfterMaxDistance && distance >= maxDistance)
        {
            velocity = 0f;
            var position = transform.position;
            position.x -= distance-maxDistance;
            transform.position = position;
        }
    }
}
