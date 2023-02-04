using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : SignalHandler
{

    public static CameraShake instance;

    private CinemachineVirtualCamera vc;
    private CinemachineBasicMultiChannelPerlin cbmcp;
    private float shakeTimeCount = 0f;

    private void Awake(){
        vc = GetComponent<CinemachineVirtualCamera>();
        instance = this;
        cbmcp = instance.vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        cbmcp.m_AmplitudeGain = intensity;
        shakeTimeCount = time;
    }

    private void Update()
    {
        if(shakeTimeCount > 0f) shakeTimeCount -= Time.deltaTime;

        if(cbmcp.m_AmplitudeGain > 0f && shakeTimeCount <= 0f)
        {
            cbmcp.m_AmplitudeGain = 0f;
        }
    }

    public override void ReceiveSignal(string signal)
    {
        switch(signal)
        {
            case "PlayerKicked":
                ShakeCamera(10f,0.1f);
            break;
        }
    }

}
