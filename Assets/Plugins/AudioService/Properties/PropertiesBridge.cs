using System;
using System.Collections.Generic;
using Plugins.AudioService.Properties.Core;
using Plugins.Timer;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.AudioService.Properties
{
    public class PropertiesBridge
    {
        public PropertiesBridge(Dictionary<int, AudioService.PooledObject> activePool)
        {
            Func<int, bool> canAccess = activePool.ContainsKey;

            Timer = new ReadonlyProperty<int, IReadonlyTimer>(canAccess, id => activePool[id].Audio.Timer);
            Time = new Property<int, float>(canAccess, id => activePool[id].Audio.Time, (id, value) => activePool[id].Audio.Time = value);
            Position = new Property<int, Vector3>(canAccess, id => activePool[id].Audio.Position, (id, value) => activePool[id].Audio.Position = value);
            Rotation = new Property<int, Quaternion>(canAccess, id => activePool[id].Audio.Rotation, (id, value) => activePool[id].Audio.Rotation = value);
            Clip = new Property<int, AudioClip>(canAccess, id => activePool[id].Audio.Clip, (id, value) => activePool[id].Audio.Clip = value);
            AudioMixerGroup = new Property<int, AudioMixerGroup>(canAccess, id => activePool[id].Audio.AudioMixerGroup,
                (id, value) => activePool[id].Audio.AudioMixerGroup = value);
            Mute = new Property<int, bool>(canAccess, id => activePool[id].Audio.Mute, (id, value) => activePool[id].Audio.Mute = value);
            BypassEffects = new Property<int, bool>(canAccess, id => activePool[id].Audio.BypassEffects, (id, value) => activePool[id].Audio.BypassEffects = value);
            BypassListenerEffects = new Property<int, bool>(canAccess, id => activePool[id].Audio.BypassListenerEffects,
                (id, value) => activePool[id].Audio.BypassListenerEffects = value);
            BypassReverbZones = new Property<int, bool>(canAccess, id => activePool[id].Audio.BypassReverbZones,
                (id, value) => activePool[id].Audio.BypassReverbZones = value);
            Loop = new Property<int, bool>(canAccess, id => activePool[id].Audio.Loop, (id, value) => activePool[id].Audio.Loop = value);
            Priority = new Property<int, int>(canAccess, id => activePool[id].Audio.Priority, (id, value) => activePool[id].Audio.Priority = value);
            Volume = new Property<int, float>(canAccess, id => activePool[id].Audio.Volume, (id, value) => activePool[id].Audio.Volume = value);
            Pitch = new Property<int, float>(canAccess, id => activePool[id].Audio.Pitch, (id, value) => activePool[id].Audio.Pitch = value);
            StereoPan = new Property<int, float>(canAccess, id => activePool[id].Audio.StereoPan, (id, value) => activePool[id].Audio.StereoPan = value);
            SpatialBlend = new Property<int, float>(canAccess, id => activePool[id].Audio.SpatialBlend, (id, value) => activePool[id].Audio.SpatialBlend = value);
            ReverbZoneMix = new Property<int, float>(canAccess, id => activePool[id].Audio.ReverbZoneMix, (id, value) => activePool[id].Audio.ReverbZoneMix = value);
            DopplerLevel = new Property<int, float>(canAccess, id => activePool[id].Audio.DopplerLevel, (id, value) => activePool[id].Audio.DopplerLevel = value);
            Spread = new Property<int, float>(canAccess, id => activePool[id].Audio.Spread, (id, value) => activePool[id].Audio.Spread = value);
            RolloffMode = new Property<int, AudioRolloffMode>(canAccess, id => activePool[id].Audio.RolloffMode,
                (id, value) => activePool[id].Audio.RolloffMode = value);
            MinDistance = new Property<int, float>(canAccess, id => activePool[id].Audio.MinDistance, (id, value) => activePool[id].Audio.MinDistance = value);
            MaxDistance = new Property<int, float>(canAccess, id => activePool[id].Audio.MaxDistance, (id, value) => activePool[id].Audio.MaxDistance = value);
        }

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