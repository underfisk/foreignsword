using HelperPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Soon refactor this class
*/
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    public enum SoundGroup
    {
        Music,
        Ambient,
        Dialogue,
        Effects
    }

    public delegate void OnValueChange(float value);
    public event OnValueChange ValueChangeObservers;


    [SerializeField] AudioSource source;
    [SerializeField] public SoundGroup Group;
    [SerializeField] bool playOnAwake = false;

    protected void Awake()
    {
        //Make sure we have instance
        if (source == null)
            source = GetComponent<AudioSource>();

        //Register a handler for the group type
        switch(Group)
        {
            case SoundGroup.Music:
                //Initialize the values with player prefs
                source.volume = LocalPrefs.MusicVolume;
                ValueChangeObservers += MusicVolumeChange;
                break;
            case SoundGroup.Dialogue:
                //Initialize the values with player prefs
                source.volume = LocalPrefs.DialogueVolume;
                ValueChangeObservers += DialogueVolumeChange;
                break;
            case SoundGroup.Ambient:
                //Initialize the values with player prefs
                source.volume = LocalPrefs.AmbientVolume;
                ValueChangeObservers += AmbientVolumeChange;
                break;
            case SoundGroup.Effects:
                //Initialize the values with player prefs
                source.volume = LocalPrefs.EffectsVolume;
                ValueChangeObservers += EffectsVolumeChange;
                break;
        }

        if (playOnAwake)
            Play();
    }

    public void EffectsVolumeChange(float v)
    {
        source.volume = v;
    }

    public void DialogueVolumeChange(float v)
    {
        source.volume = v;
    }

    public void AmbientVolumeChange(float v)
    {
        source.volume = v;
    }

    public void MusicVolumeChange(float v)
    {
        source.volume = v;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (Group == SoundGroup.Ambient)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                Play();
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (Group == SoundGroup.Ambient)
        {
            if (other.gameObject.GetComponent<Player>())
            {
                Stop();
            }
        }
    }

    #region Methods
    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Restart()
    {
        Stop();
        Play();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }
    #endregion
}
