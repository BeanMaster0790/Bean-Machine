using BeanMachine.Debug;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BeanMachine.Sounds
{
    public class SoundHolder : Addon
    {
        private Dictionary<string, Sound> _sounds = new Dictionary<string, Sound>();

        public void AddSound(string name, Sound sound)
        {
            this._sounds.Add(name, sound);

            SoundManager.Instance.AddSound(sound);
        }

        public void RemoveSound(string name)
        {
            this.RemoveSound(name);

            SoundManager.Instance.RemoveSound(this._sounds[name]);
        }

        public void PlaySound(string name)
        {
            SoundManager.Instance.PlaySound(this._sounds[name].SoundKey);
        }

        public void PlaySound(string name, Vector2 position)
        {
            SoundManager.Instance.PlaySound(this._sounds[name].SoundKey);
        }

        public void PauseSound(string name)
        {
            SoundManager.Instance.PauseSound(this._sounds[name].SoundKey);
        }

        public void StopSound(string name)
        {
            SoundManager.Instance.StopSound(this._sounds[name].SoundKey);
        }

        public void ResumeSound(string name)
        {
            SoundManager.Instance.ResumeSound(this._sounds[name].SoundKey);
        }

        public override void Update()
        {
            base.Update();

            foreach (KeyValuePair<string, Sound> sound in this._sounds)
            {
                if (sound.Value.Is3D)
                {
                    SoundManager.Instance.UpdateSoundPan(sound.Value.SoundKey, base.Parent.Position);
                    SoundManager.Instance.UpdateSoundVoloume(sound.Value.SoundKey, base.Parent.Position);
                    SoundManager.Instance.UpdateSoundModifier(sound.Value.SoundKey);
                }
            }
        }

        public override void Destroy()
        {
            foreach (KeyValuePair<string,Sound> sound in this._sounds)
            {
                SoundManager.Instance.DestroySound(sound.Value.SoundKey);
            }

            base.Destroy();
        }
    }
}
