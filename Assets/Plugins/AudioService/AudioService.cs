using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.AudioService.Core;
using Plugins.AudioService.Facade;
using Plugins.AudioService.Facade.Core;
using Plugins.AudioService.Properties;
using Plugins.AudioService.Properties.Core;
using Plugins.AudioService.Services.ID;
using Plugins.AudioService.Services.ID.Core;
using Plugins.Timer;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Audio;
using AudioSettings = Plugins.AudioService.Data.AudioSettings;
using Object = UnityEngine.Object;

namespace Plugins.AudioService
{
    public class AudioService : IAudioService
    {
        private readonly Preferences _preferences;
        private readonly IIDService _idService;
        private readonly Transform _root;

        public AudioService(Preferences preferences)
        {
            _preferences = preferences;
            _idService = new IDService();

            GameObject gameObject = new GameObject("AudioService");
            Object.DontDestroyOnLoad(gameObject);
            _root = gameObject.transform;

            AudioConfiguration audioConfiguration = UnityEngine.AudioSettings.GetConfiguration();

            if (_preferences.MaxSize > audioConfiguration.numVirtualVoices)
            {
                Debug.LogWarning("MaxSize is greater than the number of virtual voices. MaxSize will be set to the number of virtual voices.");
                _preferences.MaxSize = audioConfiguration.numVirtualVoices;
            }

            _preferences.InitialSize = Mathf.Min(_preferences.InitialSize, _preferences.MaxSize);

            Initialize();

            #region PropertiesInitialization

            Func<int, bool> canAccess = id => _activePool.ContainsKey(id);

            Timer = new ReadonlyProperty<int, IReadonlyTimer>(canAccess, id => _activePool[id].Audio.Timer);
            Time = new Property<int, float>(canAccess, id => _activePool[id].Audio.Time, (id, value) => _activePool[id].Audio.Time = value);
            Position = new Property<int, Vector3>(canAccess, id => _activePool[id].Audio.Position, (id, value) => _activePool[id].Audio.Position = value);
            Rotation = new Property<int, Quaternion>(canAccess, id => _activePool[id].Audio.Rotation, (id, value) => _activePool[id].Audio.Rotation = value);
            Clip = new Property<int, AudioClip>(canAccess, id => _activePool[id].Audio.Clip, (id, value) => _activePool[id].Audio.Clip = value);
            AudioMixerGroup = new Property<int, AudioMixerGroup>(canAccess, id => _activePool[id].Audio.AudioMixerGroup, (id, value) => _activePool[id].Audio.AudioMixerGroup = value);
            Mute = new Property<int, bool>(canAccess, id => _activePool[id].Audio.Mute, (id, value) => _activePool[id].Audio.Mute = value);
            BypassEffects = new Property<int, bool>(canAccess, id => _activePool[id].Audio.BypassEffects, (id, value) => _activePool[id].Audio.BypassEffects = value);
            BypassListenerEffects = new Property<int, bool>(canAccess, id => _activePool[id].Audio.BypassListenerEffects, (id, value) => _activePool[id].Audio.BypassListenerEffects = value);
            BypassReverbZones = new Property<int, bool>(canAccess, id => _activePool[id].Audio.BypassReverbZones, (id, value) => _activePool[id].Audio.BypassReverbZones = value);
            Loop = new Property<int, bool>(canAccess, id => _activePool[id].Audio.Loop, (id, value) => _activePool[id].Audio.Loop = value);
            Priority = new Property<int, int>(canAccess, id => _activePool[id].Audio.Priority, (id, value) => _activePool[id].Audio.Priority = value);
            Volume = new Property<int, float>(canAccess, id => _activePool[id].Audio.Volume, (id, value) => _activePool[id].Audio.Volume = value);
            Pitch = new Property<int, float>(canAccess, id => _activePool[id].Audio.Pitch, (id, value) => _activePool[id].Audio.Pitch = value);
            StereoPan = new Property<int, float>(canAccess, id => _activePool[id].Audio.StereoPan, (id, value) => _activePool[id].Audio.StereoPan = value);
            SpatialBlend = new Property<int, float>(canAccess, id => _activePool[id].Audio.SpatialBlend, (id, value) => _activePool[id].Audio.SpatialBlend = value);
            ReverbZoneMix = new Property<int, float>(canAccess, id => _activePool[id].Audio.ReverbZoneMix, (id, value) => _activePool[id].Audio.ReverbZoneMix = value);
            DopplerLevel = new Property<int, float>(canAccess, id => _activePool[id].Audio.DopplerLevel, (id, value) => _activePool[id].Audio.DopplerLevel = value);
            Spread = new Property<int, float>(canAccess, id => _activePool[id].Audio.Spread, (id, value) => _activePool[id].Audio.Spread = value);
            RolloffMode = new Property<int, AudioRolloffMode>(canAccess, id => _activePool[id].Audio.RolloffMode, (id, value) => _activePool[id].Audio.RolloffMode = value);
            MinDistance = new Property<int, float>(canAccess, id => _activePool[id].Audio.MinDistance, (id, value) => _activePool[id].Audio.MinDistance = value);
            MaxDistance = new Property<int, float>(canAccess, id => _activePool[id].Audio.MaxDistance, (id, value) => _activePool[id].Audio.MaxDistance = value);

            #endregion
        }

