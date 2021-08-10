using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionTabBehaviour : MonoBehaviour
{

    [Header("Interactables")]
    public Toggle fullScreen;

    //public Slider TextSpeed;
    public Slider GlobalVolume;
    public Slider MusicVolume;
    public Slider SfxVolume;
    public Slider UiVolume;

    [Header("Text Visualizer")]
    public TextMeshProUGUI text_Fullscreen;
    public TextMeshProUGUI text_GlobalVolume;
    public TextMeshProUGUI text_MusicVolume;
    public TextMeshProUGUI text_SfxVolume;
    public TextMeshProUGUI text_UIVolume;

    private OptionManager om;

    // Use this for initialization
    void Start()
    {
        if (GameManager.instance != null)
        {
            om = GameManager.instance.managers.optionManager;
            om.GetAllOptions();
            SetSliders();
            SetToggle();
        }

    }

    // Update is called once per frame
    void Update()
    {
        DisplayText();

        SendSliderValue();
        SendToggleValue();
        fullScreen.onValueChanged.AddListener(delegate
        {
            SetFullScreen();
        });

        DisplayText();
    }

    void DisplayText()
    {
        text_Fullscreen.text = fullScreen.isOn ? "On" : "Off";
        text_GlobalVolume.text = ((int)GetNewRatioWithValues(GlobalVolume)).ToString() + "%";
        text_MusicVolume.text = ((int)GetNewRatioWithValues(MusicVolume)).ToString() + "%";
        text_SfxVolume.text = ((int)GetNewRatioWithValues(SfxVolume)).ToString() + "%";
        text_UIVolume.text = ((int)GetNewRatioWithValues(UiVolume)).ToString() + "%";
    }

    float IntervalBetweenMinAndMax(float min, float max)
    {
        // sqrt (min - max)² = min² - 2min*max + max²
        return Mathf.Sqrt(min * min - 2 * min * max + max * max);
    }

    float ConvertValueWithMinToZero(Slider slider, float value)
    {
        return IntervalBetweenMinAndMax(slider.minValue, slider.maxValue) + value;
    }

    float GetNewRatioWithValues(Slider slider)
    {
        float min = ConvertValueWithMinToZero(slider, slider.minValue);
        float max = ConvertValueWithMinToZero(slider, slider.maxValue);
        float value = ConvertValueWithMinToZero(slider, slider.value);

        return value * 100 / max;
    }

    void SetFullScreen()
    {
        Screen.fullScreen = fullScreen.isOn;
    }


    void SendToggleValue()
    {
        om.fullScreen = fullScreen.isOn;
    }

    void SetToggle()
    {
        fullScreen.isOn = om.fullScreen;
    }

    public void SetSliders()
    {
        GlobalVolume.value = om.mixerVolumes.masterVolume;
        MusicVolume.value = om.mixerVolumes.musicVolume;
        SfxVolume.value = om.mixerVolumes.fxVolume;
        UiVolume.value = om.mixerVolumes.uxVolume;
    }

    public void SendSliderValue()
    {
        om.mixerVolumes.masterVolume = GlobalVolume.value;
        om.mixerVolumes.musicVolume = MusicVolume.value;
        om.mixerVolumes.fxVolume = SfxVolume.value;
        om.mixerVolumes.uxVolume = UiVolume.value;
    }

    public void Save()
    {
        SendSliderValue();
        om.SaveAllOptionsToPersistent();
    }

    public void OnDisable()
    {
        Save();
    }

    public void RestoreDefault()
    {
        Debug.Log("Restore");
        GameManager.instance.managers.optionManager.mixerVolumes = new MixerVolumes();
        fullScreen.isOn = false;
        GameManager.instance.managers.optionManager.fullScreen = false;
        SetSliders();
        SendSliderValue();
        Save();
    }
}
