using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderSync : MonoBehaviour
{
    [Header("Referințe Slidere UI")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {

        if (AudioManager.Instance != null)
        {

            masterSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.RemoveAllListeners();


            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);


            masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
            musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        }
    }
}