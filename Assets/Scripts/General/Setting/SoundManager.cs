using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public GameObject SFXPrefabs;
    public AudioMixerGroup SFXGroup;
    public AudioMixerGroup UIGroup;
    public AudioMixerGroup BGMGroup;
    public static SoundManager Instance;


    [System.Serializable]
    public struct SFXClip
    {
        public string name;     // 효과음 이름
        public AudioClip clip;   // 해당 오디오 클립
    }

    public enum SoundOutput
    {
        SFX,
        BGM,
        UI
    }

    [Header("효과음 목록")]
    public SFXClip[] sfxClips;
    private Dictionary<string, AudioClip> sfxDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // 클립 딕셔너리 초기화
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (SFXClip sfx in sfxClips)
        {
            if (!sfxDictionary.ContainsKey(sfx.name))
            {
                sfxDictionary.Add(sfx.name, sfx.clip);
            }
        }
    }


    void Update()
    {
      
    }



    public void PlaySFX(string name, SoundOutput soundOutput , float volum = 1)
    {
        if (sfxDictionary.TryGetValue(name, out AudioClip clip) && clip != null)
        {
            GameObject soundObj = Instantiate(SFXPrefabs); 

            AudioSource source = soundObj.GetComponent<AudioSource>();
            source.clip = clip;

            switch (soundOutput)
            {
                case SoundOutput.SFX:
                    source.outputAudioMixerGroup = SFXGroup;
                    break;
                case SoundOutput.BGM:
                    source.outputAudioMixerGroup = BGMGroup;
                    break;
                case SoundOutput.UI:
                    source.outputAudioMixerGroup = UIGroup;
                    break;

            }

            source.Play();
            source.volume = volum;
            Destroy(soundObj, clip.length);

        }
        else
        {
            Debug.LogWarning($"[SFXManager] 지정된 SFXType '{name}'에 대한 클립이 없습니다.");
        }
    }

}