using System;
using Plugins.AudioService.Facade.Core;
using Plugins.AudioService.Properties.Core;
using Plugins.Timer;
using UnityEngine;
using UnityEngine.Audio;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;

namespace Plugins.AudioService.Core
{
    public interface IAudioService
    {
        public int Play(AudioClip clip, Vector3 position, Quaternion rotation, AudioSettings settings);
        public int Play(AudioClip clip, Vector3 position, AudioSettings settings);
        public int Play(AudioClip clip, AudioSettings settings);
        public int Play(AudioClip clip, Vector3 position);
        public void Pause(int id);
        public void PauseAll();
        public void PauseAll(Func<IReadonlyAudio, bool> predicate);
        public void Resume(int id);
        public void ResumeAll();
        public void ResumeAll(Func<IReadonlyAudio, bool> predicate);
        public void Stop(int id);
        public void StopAll();
        public void StopAll(Func<IReadonlyAudio, bool> predicate);
        public bool IsActive(int id);
        public int ActiveAudiosCount();
        public int ActiveAudiosCount(Func<IReadonlyAudio, bool> predicate);
        public void ApplySettings(int id, AudioSettings settings);

        public IReadonlyProperty<int, IReadonlyTimer> Timer { get; }
        public IProperty<int, float> Time { get; }
        public IProperty<int, Vector3> Position { get; }
        public IProperty<int, Quaternion> Rotation { get; }
        public IProperty<int, AudioClip> Clip { get; }
        public IProperty<int, AudioMixerGroup> AudioMixerGroup { get; }
        public IProperty<int, bool> Mute { get; }
        public IProperty<int, bool> BypassEffects { get; }
        public IProperty<int, bool> BypassListenerEffects { get; }
        public IProperty<int, bool> BypassReverbZones { get; }
        public IProperty<int, bool> Loop { get; }
        public IProperty<int, int> Priority { get; }
        public IProperty<int, float> Volume { get; }
        public IProperty<int, float> Pitch { get; }
        public IProperty<int, float> StereoPan { get; }
        public IProperty<int, float> SpatialBlend { get; }
        public IProperty<int, float> ReverbZoneMix { get; }
        public IProperty<int, float> DopplerLevel { get; }
        public IProperty<int, float> Spread { get; }
        public IProperty<int, AudioRolloffMode> RolloffMode { get; }
        public IProperty<int, float> MinDistance { get; }
        public IProperty<int, float> MaxDistance { get; }
    }
}