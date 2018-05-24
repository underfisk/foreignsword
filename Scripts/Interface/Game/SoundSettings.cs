using HelperPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider AmbientSlider;
    [SerializeField] Slider DialogueSlider;
    [SerializeField] Slider EffectsSlider;

    [SerializeField] Text Music, Ambient, Dialogue, Effects;

    public void Start()
    {
        //Initialize them with the playerprefs values
        MusicSlider.value = LocalPrefs.MusicVolume;
        AmbientSlider.value = LocalPrefs.AmbientVolume;
        DialogueSlider.value = LocalPrefs.DialogueVolume;
        EffectsSlider.value = LocalPrefs.EffectsVolume;

        //Delegates the new function to notify
        MusicSlider.onValueChanged.AddListener(delegate { ValueChangeSave(1); });
        AmbientSlider.onValueChanged.AddListener(delegate { ValueChangeSave(2); });
        DialogueSlider.onValueChanged.AddListener(delegate { ValueChangeSave(3); });
        EffectsSlider.onValueChanged.AddListener(delegate { ValueChangeSave(4); });

        //Initialize the % of sound
        Music.text = $"{Math.Round( ((MusicSlider.value / 1f) * 100) ,2)} %";
        Ambient.text = $"{Math.Round(((AmbientSlider.value / 1f) * 100),2)} %";
        Dialogue.text = $"{Math.Round(((DialogueSlider.value / 1f) * 100),2)} %";
        Effects.text = $"{Math.Round(((EffectsSlider.value / 1f) * 100),2)} %";
    }

    public void ValueChangeSave(int sliderId)
    {
        //SOON: Notify the respective observers so they will update the sound in real time
        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();
        switch (sliderId)
        {
            case 1:
                LocalPrefs.MusicVolume = MusicSlider.value;
                Music.text = $"{Math.Round(((MusicSlider.value / 1f) * 100), 2)} %";
                foreach (GameObject obj in sceneObjects)
                {
                    if (obj.GetComponent<SoundController>() && obj.GetComponent<SoundController>().Group == SoundController.SoundGroup.Music)
                        obj.GetComponent<SoundController>().MusicVolumeChange(MusicSlider.value);
                }
                break;
            case 2:
                LocalPrefs.AmbientVolume = AmbientSlider.value;
                Ambient.text = $"{Math.Round(((AmbientSlider.value / 1f) * 100), 2)} %";
                foreach (GameObject obj in sceneObjects)
                {
                    if (obj.GetComponent<SoundController>() && obj.GetComponent<SoundController>().Group == SoundController.SoundGroup.Ambient)
                        obj.GetComponent<SoundController>().AmbientVolumeChange(AmbientSlider.value);
                }
                break;
            case 3:
                LocalPrefs.DialogueVolume = DialogueSlider.value;
                Dialogue.text = $"{Math.Round(((DialogueSlider.value / 1f) * 100), 2)} %";
                foreach (GameObject obj in sceneObjects)
                {
                    if (obj.GetComponent<SoundController>() && obj.GetComponent<SoundController>().Group == SoundController.SoundGroup.Dialogue)
                        obj.GetComponent<SoundController>().DialogueVolumeChange(DialogueSlider.value);
                }
                break;
            case 4:
                LocalPrefs.EffectsVolume = EffectsSlider.value;
                Effects.text = $"{Math.Round(((EffectsSlider.value / 1f) * 100), 2)} %";
                foreach (GameObject obj in sceneObjects)
                {
                    if (obj.GetComponent<SoundController>() && obj.GetComponent<SoundController>().Group == SoundController.SoundGroup.Effects)
                        obj.GetComponent<SoundController>().EffectsVolumeChange(EffectsSlider.value);
                }
                break;
        }
    }

}
