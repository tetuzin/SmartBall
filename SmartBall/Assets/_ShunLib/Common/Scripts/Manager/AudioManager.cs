using UnityEngine;
using UnityEngine.Audio;
using System.Threading.Tasks;

using ShunLib.Dict;

namespace ShunLib.Manager.Audio
{
    public class AudioManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------

        private const string AUDIO_MIXER_MASTER = "Master";
        private const string AUDIO_MIXER_SE = "SE";
        private const string AUDIO_MIXER_VOICE = "Voice";
        private const string AUDIO_MIXER_BGM = "BGM";
        
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("オーディオソース")] 
        [SerializeField] private AudioSource _audioSourceSE = default;
        [SerializeField] private AudioSource _audioSourceVoice = default;
        [SerializeField] private AudioSource _audioSourceBGM = default;

        [Header("オーディオミキサー")] 
        [SerializeField] private AudioMixer _audioMixer = default;

        [Header("SEリスト")] 
        [SerializeField] private AudioClipTable _seAudioDictionary = default;

        [Header("Voiceリスト")] 
        [SerializeField] private AudioClipTable _voiceAudioDictionary = default;

        [Header("BGMリスト")] 
        [SerializeField] private AudioClipTable _bgmAudioDictionary = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public AudioSource AudioSE
        {
            get { return _audioSourceSE; }
        }
        public AudioSource AudioVoice
        {
            get { return _audioSourceVoice; }
        }
        public AudioSource AudioBGM
        {
            get { return _audioSourceBGM; }
        }
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        
        public async Task Initialize()
        {
            await InitSELoad();
            await InitBGMLoad();
        }

        // SEリストを設定
        public void SetSeAudioDictionary(AudioClipTable audioCliptable)
        {
            _seAudioDictionary = audioCliptable;
        }

        // Voiceリストを設定
        public void SetVoiceAudioDictionary(AudioClipTable audioCliptable)
        {
            _voiceAudioDictionary = audioCliptable;
        }

        // BGMリストを設定
        public void SetBgmAudioDictionary(AudioClipTable audioCliptable)
        {
            _bgmAudioDictionary = audioCliptable;
        }
        
        // stringでSEを再生
        public void PlaySE(string key, bool isCoercion = false)
        {
            if (_seAudioDictionary.IsValue(key))
            {
                if (isCoercion) _audioSourceSE.Stop();
                _audioSourceSE.PlayOneShot(_seAudioDictionary.GetValue(key));
            }
            else
            {
                Debug.LogWarning("<color=red>SEAudioClip\"" + key + "\"は設定されていません</color>");
            }
        }

        // stringでVoiceを再生
        public void PlayVoice(string key)
        {
            if (_voiceAudioDictionary.IsValue(key))
            {
                _audioSourceVoice.Stop();
                _audioSourceVoice.PlayOneShot(_voiceAudioDictionary.GetValue(key));
            }
            else
            {
                Debug.LogWarning("<color=red>VoiceAudioClip\"" + key + "\"は設定されていません</color>");
            }
        }
        
        // stringでBGMを再生
        public void PlayBGM(string key)
        {
            if (_bgmAudioDictionary.IsValue(key))
            {
                _audioSourceBGM.loop = true;
                _audioSourceBGM.clip = _bgmAudioDictionary.GetValue(key);
                _audioSourceBGM.Play();
            }
            else
            {
                Debug.LogWarning("<color=red>BGMAudioClip\"" + key + "\"番は設定されていません</color>");
            }
        }

        // BGMを停止
        public void StopBGM()
        {
            _audioSourceBGM.Stop();
        }
        
        // Master音量の設定
        public void SetMasterVolume(float volume)
        {
            _audioMixer.SetFloat(AUDIO_MIXER_MASTER, volume);
        }
        
        // SE音量の設定
        public void SetSEVolume(float volume)
        {
            _audioMixer.SetFloat(AUDIO_MIXER_SE, volume);
        }

        // SE音量の設定
        public void SetVoiceVolume(float volume)
        {
            _audioMixer.SetFloat(AUDIO_MIXER_VOICE, volume);
        }
        
        // BGM音量の設定
        public void SetBGMVolume(float volume)
        {
            _audioMixer.SetFloat(AUDIO_MIXER_BGM, volume);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        
        // SEの初期化読み込み処理
        protected virtual async Task InitSELoad() { }
        
        // BGMの初期化読み込み処理
        protected virtual async Task InitBGMLoad() { }
    }
}

