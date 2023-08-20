using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private bool LoadConfig = true;

    [SerializeField] private AudioSource BG_AudioSource; //background music
    [SerializeField] private AudioSource SE_AudioSource; //sound effect

    [Range(0f, 1f)]
    public float BGvol, SEvol;

    public void SetSE_audioClip(AudioClip _audioClip) => SE_AudioSource.clip = _audioClip;
    public void SetBG_audioClip(AudioClip _audioClip) => BG_AudioSource.clip = _audioClip;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        if (BG_AudioSource == null || SE_AudioSource == null)
            Create_BG_SE_GameObject();

        InitMaster_audio();
        InitBG_audio();
        InitSE_audio();
    }

    public void RemovePrevAndPlaySE(AudioClip clip)
    {
        if (SE_AudioSource.isPlaying)
            SE_AudioSource.Stop();

        SE_AudioSource.clip = clip;
        SE_AudioSource.Play();
    }

    public void WaitPrevAndPlaySE(AudioClip clip)
    {
        _ = StartCoroutine(CheckToPlay(clip));
    }

    private IEnumerator CheckToPlay(AudioClip clip)
    {
        while (SE_AudioSource.isPlaying)
        {
            yield return null;
        }
        PlaySEOneShot(clip);
    }

    public void PlayBG(AudioClip clip)
    {
        if (clip != null)
        {
            Debug.Log($"clip name {clip.name}");
            BG_AudioSource.loop = true;
            BG_AudioSource.clip = clip;
            BG_AudioSource.Play();
        }
    }

    public void StopBGMusic() => BG_AudioSource.Stop();
    public void StopSE() => SE_AudioSource.Stop();
    public void NullBGMusic() => BG_AudioSource.clip = null;

    public void RemoveMiniGameSoundEffect()
    {
        SetSE_audioClip(null);
    }

    public void MuteSE(bool mute) => SE_AudioSource.mute = mute;
    public void MuteBG(bool mute) => BG_AudioSource.mute = mute;

    public void SetVolumnSE(float value) { SEvol = value; SE_AudioSource.volume = value; PlayerPrefs.SetFloat("SEvolumn", value); }
    public void SetVolumnBG(float value) { BGvol = value; BG_AudioSource.volume = value; PlayerPrefs.SetFloat("BGvolumn", value); }

    private void Create_BG_SE_GameObject()
    {
        var transform = this.transform;
        var goBG = new GameObject("BG audioSource");
        var goSE = new GameObject("Sound Effect audioSource");
        goBG.AddComponent<AudioSource>();
        goSE.AddComponent<AudioSource>();
        BG_AudioSource = goBG.GetComponent<AudioSource>();
        SE_AudioSource = goSE.GetComponent<AudioSource>();
        goBG.transform.SetParent(transform);
        goSE.transform.SetParent(transform);
    }

    private void InitBG_audio()
    {
        if (LoadConfig)
            SetVolumnBG(PlayerPrefs.GetFloat("BGvolumn", BGvol));

        BG_AudioSource.loop = true;
    }

    private void InitSE_audio()
    {
        if (LoadConfig)
            SetVolumnSE(PlayerPrefs.GetFloat("SEvolumn", SEvol));

        SE_AudioSource.playOnAwake = false;
        SE_AudioSource.loop = false;
    }

    private void InitMaster_audio()
    {
        //masterVlm = vlm;
    }

    public void PlaySE(AudioClip _clip, bool _loop)
    {
        SE_AudioSource.loop = _loop;
        SE_AudioSource.clip = _clip;
        SE_AudioSource.Play();
    }

    public void PlaySEOneShot(AudioClip clip)
    {
        if (clip != null)
        {
            SE_AudioSource.loop = false;
            SE_AudioSource.PlayOneShot(clip);
        }
    }

    public void PlaySEInterval(AudioClip clip, float fromSeconds, float toSeconds)
    {
        SE_AudioSource.clip = clip;
        SE_AudioSource.time = fromSeconds;
        SE_AudioSource.Play();
        SE_AudioSource.SetScheduledEndTime(AudioSettings.dspTime + (toSeconds - fromSeconds));
    }
}