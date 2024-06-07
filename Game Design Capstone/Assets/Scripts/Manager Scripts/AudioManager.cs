using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioClip startDrivingClip, stopDrivingClip, screeClip, thankYouClip;

    private AudioSource[] _audioSources;


    private void Awake() {
        if (instance == null)
            instance = this;

        _audioSources = gameObject.GetComponents<AudioSource>();
    }

    public void PlayStartDrivingSound() {
        _audioSources[0].clip = startDrivingClip;
        // _audioSources[0].volume = 0.1f;
        _audioSources[0].Play();
    }

    public void PlayStopDrivingSound() {
        _audioSources[0].clip = stopDrivingClip;
        // _audioSources[0].volume = 0.1f;
        _audioSources[0].Play();
    }

    public void PlayScreeSound() {
        _audioSources[0].clip = screeClip;
        // _audioSources[0].volume = 0.1f;
        _audioSources[0].Play();
    }

    public void PlayThankYouSound() {
        _audioSources[0].clip = thankYouClip;
        // _audioSources[0].volume = 0.1f;
        // _audioSources[1].loop = true;
        _audioSources[0].Play();
    }

}
