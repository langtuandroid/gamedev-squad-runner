using System;
using JetSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class AudioManagersr : MonoBehaviour
    {
        private const string _musicKey = "Music";
        private const string _soundKey = "Sound";
        public const string _vibrtionKey = "Vibration";

        public bool IsVirbration;
        
        [SerializeField] 
        private Toggle _musicTogglesr;
        [SerializeField] 
        private Toggle _soundTogglesr;
        [SerializeField] 
        private Toggle _vibrationTogglesr;
        
        [SerializeField] 
        private Toggle _musicTogglePausesr;
        [SerializeField] 
        private Toggle _soundTogglePausesr;
        [SerializeField] 
        private Toggle _vibrationTogglePausesr;
        
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
            if (Instancesr != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instancesr = this;
            }
            
            Vibration.Init();
            LoadCurrentSettingssr();
            SubscribeToggleEventssr(_musicTogglesr, _musicKey);
            SubscribeToggleEventssr(_soundTogglesr, _soundKey);
            SubscribeToggleEventssr(_vibrationTogglesr, _vibrtionKey);
            SubscribeToggleEventssr(_musicTogglePausesr, _musicKey);
            SubscribeToggleEventssr(_soundTogglePausesr, _soundKey);
            SubscribeToggleEventssr(_vibrationTogglePausesr, _vibrtionKey);
        }

        private void OnDestroy()
        {
            UnsubscribeToggleEventssr(_musicTogglesr);
            UnsubscribeToggleEventssr(_soundTogglesr);
            UnsubscribeToggleEventssr(_vibrationTogglesr);
            UnsubscribeToggleEventssr(_musicTogglePausesr);
            UnsubscribeToggleEventssr(_soundTogglePausesr);
            UnsubscribeToggleEventssr(_vibrationTogglePausesr);
        }

        private void LoadCurrentSettingssr()
        {
            _musicTogglesr.isOn = PlayerPrefsManager.LoadSettings(_musicKey);
            _soundTogglesr.isOn = PlayerPrefsManager.LoadSettings(_soundKey);
            _vibrationTogglesr.isOn = PlayerPrefsManager.LoadSettings(_vibrtionKey);
            _musicTogglePausesr.isOn = PlayerPrefsManager.LoadSettings(_musicKey);
            _soundTogglePausesr.isOn = PlayerPrefsManager.LoadSettings(_soundKey);
            _vibrationTogglePausesr.isOn = PlayerPrefsManager.LoadSettings(_vibrtionKey);
            _musicAudioSourcesr.mute = _musicTogglesr.isOn;
            _sfxAudioSourcesr.mute = _soundTogglesr.isOn;
            IsVirbration = _vibrationTogglesr.isOn;
        }

        private void SubscribeToggleEventssr(Toggle toggle, string key)
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                PlayerPrefsManager.SaveSettings(key, isOn);
                switch (key)
                {
                    case _musicKey:
                        PlaySFXOneShotsr(0);
                        _musicAudioSourcesr.mute = isOn;
                        break;
                    case _soundKey:
                        PlaySFXOneShotsr(0);
                        _sfxAudioSourcesr.mute = isOn;
                        break;
                    case _vibrtionKey:
                        IsVirbration = isOn;
                        Vibration.VibrateNope();
                        break;
                }

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


        public void Start()
        {
            PlayMusicsr(0);
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
