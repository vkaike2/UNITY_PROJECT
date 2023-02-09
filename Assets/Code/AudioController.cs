using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [Header("configuration")]
    [SerializeField]
    private PitchVariation _pitchVariation;
    [Space]
    [SerializeField]
    private List<AudioModel> _audioModels;

    private AudioSource _audioSource;

    private void OnValidate()
    {
        if (_audioModels == null) return;
        if (_audioModels.Count == 0) return;


        foreach (var model in _audioModels)
        {
            if (model == null) return;
            if (model.Clip == null) return;

            model.name = model.Clip.name;
        }

    }

    public void PlayClip(ClipName clipName, float pitch = 1)
    {
        AudioModel model = _audioModels.FirstOrDefault(e => e.ClipName == clipName);
        if (model == null) return;
        _audioSource.pitch = pitch;
        _audioSource.clip = model.Clip;
        _audioSource.Play();
    }

    public void PlayWithPitchVariation(ClipName clipName)
    {
        float pitch = UnityEngine.Random.Range(_pitchVariation.From, _pitchVariation.To);
        PlayClip(clipName, pitch);
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public enum ClipName
    {
        Player_Step,
        Player_Jump,
        Player_Fart,
        Player_Poop
    }

    [Serializable]
    public class AudioModel
    {
        [HideInInspector]
        public string name;

        [SerializeField]
        private ClipName _clipName;
        [SerializeField]
        private AudioClip _clip;

        public ClipName ClipName => _clipName;
        public AudioClip Clip => _clip;
    }

    [Serializable]
    public class PitchVariation
    {
        [SerializeField]
        private float _from = 1;
        [SerializeField]
        private float _to = 2;

        public float From => _from;
        public float To => _to;
    }
}
