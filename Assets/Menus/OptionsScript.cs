using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScript : MonoBehaviour
{
    [Header("Graphics")]
    [SerializeField] private TMP_Dropdown monitorDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SoundsSlider;
    [SerializeField] private Toggle MuteMasterToggle;
    [SerializeField] private Toggle MuteMusicToggle;
    [SerializeField] private Toggle MuteSoundsToggle;

    Resolution[] resolutions;


    public void Start()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SoundsSlider.value = PlayerPrefs.GetFloat("SoundsVolume", 1f);

        if (QualitySettings.vSyncCount >= 1)
        {
            vsyncToggle.isOn = true;
        }
        else
        {
            vsyncToggle.isOn = false;
        }

        fullscreenToggle.isOn = Screen.fullScreen;

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + (int)resolutions[i].refreshRateRatio.value + "Hz";

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        monitorDropdown.ClearOptions();
        for (int i = 0; i < Display.displays.Length; i++)
        {
            List<string> monitors = new List<string>();
            string screenString = "Monitor " + (i + 1);
            monitors.Add(screenString);
            monitorDropdown.AddOptions(monitors);
        }

        monitorDropdown.value = PlayerPrefs.GetInt("UnitySelectMonitor");
    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetMonitor(int monitorNum)
    {
        PlayerPrefs.SetInt("UnitySelectMonitor", monitorNum);
    }


    public void SetVolumeMaster(float volume)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        MasterSlider.value = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void SetVolumeMusic(float volume)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
        MusicSlider.value = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void SetVolumeSounds(float volume)
    {
        audioMixer.SetFloat("SoundsVol", Mathf.Log10(volume) * 20);
        SoundsSlider.value = volume;
        PlayerPrefs.SetFloat("SoundsVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetVsync(bool vsync)
    {
        if (vsync)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }


    public void SetMasterMute(bool masterToggle)
    {
        if (!masterToggle)
        {
            audioMixer.SetFloat("MasterVol", -80);
            MasterSlider.value = -80;
            MasterSlider.interactable = false;
            PlayerPrefs.SetFloat("MasterVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MasterVol", 0);
            MasterSlider.value = 1;
            MasterSlider.interactable = true;
            PlayerPrefs.SetFloat("MasterVolume", 1);
        }
    }
    public void SetMusicMute(bool musicToggle)
    {
        if (!musicToggle)
        {
            audioMixer.SetFloat("MusicVol", -80);
            MusicSlider.value = -80;
            MusicSlider.interactable = false;
            PlayerPrefs.SetFloat("MasterVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("MusicVol", 0);
            MusicSlider.value = 1;
            MusicSlider.interactable = true;
            PlayerPrefs.SetFloat("MasterVolume", 1);
        }
    }
    public void SetSoundsMute(bool soundsToggle)
    {
        if (!soundsToggle)
        {
            audioMixer.SetFloat("SoundsVol", -80);
            SoundsSlider.value = -80;
            SoundsSlider.interactable = false;
            PlayerPrefs.SetFloat("MasterVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SoundsVol", 0);
            SoundsSlider.value = 1;
            SoundsSlider.interactable = true;
            PlayerPrefs.SetFloat("MasterVolume", 1);
        }
    }
}
