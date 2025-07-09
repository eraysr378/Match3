
using Interfaces;
using UnityEngine;

namespace Pools
{
    public class PooledAudioSource : MonoBehaviour, IPoolableObject
    {
        public bool IsInUse { get; private set; }
        private AudioSourcePool _pool;
        private AudioSource _audioSource;
        
        public void Initialize(AudioSourcePool pool)
        {
            _audioSource = GetComponent<AudioSource>();
            
            _pool = pool;
        }
        public void PlayOneShot(AudioClip clip, float pitch, float volume)
        {
            _audioSource.loop = false;
            _audioSource.pitch = pitch;
            _audioSource.volume = volume;
            _audioSource.PlayOneShot(clip);
            var actualLength = (clip.length / pitch);
            Invoke(nameof(OnReturnToPool),actualLength);
        }

        public void StopImmediate()
        {
            _audioSource.Stop();
            OnReturnToPool();
        }

   
        public void OnSpawn()
        {
            IsInUse = true;
        }

        public void OnReturnToPool()
        {
            IsInUse = false;
            _pool.ReturnToPool(this);
            
        }
        //
        // public void PlayLoop(AudioClip clip, float pitch, float volume)
        // {
        //     AudioSource.clip = clip;
        //     AudioSource.pitch = pitch;
        //     AudioSource.volume = volume;
        //     AudioSource.loop = true;
        //     IsInUse = true;
        //     AudioSource.Play();
        // }
        // public void FadeOut(float duration)
        // {
        //     StartCoroutine(FadeOutCoroutine(duration));
        // }
        //
        // private IEnumerator FadeOutCoroutine(float duration)
        // {
        //     float startVolume = AudioSource.volume;
        //     float t = 0f;
        //
        //     while (t < duration)
        //     {
        //         t += Time.deltaTime;
        //         AudioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
        //         yield return null;
        //     }
        //
        //     StopImmediate();
        //     AudioSource.volume = startVolume; // Reset for reuse
        // }
    }
}