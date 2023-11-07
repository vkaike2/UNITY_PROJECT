using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _mixer;

    [Space]
    [Header("configurations")]
    [Range(0.0001f, 1f)]
    [SerializeField]
    private float _musicVolume = 1f;
    [Range(0.0001f, 1f)]
    [SerializeField]
    private float _vfxVolume = 1f;

    private void FixedUpdate()
    {
        SetVFXVolume();
        SetMusicVolume();
    }

    private void SetVFXVolume()
    {
        _mixer.SetFloat("VFXvolume", Mathf.Log10(_vfxVolume) * 20);
    }

    private void SetMusicVolume()
    {
        _mixer.SetFloat("MUSICvolume", Mathf.Log10(_musicVolume) * 20);
    }
}
