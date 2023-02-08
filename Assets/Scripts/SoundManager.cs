using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SignalHandler
{
    public AudioClip[] kick,jump,bounce,trap,goal;

    private AudioSource src;


    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    public override void ReceiveSignal(string signal)
    {
        switch(signal)
        {
            case "PlayerKicked":
                PlaySoundRandomized(kick);
            break;
            case "PlayerJumped":
                PlaySoundRandomized(jump);
            break;
            case "BallCollision":
                PlaySoundRandomized(bounce);
            break;
            case "PlayerTrapped":
                PlaySoundRandomized(trap);
            break;
            case "Goal":
                PlaySoundRandomized(goal);
            break;
        }
    }

    private void PlaySoundRandomized(AudioClip[] clip)
    {
        src.PlayOneShot(clip[Random.Range(0,clip.Length)]);

    }
}
