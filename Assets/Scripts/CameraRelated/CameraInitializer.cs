using System;
using UnityEngine;

namespace CameraRelated
{
    public static class CameraInitializer
    {
        public static event Action OnCameraInitialized;
        public static void ComputeCameraView(int height, int width)
        {
            Camera cam = Camera.main;

            Vector3 center = new Vector3((float)width / 2, (float)height / 2, 0);
            cam.transform.position = center + Vector3.back * 10.0f + Vector3.up * 0.75f;

            float halfSize = 0.0f;
            
            if (Screen.height > Screen.width)
            {
                float screenRatio = Screen.height / (float)Screen.width;
                halfSize = ((width + 1) * 0.5f) * screenRatio;
            }
            else
            {
                //On Wide screen, we fit vertically
                halfSize = (height + 3) * 0.5f ;
            }
            cam.orthographicSize = halfSize;
            OnCameraInitialized?.Invoke();
        }
    }
}