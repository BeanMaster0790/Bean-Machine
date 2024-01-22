﻿using BeanMachine.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace BeanMachine.Sounds
{
    public class SoundManager
    {
        public static SoundManager Instance = new SoundManager();

        private Dictionary<int, Sound> _soundEffects = new Dictionary<int, Sound>();

        private Dictionary<int, SoundEffectInstance> _soundInstances = new Dictionary<int, SoundEffectInstance>();
        private Dictionary<int, SoundEffectInstance> _clonedSoundInstances = new Dictionary<int, SoundEffectInstance>();

        public AudioListener AudioListener = new AudioListener();

        public void AddSound(Sound sound)
        {
            _soundEffects.Add(sound.SoundKey, sound);
        }

        public void RemoveSound(Sound sound) 
        { 
            _soundEffects.Remove(sound.SoundKey);
        }

        public int GenerateSoundKey()
        {
            int key = 0;

            while (true)
            {
                key = Random.RandomInt(int.MinValue, int.MaxValue);

                bool keyTaken = false;

                foreach (KeyValuePair<int,Sound> sound in this._soundEffects)
                {
                    if (sound.Key == key)
                        keyTaken = true;                
                }

                if (!keyTaken)
                {
                    return key;
                }

            }
        }

        public void PlaySound(int key)
        {
            Sound sound = _soundEffects[key];

            SoundEffect soundEffect = sound.Effect;

            SoundEffectInstance instance = soundEffect.CreateInstance();

            instance.IsLooped = sound.IsLooped;

            instance.Play();

            this._soundInstances.Add(key, instance);

            if (sound.Is3D)
            {
                SoundEffectInstance clonedInstance = soundEffect.CreateInstance();

                clonedInstance.IsLooped = sound.IsLooped;

                clonedInstance.Volume = instance.Volume * sound.ClonedSoundModifier;

                clonedInstance.Play();

                this._clonedSoundInstances.Add(key, clonedInstance);
            }
        }

        public void PauseSound(int key)
        {
            this._soundInstances[key].Pause();

            if (this._clonedSoundInstances.ContainsKey(key))
            {
                this._clonedSoundInstances[key].Pause();
            }
        }

        public void ResumeSound(int key)
        {
            this._soundInstances[key].Resume();

            if (this._clonedSoundInstances.ContainsKey(key))
            {
                this._clonedSoundInstances[key].Resume();
            }
        }

        public void StopSound(int key, bool instantly = true)
        {
            this._soundInstances[key].Stop(instantly);

            if (this._clonedSoundInstances.ContainsKey(key))
            {
                this._clonedSoundInstances[key].Stop(instantly);
            }
        }

        public void DestroySound(int key)
        {
            this._soundInstances[key].Stop();
            this._soundInstances[key].Dispose();


            this._soundInstances.Remove(key);

            if (this._clonedSoundInstances.ContainsKey(key))
            {
                this._clonedSoundInstances[key].Stop();
                this._clonedSoundInstances[key].Dispose();

                this._clonedSoundInstances.Remove(key);
            }

            this._soundEffects.Remove(key);
        }

        public void UpdateSoundPan(int key, Vector2 position)
        {
            SoundEffectInstance instance = this._soundInstances[key];
            SoundEffectInstance clonedInstance = this._clonedSoundInstances[key];

            Vector2 direction = position - new Vector2(AudioListener.Position.X, AudioListener.Position.Y);
            direction.Normalize();

            float soundPan = MathHelper.ToDegrees((float)Math.Atan2(direction.Y, direction.X));

            if (direction.Y >= 0)
                soundPan -= 90;
            else
                soundPan += 90;

            soundPan /= 90;

            if(soundPan >= -1 && soundPan <= 1)
            {
                if(direction.Y >= 0)
                {
                    instance.Pan = -soundPan;

                    clonedInstance.Pan = soundPan;     
                }
                else
                {
                    instance.Pan = soundPan;

                    clonedInstance.Pan = -soundPan;
                }
            }
        }

        public void UpdateSoundVoloume(int key, Vector2 position)
        {
            Sound sound = this._soundEffects[key];

            SoundEffectInstance instance = this._soundInstances[key];
            SoundEffectInstance clonedInstance = this._clonedSoundInstances[key];

            float distanceToListener = Vector2.Distance(new Vector2(this.AudioListener.Position.X, this.AudioListener.Position.Y), position);

            if (distanceToListener < sound.FadeOutDistance)
            {
                float fadeOutRegion = sound.FadeOutDistance - sound.FadeOutStart;

                float distanceFromFadeStart = distanceToListener - sound.FadeOutStart;

                float distanceTravelledInRegion = distanceFromFadeStart / fadeOutRegion;

                distanceTravelledInRegion = (1 - distanceTravelledInRegion) / sound.SoundRollOffAmout;

                instance.Volume = sound.MaxVoloume * distanceTravelledInRegion;
                clonedInstance.Volume = instance.Volume * sound.ClonedSoundModifier;
            }
            else
            {
                instance.Volume = 0;
                clonedInstance.Volume = 0;
            }
        }

        public void UpdateSoundModifier(int key)
        {
            Sound sound = this._soundEffects[key];

            SoundEffectInstance instance = this._soundInstances[key];
            SoundEffectInstance clonedInstance = this._clonedSoundInstances[key];

            float soundPan = MathF.Abs(instance.Pan);

            float distanceFromPanZero = 1 - soundPan;

            sound.ClonedSoundModifier = MathHelper.Clamp(distanceFromPanZero, 0.25f, 1f);
        }

    }
}
