using UnityEngine;

public class SafeAreaAdjuster : MonoBehaviour
{
    private RectTransform _rectTransform;
    void Awake(){
        _rectTransform = GetComponent<RectTransform>();

    }
    void Start()
    {
        if(_rectTransform == null){
            Debug.LogWarning("buuu");
        }
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        // Get the safe area rectangle
        Rect safeArea = Screen.safeArea;

        // Convert the safe area rectangle into anchor values
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        // Normalize values to the range [0,1]
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Apply the normalized anchors to the RectTransform
        if(_rectTransform == null){
            return;
        }
        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }

    void OnRectTransformDimensionsChange()
    {
        ApplySafeArea(); // Reapply safe area in case of screen changes
    }
}