        #region Properties

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

        #endregion

        private readonly HashSet<PooledObject> _totalPool = new HashSet<PooledObject>();
        private readonly Dictionary<int, PooledObject> _activePool = new Dictionary<int, PooledObject>();
        private readonly HashSet<PooledObject> _inactivePool = new HashSet<PooledObject>();

        public int Play(AudioClip clip, Vector3 position, Quaternion rotation, AudioSettings settings)
        {
            PooledObject pooledObject = GetFree();
            IAudio audio = pooledObject.Audio;
            audio.Clip = clip;
            audio.Position = position;
            audio.Rotation = rotation;
            audio.ApplySettings(settings);
            audio.Play();
            return pooledObject.ID;
        }

        public int Play(AudioClip clip, Vector3 position, AudioSettings settings) => Play(clip, position, Quaternion.identity, settings);

        public int Play(AudioClip clip, AudioSettings settings) => Play(clip, Vector3.zero, Quaternion.identity, settings);

        public int Play(AudioClip clip, Vector3 position) => Play(clip, position, Quaternion.identity, _preferences.DefaultSettings);

        public void Pause(int id)
        {
            if (_activePool.TryGetValue(id, out PooledObject pooledObject))
                pooledObject.Audio.Pause();
        }

        public void PauseAll()
        {
            foreach (PooledObject pooledObject in _activePool.Values)
            {
                pooledObject.Audio.Pause();
            }
        }

        public void PauseAll(Func<IReadonlyAudio, bool> predicate)
        {
            foreach (PooledObject pooledObject in _activePool.Values)
            {
                if (predicate(pooledObject.Audio))
                    pooledObject.Audio.Pause();
            }
        }

        public void Resume(int id)
        {
            if (_activePool.TryGetValue(id, out PooledObject pooledObject))
                pooledObject.Audio.Resume();
        }

        public void ResumeAll()
        {
            foreach (PooledObject pooledObject in _activePool.Values)
            {
                pooledObject.Audio.Resume();
            }
        }

        public void ResumeAll(Func<IReadonlyAudio, bool> predicate)
        {
            foreach (PooledObject pooledObject in _activePool.Values)
            {
                if (predicate(pooledObject.Audio))
                    pooledObject.Audio.Resume();
            }
        }

        public void Stop(int id)
        {
            if (_activePool.TryGetValue(id, out PooledObject pooledObject))
                pooledObject.Audio.Stop();
        }

        public void StopAll()
        {
            foreach (PooledObject pooledObject in _activePool.Values.ToList())
            {
                pooledObject.Audio.Stop();
            }
        }

        public void StopAll(Func<IReadonlyAudio, bool> predicate)
        {
            foreach (PooledObject pooledObject in _activePool.Values.ToList())
            {
                if (predicate(pooledObject.Audio))
                    pooledObject.Audio.Stop();
            }
        }

        public bool IsActive(int id) => _activePool.ContainsKey(id);

        public int ActiveAudiosCount() => _activePool.Count;

        public int ActiveAudiosCount(Func<IReadonlyAudio, bool> predicate)
        {
            int count = 0;

            foreach (PooledObject pooledObject in _activePool.Values)
            {
                if (predicate(pooledObject.Audio))
                    count++;
            }

            return count;
        }

