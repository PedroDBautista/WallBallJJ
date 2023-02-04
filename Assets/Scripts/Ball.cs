using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : SignalHandler
{
    void OnCollisionEnter2D(Collision2D c)
    {
        SendSignal("BallCollision");
    }
}
