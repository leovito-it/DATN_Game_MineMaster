using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SFX
{
    public static class SFX_Extensions
    {
        /// <summary> Play <b>clip</b> once</summary>
        public static void Play(this AudioClip clip)
        {
            if (SFX_Manager.Instance != null && clip != null)
                SFX_Manager.Instance.PlaySE(clip, false);
        }

        public static void Play(this AudioClip clip, float duration)
        {
            SFX_Manager.Instance.StartCoroutine(PlayEF(clip, duration));
        }

        public static void PlayDelayed(this AudioClip clip, float delay)
        {
            SFX_Manager.Instance.PlaySE(clip, false, delay);
        }

        public static void PlayInterval(this AudioClip clip, float from, float to)
        {
            if (SFX_Manager.Instance != null && clip != null)
                SFX_Manager.Instance.PlaySEInterval(clip, from, to);
        }

        public static void PlayAsBackground(this AudioClip clip)
        {
            SFX_Manager.Instance.PlayBG(clip);
        }

        public static void PlayEF(this GameObject obj, AudioClip clip, float volumnScale = 1f, bool loop = false)
        {
            if (clip == null)
                return;

            if (!obj.TryGetComponent(out AudioSource source))
            {
                source = obj.AddComponent<AudioSource>();
            }
            source.mute = SFX_Manager.Instance.SE_AudioSource.mute;
            source.volume = SFX_Manager.Instance.SEvol;
            source.loop = loop;

            if (source.mute || clip == source.clip)
                return;

            source.PlayOneShot(clip, volumnScale);
        }

        /// <summary> Play <b>clip</b> using AudioManager script</summary>
        static IEnumerator PlayEF(AudioClip clip, float duration)
        {
            if (SFX_Manager.Instance != null && clip != null)
            {
                float timer = 0f;
                while (timer < duration)
                {
                    timer += clip.length;
                    clip.Play();

                    yield return new WaitForSeconds(clip.length);
                }
            }
        }

        /// <summary> Stop <b>clip</b> on this object</summary>
        public static void StopEF(this GameObject obj)
        {
            if (!obj.TryGetComponent(out AudioSource source))
            {
                return;
            }

            source.clip = null;
        }
    }

    public class SFX_Manager : Singleton<SFX_Manager>
    {
        const string BG_VOLUMN = "BGvolumn";
        const string BG_MUTE = "BGmute";

        const string SE_VOLUMN = "SEvolumn";
        const string SE_MUTE = "SEmute";

        public AudioSource BG_AudioSource { get; private set; }
        public AudioSource SE_AudioSource { get; private set; }

        public float BGvol
        {
            get { return PlayerPrefs.GetFloat(BG_VOLUMN, 1); }
            set
            {
                DEFINE.SaveKey(BG_VOLUMN, Mathf.Clamp01(value));

                if (BG_AudioSource != null)
                {
                    BG_AudioSource.volume = value;
                    BGmute = value == 0;
                }
            }
        }
        public float SEvol
        {
            get { return PlayerPrefs.GetFloat(SE_VOLUMN, 1); }
            set
            {
                DEFINE.SaveKey(SE_VOLUMN, Mathf.Clamp01(value));
                if (SE_AudioSource != null)
                {
                    SE_AudioSource.volume = value;
                    SE_AudioSource.mute = value == 0;
                }
            }
        }

        public bool BGmute
        {
            get { return PlayerPrefs.GetInt(BG_MUTE, 0) == 1; }
            set
            {
                DEFINE.SaveKey(BG_MUTE, value ? 1 : 0);

                if (BG_AudioSource != null)
                {
                    BG_AudioSource.mute = value;
                }
            }
        }

        public bool SEmute
        {
            get { return PlayerPrefs.GetInt(SE_MUTE, 0) == 1; }
            set
            {
                DEFINE.SaveKey(SE_MUTE, value ? 1 : 0);

                if (SE_AudioSource != null)
                {
                    SE_AudioSource.mute = value;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            if (BG_AudioSource == null || SE_AudioSource == null)
                Create_BG_SE_GameObject();

            InitBG_audio();
            InitSE_audio();
        }

        private void Create_BG_SE_GameObject()
        {
            GameObject goBG = new("BG audioSource");
            GameObject goSE = new("Sound Effect audioSource");

            BG_AudioSource = goBG.AddComponent<AudioSource>();
            SE_AudioSource = goSE.AddComponent<AudioSource>();

            goBG.transform.SetParent(transform);
            goSE.transform.SetParent(transform);
        }

        private void InitBG_audio()
        {
            SetVolumnBG(BGvol);
            BG_AudioSource.playOnAwake = true;
            BG_AudioSource.loop = true;
        }

        private void InitSE_audio()
        {
            SetVolumnSE(SEvol);
            SE_AudioSource.playOnAwake = false;
            SE_AudioSource.loop = false;
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

        public void SetVolumnSE(float value) => SEvol = value;
        public void SetVolumnBG(float value) => BGvol = value;


        public void PlaySE(AudioClip _clip, bool _loop = false, float delay = 0)
        {
            if (_clip == null || SE_AudioSource.mute)
                return;

            SE_AudioSource.loop = _loop;
            SE_AudioSource.clip = _clip;

            if (delay == 0)
                SE_AudioSource.Play();
            else
                SE_AudioSource.PlayDelayed(delay);
        }
        public void PlaySEOneShot(AudioClip clip)
        {
            if (clip == null || SE_AudioSource.mute)
                return;

            SE_AudioSource.loop = false;
            SE_AudioSource.PlayOneShot(clip);
        }
        public void PlaySEInterval(AudioClip clip, float fromSeconds, float toSeconds)
        {
            if (clip == null || SE_AudioSource.mute)
                return;

            SE_AudioSource.clip = clip;
            SE_AudioSource.time = fromSeconds;
            SE_AudioSource.Play();
            SE_AudioSource.SetScheduledEndTime(AudioSettings.dspTime + (toSeconds - fromSeconds));
        }
    }
}