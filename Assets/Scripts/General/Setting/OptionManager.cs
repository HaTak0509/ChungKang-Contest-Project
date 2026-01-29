using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // New Input System 사용 시

public class OptionManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("슬라이더")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider uiSlider;
    public Slider sfxSlider;



    public TMP_Dropdown dropdown;
    //public Toggle Toggle;
    public int resolutionNum;
    List<Resolution> resolutions = new List<Resolution>();
    FullScreenMode screenMode;


    void Start()
    {

        // 1. 저장된 설정 불러오기 (값이 없으면 기본값 0)
        float master = PlayerPrefs.GetFloat("Master", 0f);
        float ui = PlayerPrefs.GetFloat("UI", 0f);
        float bgm = PlayerPrefs.GetFloat("BGM", 0f);
        float sfx = PlayerPrefs.GetFloat("SFX", 0f);

        // 2. 슬라이더 위치 초기화
        masterSlider.value = master;
        uiSlider.value = ui;
        bgmSlider.value = bgm;
        sfxSlider.value = sfx;

        // 3. 실제 오디오 믹서에 적용
        SetMasterVolume(master);
        SetBGMVolume(ui);
        SetSFXVolume(bgm);
        SetSFXVolume(sfx);

        foreach (Resolution res in Screen.resolutions)
        {
            if (res.refreshRateRatio.numerator == 60)
                resolutions.Add(res);
        }
        dropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = item.width + "x" + item.height + " " + item.refreshRateRatio + "hz";
            dropdown.options.Add(optionData);

            if (item.width == Screen.width && item.height == Screen.height)
                dropdown.value = optionNum;
            optionNum++;
        }
        dropdown.RefreshShownValue();

        int temp_fullscreen = PlayerPrefs.GetInt("FullScreen", 0);

        //Toggle.isOn = temp_fullscreen == 1 ? true : false;

    }

    // --- 비디오 설정 함수들 ---
    public void DropBoxOptionChange(int index)
    {
        resolutionNum = index;
        OkClick();
    }

    //public void FullScreen(bool isFull)
    //{
    //    screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    //    PlayerPrefs.SetInt("FullScreen", isFull ? 1 : 0);
    //    PlayerPrefs.Save();

    //    OkClick();
    //}

    public void OkClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }


    // --- 오디오 설정 함수들 ---

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", volume);
        PlayerPrefs.SetFloat("Master", volume);
    }
    public void SetUIVolume(float volume)
    {
        audioMixer.SetFloat("UI", volume);
        PlayerPrefs.SetFloat("UI", volume);
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", volume);
        PlayerPrefs.SetFloat("BGM", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFX", volume);
    }
}