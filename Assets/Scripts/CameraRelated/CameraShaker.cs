using DG.Tweening;
using Managers;
using UnityEngine;

namespace CameraRelated
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float smallShakeDuration = 0.25f;
        [SerializeField] private float smallShakeStrength = 0.25f;
        [SerializeField] private int smallShakeVibrato = 10;

        [SerializeField] private float bigShakeDuration = 0.4f;
        [SerializeField] private float bigShakeStrength = 0.4f;
        [SerializeField] private int bigShakeVibrato = 20;
        private Vector3 _originalPos;
        private Tween _shakeTween;

        private void OnEnable()
        {
            CameraInitializer.OnCameraInitialized += CameraInitializer_OnCameraInitialized;
            EventManager.OnSmallCameraShakeRequest += SmallShake;
            EventManager.OnBigCameraShakeRequest += BigShake;
        }

        private void OnDisable()
        {
            CameraInitializer.OnCameraInitialized -= CameraInitializer_OnCameraInitialized;
            EventManager.OnSmallCameraShakeRequest -= SmallShake;
            EventManager.OnBigCameraShakeRequest -= BigShake;
        }

        private void CameraInitializer_OnCameraInitialized()
        {
            _originalPos = transform.position;
        }


        private void Shake(float duration, float strength, int vibrato = 10, float randomness = 90f)
        {
            _shakeTween?.Kill();
            transform.position = _originalPos; // oncomplete may not run
            _shakeTween = transform.DOShakePosition(
                duration,
                strength,
                vibrato,
                randomness,
                snapping: false,
                fadeOut: true
            ).OnComplete(() =>
            {
                transform.position = _originalPos;
            });
        }

        private void SmallShake()
        {
            Shake(smallShakeDuration, smallShakeStrength,smallShakeVibrato);
        }

        private void BigShake()
        {
            Shake(bigShakeDuration, bigShakeStrength,bigShakeVibrato);

        }
    }
}