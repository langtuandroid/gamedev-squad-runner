using JetSystems;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        private const string _musicGroupName = "MusicVolume";
        private const string _sfxGroupName = "SFXVolume";
        public const string _vibrtionGroupName = "Vibration";

        public bool IsVirbration;
        
        [SerializeField] 
        private Toggle _musicToggle;
        [SerializeField] 
        private Toggle _soundToggle;
        [SerializeField] 
        private Toggle _vibrationToggle;
        
        [SerializeField] 
        private Toggle _musicTogglePause;
        [SerializeField] 
        private Toggle _soundTogglePause;
        [SerializeField] 
        private Toggle _vibrationTogglePause;
        
        [SerializeField] 
        private AudioMixer _audioMixer;
        [SerializeField] 
        private AudioSource _musicAudioSource; 
        [SerializeField] 
        private AudioClip[] _musicTracks; 
        [SerializeField] 
        private AudioSource _sfxAudioSource; 
        [SerializeField] 
        private AudioClip[] _sfxTracks;
        
       // private Settings _settingsData;

        public static AudioManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            //_settingsData = new Settings();
            Vibration.Init();
            LoadCurrentSettings();
            SubscribeToggleEvents(_musicToggle, _musicGroupName);
            SubscribeToggleEvents(_soundToggle, _sfxGroupName);
            SubscribeToggleEvents(_vibrationToggle, _vibrtionGroupName);
            SubscribeToggleEvents(_musicTogglePause, _musicGroupName);
            SubscribeToggleEvents(_soundTogglePause, _sfxGroupName);
            SubscribeToggleEvents(_vibrationTogglePause, _vibrtionGroupName);
        }

        private void OnDestroy()
        {
            UnsubscribeToggleEvents(_musicToggle);
            UnsubscribeToggleEvents(_soundToggle);
            UnsubscribeToggleEvents(_vibrationToggle);
            UnsubscribeToggleEvents(_musicTogglePause);
            UnsubscribeToggleEvents(_soundTogglePause);
            UnsubscribeToggleEvents(_vibrationTogglePause);
        }

        private void LoadCurrentSettings()
        {
            _musicToggle.isOn = PlayerPrefsManager.LoadSettings(_musicGroupName);
            _soundToggle.isOn = PlayerPrefsManager.LoadSettings(_sfxGroupName);
            _vibrationToggle.isOn = PlayerPrefsManager.LoadSettings(_vibrtionGroupName);
            _musicTogglePause.isOn = PlayerPrefsManager.LoadSettings(_musicGroupName);
            _soundTogglePause.isOn = PlayerPrefsManager.LoadSettings(_sfxGroupName);
            _vibrationTogglePause.isOn = PlayerPrefsManager.LoadSettings(_vibrtionGroupName);
            _musicAudioSource.mute = _musicToggle.isOn;
            _sfxAudioSource.mute = _soundToggle.isOn;
            IsVirbration = _vibrationToggle.isOn;
        }

        private void SubscribeToggleEvents(Toggle toggle, string key)
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                PlayerPrefsManager.SaveSettings(key, isOn);
                switch (key)
                {
                    case _musicGroupName:
                        PlaySFXOneShot(0);
                        _musicAudioSource.mute = isOn;
                        break;
                    case _sfxGroupName:
                        PlaySFXOneShot(0);
                        _sfxAudioSource.mute = isOn;
                        break;
                    case _vibrtionGroupName:
                        IsVirbration = isOn;
                         Vibration.VibrateNope();
                        break;
                }

                LoadCurrentSettings();
            });
        }
        
        private void UnsubscribeToggleEvents(Toggle toggle)
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(isOn => { });
            }
        }


        public void Start()
        {
            PlayMusic(0);
        }

        public void PlayMusic(int trackIndex)
        {
            if (trackIndex >= 0 && trackIndex < _musicTracks.Length)
            {
                _musicAudioSource.clip = _musicTracks[trackIndex];
                _musicAudioSource.Play();
            }
            else
            {
                Debug.LogError("Invalid Musictrack index");
            }
        }

        public void PlaySFXOneShot(int trackIndex)
        {
            if (trackIndex >= 0 && trackIndex < _sfxTracks.Length)
            {
                _sfxAudioSource.PlayOneShot(_sfxTracks[trackIndex]);
            }
            else
            {
                Debug.LogError("Invalid UItrack index");
            }
        }
    }
}
