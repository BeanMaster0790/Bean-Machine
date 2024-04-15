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

            Parent.Scene.SoundManager.AddSound(sound);
        }

        public void RemoveSound(string name)
        {
            this.RemoveSound(name);

            Parent.Scene.SoundManager.RemoveSound(this._sounds[name]);
        }

        public void PlaySound(string name)
        {
            Parent.Scene.SoundManager.PlaySound(this._sounds[name].SoundKey);
        }

        public void PlaySound(string name, Vector2 position)
        {
            Parent.Scene.SoundManager.PlaySound(this._sounds[name].SoundKey);
        }

        public void PauseSound(string name)
        {
            Parent.Scene.SoundManager.PauseSound(this._sounds[name].SoundKey);
        }

        public void StopSound(string name)
        {
            Parent.Scene.SoundManager.StopSound(this._sounds[name].SoundKey);
        }

        public void ResumeSound(string name)
        {
            Parent.Scene.SoundManager.ResumeSound(this._sounds[name].SoundKey);
        }

        public override void Update()
        {
            base.Update();

            foreach (KeyValuePair<string, Sound> sound in this._sounds)
            {
                if (sound.Value.Is3D)
                {
                    Parent.Scene.SoundManager.UpdateSoundPan(sound.Value.SoundKey, base.Parent.Position);
                    Parent.Scene.SoundManager.UpdateSoundVoloume(sound.Value.SoundKey, base.Parent.Position);
                    Parent.Scene.SoundManager.UpdateSoundModifier(sound.Value.SoundKey);
                }
            }
        }

        public override void Destroy()
        {
            foreach (KeyValuePair<string, Sound> sound in this._sounds)
            {
                Parent.Scene.SoundManager.DestroySound(sound.Value.SoundKey);
            }

            base.Destroy();
        }
    }
}