        public void ApplySettings(int id, AudioSettings settings)
        {
            if (_activePool.TryGetValue(id, out PooledObject pooledObject))
                pooledObject.Audio.ApplySettings(settings);
        }

        private void Initialize() => Expand(_preferences.InitialSize);

        private void Expand()
        {
            GameObject gameObject = new GameObject("Audio");
            gameObject.transform.SetParent(_root);
            gameObject.SetActive(false);

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            IAudio audio = new Audio(audioSource);

            PooledObject pooledObject = new PooledObject
            {
                Audio = audio,
                GameObject = gameObject
            };

            _totalPool.Add(pooledObject);
            _inactivePool.Add(pooledObject);

            StartObserving(pooledObject);
        }

        private void Expand(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Expand();
            }
        }

        private void StartObserving(PooledObject pooledObject)
        {
            pooledObject.Subscriptions.Add(pooledObject.GameObject.OnEnableAsObservable().Subscribe(_ => OnEnabled(pooledObject)));
            pooledObject.Subscriptions.Add(pooledObject.GameObject.OnDisableAsObservable().Subscribe(_ => OnDisabled(pooledObject)));
            pooledObject.Subscriptions.Add(pooledObject.GameObject.OnDestroyAsObservable().Subscribe(_ => OnDestroyed(pooledObject)));
        }

        private void StopObserving(PooledObject pooledObject) => pooledObject.Subscriptions.Dispose();

        private void OnEnabled(PooledObject pooledObject)
        {
            pooledObject.ID = _idService.Get();
            _activePool.Add(pooledObject.ID, pooledObject);
            _inactivePool.Remove(pooledObject);
        }

        private void OnDisabled(PooledObject pooledObject)
        {
            _activePool.Remove(pooledObject.ID);
            _inactivePool.Add(pooledObject);
        }

        private void OnDestroyed(PooledObject pooledObject)
        {
            StopObserving(pooledObject);
            _activePool.Remove(pooledObject.ID);
            _inactivePool.Remove(pooledObject);
            _totalPool.Remove(pooledObject);
            pooledObject.Audio.Stop();
        }

        private PooledObject GetFree()
        {
            if (_inactivePool.Count > 0)
                return MakeActive(_inactivePool.First());

            if (_totalPool.Count < _preferences.MaxSize)
            {
                Expand();
                return GetFree();
            }

            return MakeActive(GetLessImportant());
        }

        private PooledObject MakeActive(PooledObject pooledObject)
        {
            pooledObject.GameObject.SetActive(true);
            return pooledObject;
        }

        private PooledObject GetLessImportant()
        {
            int minPriority = int.MinValue;
            float maxPlayTime = float.MinValue;
            bool allPrioritiesAreEqual = true;

            PooledObject foundPooledObject = null;
            PooledObject lessImportantPooledObject = null;
            PooledObject longestPlayedPooledObject = null;

            ICollection<PooledObject> activePool = _activePool.Values;
            minPriority = activePool.First().Audio.Priority;

            foreach (PooledObject pooledObject in activePool)
            {
                if (pooledObject.Audio.Priority > minPriority)
                {
                    minPriority = pooledObject.Audio.Priority;
                    lessImportantPooledObject = pooledObject;
                    allPrioritiesAreEqual = false;
                }

                if (pooledObject.Audio.Timer.Time.TotalSeconds.Value > maxPlayTime)
                {
                    maxPlayTime = (float)pooledObject.Audio.Timer.Time.TotalSeconds.Value;
                    longestPlayedPooledObject = pooledObject;
                }
            }

            foundPooledObject = allPrioritiesAreEqual ? longestPlayedPooledObject : lessImportantPooledObject;

            foundPooledObject?.Audio.Stop();
            return foundPooledObject;
        }

        [Serializable]
        public class Preferences
        {
            public int InitialSize = 10;
            public int MaxSize = 50;
            public AudioSettings DefaultSettings;
        }

        [Serializable]
        private class PooledObject
        {
            public IAudio Audio;
            public GameObject GameObject;
            public int ID;
            public CompositeDisposable Subscriptions = new CompositeDisposable();
        }
    }
}