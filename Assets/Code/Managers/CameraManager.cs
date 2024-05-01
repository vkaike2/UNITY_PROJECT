using System;
using System.Collections;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [field: Header("EVENT")]
    [field: SerializeField]
    public ScriptableCameraEvents CameraEvents { get; private set; }

    [Header("COMPONENTS")]
    [SerializeField]
    private CinemachineVirtualCamera _virutalCamera;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _screenShakeDuration = 0.3f;
    [SerializeField]
    private float _screenShakeIntensity = 5f;


    private void Awake()
    {
        CameraEvents.OnScreenShakeEvent.AddListener(OnScreenShake);
    }

    private void OnScreenShake(GameObject source)
    {
        Debug.Log("test");
        StartCoroutine(ShakeCamera());
    }

    public IEnumerator ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _virutalCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _screenShakeIntensity;

        float cdw = 0;
        while (cdw <= _screenShakeDuration)
        {
            cdw += Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_screenShakeIntensity, 0f, cdw / _screenShakeDuration);
            yield return new WaitForFixedUpdate();
        }
    }
}