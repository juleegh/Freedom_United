using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

public class GameAudio : MonoBehaviour, NotificationsListener
{
    private static GameAudio instance;
    public static GameAudio Instance { get { return instance; } }

    [Serializable]
    public class SoundsDictionary : SerializableDictionaryBase<AudioEvent, AudioClip> { }

    [SerializeField] private SoundsDictionary sounds;
    [SerializeField] private AudioSource audioSource;

    public void ConfigureComponent()
    {
        instance = this;
    }

    public void AudioToEvent(AudioEvent audioEvent)
    {
        if (!sounds.ContainsKey(audioEvent))
            return;

        audioSource.PlayOneShot(sounds[audioEvent]);
    }
}
