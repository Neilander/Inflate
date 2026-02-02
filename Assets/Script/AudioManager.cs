using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class ClipInfo
    {
        public string name;
        [Range(0f, 1f)]
        public float volume = 1f;
        public AudioClip clip;
    }

    [Header("SFX Clip List")]
    public List<ClipInfo> sfxClips;

    private AudioSource bgmSource;
    private AudioSource[] sfxSources;
    private Dictionary<string, ClipInfo> sfxDict;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        var sources = GetComponents<AudioSource>();
        if (sources.Length < 4)
        {
            Debug.LogWarning("AudioManager 需要至少 4 个 AudioSource");
            return;
        }

        bgmSource = sources[0];

        sfxSources = new AudioSource[3];
        sfxSources[0] = sources[1];
        sfxSources[1] = sources[2];
        sfxSources[2] = sources[3];

        // 建立查表
        sfxDict = new Dictionary<string, ClipInfo>();
        foreach (var info in sfxClips)
        {
            if (info == null || string.IsNullOrEmpty(info.name) || info.clip == null)
                continue;

            if (!sfxDict.ContainsKey(info.name))
            {
                sfxDict.Add(info.name, info);
            }
        }
    }

    /// <summary>
    /// 播放音效（按名字）
    /// </summary>
    public static void PlaySFX(string name)
    {
        if (Instance == null || string.IsNullOrEmpty(name))
            return;

        if (!Instance.sfxDict.TryGetValue(name, out var info))
            return;

        foreach (var source in Instance.sfxSources)
        {
            if (!source.isPlaying)
            {
                source.PlayOneShot(info.clip, info.volume);
                return;
            }
        }

        // 3 个 SFX 槽都在播：直接丢
    }
}
