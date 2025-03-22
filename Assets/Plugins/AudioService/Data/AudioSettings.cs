using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.AudioService.Data
{
    [Serializable]
    public class AudioSettings
    {
        public AudioMixerGroup AudioMixerGroup;
        public bool Mute;
        public bool BypassEffects;
        public bool BypassListenerEffects;
        public bool BypassReverbZones;
        public bool Loop;
        public int Priority = 128;
        public float Volume = 1f;
        public float Pitch = 1f;
        public float StereoPan;
        public float SpatialBlend = 1f;
        public float ReverbZoneMix = 1f;
        public float DopplerLevel = 1f;
        public float Spread;
        public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;
        public float MinDistance = 1f;
        public float MaxDistance = 500f;
    }
}