using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    Resolution[] _rsl;
    List<string> _resolutions;
    public Dropdown dropdown;
    public Toggle toggle;
    public AudioMixer audioMixer;
    public Slider slider;
    private bool _isFullScreen = true;

    public void Start()
    {
        _resolutions = new List<string>();
        _rsl = Screen.resolutions;
        foreach (var i in _rsl)
        {
            _resolutions.Add(i.width + "x" + i.height);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(_resolutions);
        
        slider.value = (PlayerPrefs.GetFloat("volume"));
        toggle.SetIsOnWithoutNotify((PlayerPrefs.GetInt("screenMode") == 1));
        Screen.fullScreen = (PlayerPrefs.GetInt("screenMode") == 1);
        _isFullScreen =  (PlayerPrefs.GetInt("screenMode") == 1);
        dropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("resolution"));
        Screen.SetResolution(_rsl[PlayerPrefs.GetInt("resolution")].width, _rsl[PlayerPrefs.GetInt("resolution")].height, _isFullScreen);
        gameObject.SetActive(false);
    }
    
    public void ScreenMode()
    {
        _isFullScreen = !_isFullScreen;
        Screen.fullScreen = _isFullScreen;
    }

    public void AudioVolume(float sliderValue)
    {
        audioMixer.SetFloat("masterVolume", sliderValue);
    }

    public void Resolution(int r)
    {
        Screen.SetResolution(_rsl[r].width, _rsl[r].height, _isFullScreen);
    }

    public void Back()
    {
        float volume;
        audioMixer.GetFloat("masterVolume", out volume);
        PlayerPrefs.SetFloat("volume",volume);
        PlayerPrefs.SetInt("screenMode",_isFullScreen?1:0);
        PlayerPrefs.SetInt("resolution",dropdown.value);
    }
}
