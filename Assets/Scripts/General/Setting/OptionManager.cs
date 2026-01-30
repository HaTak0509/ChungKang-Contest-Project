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
    public Toggle Toggle;
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

        // 해상도 리스트 채우기
        resolutions.Clear();
        foreach (Resolution res in Screen.resolutions)
        {
            // 60Hz만 넣고 싶다면 이 조건을 유지하되, 만약 하나도 안 잡힐 경우를 대비해야 함
            if (res.refreshRateRatio.numerator == 60)
                resolutions.Add(res);
        }

        // [중요] 만약 60Hz 해상도가 하나도 없다면 모든 해상도를 그냥 다 넣음 (안전장치)
        if (resolutions.Count == 0)
        {
            foreach (Resolution res in Screen.resolutions)
                resolutions.Add(res);
        }

        dropdown.options.Clear();
        int optionNum = 0;
        int currentResIndex = 0;

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = $"{item.width}x{item.height} {item.refreshRateRatio.numerator}hz";
            dropdown.options.Add(optionData);

            // 현재 스크린 해상도와 일치하는 인덱스 찾기
            if (item.width == Screen.width && item.height == Screen.height)
            {
                currentResIndex = optionNum;
            }
            optionNum++;
        }

        // 드롭다운 값 설정 (이때 DropBoxOptionChange가 호출될 수 있음)
        dropdown.value = currentResIndex;
        resolutionNum = currentResIndex; // 현재 인덱스 동기화
        dropdown.RefreshShownValue();

        // 풀스크린 토글 설정
        int temp_fullscreen = PlayerPrefs.GetInt("FullScreen", 1); // 기본값 1(전체화면) 권장
        screenMode = (temp_fullscreen == 1) ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Toggle.isOn = (temp_fullscreen == 1);

    }

    // --- 비디오 설정 함수들 ---
    public void DropBoxOptionChange(int index)
    {
        resolutionNum = index;
        OkClick();
    }

    public void FullScreen(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        PlayerPrefs.SetInt("FullScreen", isFull ? 1 : 0);
        PlayerPrefs.Save();

        OkClick();
    }

    public void OkClick()
    {
        // resolutions가 비어있지 않고, index가 0 이상이면서 개수보다 작을 때만 실행
        if (resolutions != null && resolutionNum >= 0 && resolutionNum < resolutions.Count)
        {
            Screen.SetResolution(resolutions[resolutionNum].width,
                                 resolutions[resolutionNum].height,
                                 screenMode);
        }
        else
        {
            Debug.LogError($"해상도 인덱스 오류! 현재 index: {resolutionNum}, " +
                           $"전체 개수: {(resolutions != null ? resolutions.Count : 0)}");
        }
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