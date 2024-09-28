using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        if (masterVolumeSlider == null || musicVolumeSlider == null || sfxVolumeSlider == null)
        {
            throw new MissingReferenceException("One or more sliders are missing reference. Check slider reference");
        }
    }

    private void Start()
    {
        LoadVolumeSettings();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat(MasterVolumeKey, Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat(MasterVolumeKey, level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat(MusicVolumeKey, Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat(MusicVolumeKey, level);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat(SFXVolumeKey, Mathf.Log10(level) * 20f);
        PlayerPrefs.SetFloat(SFXVolumeKey, level);
    }

    private void LoadVolumeSettings()
    {
        float defaultVolume = 0.5f; // Default volume
        float masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, defaultVolume);
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, defaultVolume);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeKey, defaultVolume);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }
}