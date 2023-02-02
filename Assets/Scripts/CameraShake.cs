using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
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

    public static void ShakeCamera(float intensity, float time)
    {
        instance.cbmcp.m_AmplitudeGain = intensity;
        instance.shakeTimeCount = time;
    }

    private void Update()
    {
        if(shakeTimeCount > 0f) shakeTimeCount -= Time.deltaTime;

        if(cbmcp.m_AmplitudeGain > 0f && shakeTimeCount <= 0f)
        {
            cbmcp.m_AmplitudeGain = 0f;
        }
    }

}
