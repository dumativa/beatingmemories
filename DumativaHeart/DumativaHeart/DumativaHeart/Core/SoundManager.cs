using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DumativaHeart.Core
{
    static class SoundManager
    {

        static public Dictionary<String, SoundEffect> _sounds = new Dictionary<String, SoundEffect>();
        static public Dictionary<String, Song> _musics = new Dictionary<String, Song>();

        //Volume and control atribute
        static private float _musicVolume = 0;
        static private float _soundVolume = 1;
        static private Boolean _isMusicMuted = false;
        static public Boolean isEffectsMuted = false;
        static private Boolean _isPaused = false;
        static public float soundEffectScale = 100;

        //Music changing atributes
        static private Boolean _isMusicChanging = false;
        static private String _nextMusic = null;
        static private Boolean _fadeIn = false;
        static private Boolean _fadeOut = false;
        static private float _fadeMusicVolume;
        static private float _fadeInVelocity = 0.01f;
        static private float _fadeOutVelocity = 0.01f;
        static public string currentMusic;

        //Visualization Data
        static private VisualizationData _visData = new VisualizationData();

        static public void AddMusic(String name, Song music)
        {
            if(!_musics.Keys.Contains(name))
                _musics.Add(name, music);
        }

        static public void AddSoundEffect(String name, SoundEffect soundEffect)
        {
            if (!_sounds.Keys.Contains(name))
                _sounds.Add(name, soundEffect);
        }

        static public void PlaySoundEffect(String name, float pitch = 0, float pan = 0)
        {
            SoundEffectInstance soundInstance = _sounds[name].CreateInstance();
            

            if (!isEffectsMuted)
                soundInstance.Volume = _soundVolume;
            else
                soundInstance.Volume = 0;

             
            soundInstance.Pitch = pitch;
            soundInstance.Pan = pan;
            soundInstance.Play();
        }

        static public void PlaySoundEffect(String name, Vector2 listenerPosition, Vector2 emitterPosition, float pitch = 0)
        {
            AudioListener listener = new AudioListener();
            listener.Position = new Vector3(listenerPosition.X / soundEffectScale, listenerPosition.Y / soundEffectScale, 0);
            AudioEmitter emitter = new AudioEmitter();
            emitter.Position = new Vector3(emitterPosition.X / soundEffectScale, listenerPosition.Y / soundEffectScale, 0);

            
            

            SoundEffectInstance soundInstance = _sounds[name].CreateInstance();


            if (!isEffectsMuted)
                soundInstance.Volume = _soundVolume;
            else
                soundInstance.Volume = 0;

            soundInstance.Pitch = pitch;
            soundInstance.Apply3D(listener, emitter);
            soundInstance.Play();

        }

        static public void ToggleMusicMute()
        {
            _isMusicMuted = !_isMusicMuted;
            MediaPlayer.IsMuted = true;
        }

        static public void ToggleEffectsMute()
        {
            isEffectsMuted = !isEffectsMuted;
            MediaPlayer.IsMuted = true;
        }

        static public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (!_isPaused)
                MediaPlayer.Resume();
            else
                MediaPlayer.Pause();
        }

        static public void StopMusic()
        {
            MediaPlayer.Stop();
        }

        static public void SetMusic(String name, Boolean repeating = true, Boolean fadeOut = false, Boolean fadeIn = false)
        {
            if (currentMusic == name)
                return;

            MediaPlayer.IsRepeating = repeating;
            MediaPlayer.IsVisualizationEnabled = false;

            currentMusic = name;

            if (!fadeIn && !fadeOut)
            {
                MediaPlayer.Play(_musics[name]);
            }
            else
            {
                _fadeIn = fadeIn;
                _fadeOut = fadeOut;
                _nextMusic = name;
                _isMusicChanging = true;
                _fadeMusicVolume = _musicVolume;

                if (!fadeOut && fadeIn)
                {
                    _fadeMusicVolume = 0;
                    MediaPlayer.Volume = 0;
                    MediaPlayer.Play(_musics[_nextMusic]);
                }
            }
        }

        static public void Update()
        {
            validateMusicChange();
            validateVisualizationData();
        }

        private static void validateVisualizationData()
        {
            MediaPlayer.GetVisualizationData(_visData);
        }

        private static void validateMusicChange()
        {
            if (_isMusicChanging)
            {
                if (_fadeOut)
                {
                    _fadeMusicVolume -= _musicVolume * _fadeOutVelocity;
                    MediaPlayer.Volume = _fadeMusicVolume;

                    if (_fadeMusicVolume <= 0)
                    {
                        if (!_fadeIn)
                        {
                            MediaPlayer.Play(_musics[_nextMusic]);
                            _fadeIn = false;
                            _fadeOut = false;
                            MediaPlayer.Volume = _musicVolume;
                            _isMusicChanging = false;
                            _nextMusic = null;
                        }
                        else
                        {
                            MediaPlayer.Play(_musics[_nextMusic]);
                            _fadeOut = false;
                        }
                    }
                }
                else if (_fadeIn)
                {
                    _fadeMusicVolume += _musicVolume * _fadeInVelocity;
                    MediaPlayer.Volume = _fadeMusicVolume;

                    if (_fadeMusicVolume >= _musicVolume)
                    {
                        MediaPlayer.Volume = _musicVolume;
                        _fadeIn = false;
                        _fadeOut = false;
                        MediaPlayer.Volume = _musicVolume;
                        _isMusicChanging = false;
                        _nextMusic = null;
                    }
                }
            }
        }

        public static float MusicActivityBySample(float Amplifier = 0.5f)
        {

            List<float> temp = SoundManager.MusicVisualizationData.Samples.ToList<float>();

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] < 0)
                    temp[i] *= -1;
            }

            return temp.Average() / Amplifier;
        }


        #region Gets & Sets

        static public float SoundVolume
        {
            get
            {
                return _soundVolume;
            }

            set
            {
                _soundVolume = MathHelper.Clamp(value, 0, 1);
            }
        }


        static public float MusicVolume
        {
            get
            {
                return _musicVolume;
            }

            set
            {
                _musicVolume = MathHelper.Clamp(value, 0, 1);
                MediaPlayer.Volume = _musicVolume;
            }
        }

        static public float FadeInVelocity
        {
            get
            {
                return _fadeInVelocity;
            }

            set
            {
                _fadeInVelocity = MathHelper.Clamp(value, 0, 1);
            }
        }

        static public float FadeOutVelocity
        {
            get
            {
                return _fadeOutVelocity;
            }

            set
            {
                _fadeOutVelocity = MathHelper.Clamp(value, 0, 1);
            }
        }

        static public Boolean IsMusicChanging
        {
            get
            {
                return _isMusicChanging;
            }
        }

        static public Boolean IsMusicMuted
        {
            get
            {
                return _isMusicMuted;
            }
            set
            {
                _isMusicMuted = value;
                MediaPlayer.IsMuted = _isMusicMuted;
            }
        }

        static public Boolean IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;

                if (!_isPaused)
                    MediaPlayer.Resume();
                else
                    MediaPlayer.Pause();
            }
        }

        static public VisualizationData MusicVisualizationData
        {
            get
            {
                return _visData;
            }
        }
        
        #endregion

    }
}
