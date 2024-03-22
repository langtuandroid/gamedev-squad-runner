using System;
using JetSystems;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Settings
{
    public class AudioManagersr : MonoBehaviour
    {
        
        private const string _soundKey = "Sound";
        
        [SerializeField] 
        private Toggle _soundTogglesr;
        [SerializeField] 
        private Image _soundOn;
        [SerializeField]
        public AudioMixer _audioMixer;
        [SerializeField] 
        private AudioSource _musicAudioSourcesr; 
        [SerializeField] 
        private AudioClip[] _musicTrackssr; 
        [SerializeField] 
        private AudioSource _sfxAudioSourcesr; 
        [SerializeField] 
        private AudioClip[] _sfxTrackssr;

        public static AudioManagersr Instancesr { get; private set; }
        
        private void Awake()
        {
            _audioMixer.SetFloat("MasterMixer",  -80f);
            if (Instancesr != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instancesr = this;
            }
            
            Vibration.Init();
            SubscribeToggleEventssr(_soundTogglesr, _soundKey);
        }

        private void OnDestroy()
        {
            UnsubscribeToggleEventssr(_soundTogglesr);
        }
        
        public void Start()
        {
            PlayMusicsr(0);
            LoadCurrentSettingssr();
        }

        private void LoadCurrentSettingssr()
        {
            _soundTogglesr.isOn = PlayerPrefsManager.LoadSettings(_soundKey);
            _audioMixer.SetFloat("MasterMixer",  _soundTogglesr.isOn ? -80f : 0f);
            _soundOn.enabled = !_soundTogglesr.isOn;
        }

        private void SubscribeToggleEventssr(Toggle toggle, string key)
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                PlayerPrefsManager.SaveSettings(key, isOn);
                LoadCurrentSettingssr();
            });
        }
        
        private void UnsubscribeToggleEventssr(Toggle toggle)
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(isOn => { });
            }
        }

        
        private void PlayMusicsr(int trackIndex)
        {
            if (trackIndex >= 0 && trackIndex < _musicTrackssr.Length)
            {
                _musicAudioSourcesr.clip = _musicTrackssr[trackIndex];
                _musicAudioSourcesr.Play();
            }
            else
            {
                Debug.LogError("Invalid Musictrack index");
            }
        }

        public void PlaySFXOneShotsr(int trackIndex)
        {
            if (trackIndex >= 0 && trackIndex < _sfxTrackssr.Length)
            {
                _sfxAudioSourcesr.PlayOneShot(_sfxTrackssr[trackIndex]);
            }
            else
            {
                Debug.LogError("Invalid UItrack index");
            }
        }
        
        private bool IsPrimeNumbersr(int number)
        {
            if (number <= 1)
                return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
        
    }
}
