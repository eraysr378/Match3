using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScaler : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Scale the background without breaking the aspect ratio
    void Start()
    {
        // Get the size of the sprite
        Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;

        // Get the safe area in screen space (pixels)
        Rect safeArea = Screen.safeArea;

        // Convert safe area size from pixels to world units
        float safeWidthInWorld = safeArea.width / Screen.width * Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        float safeHeightInWorld = safeArea.height / Screen.height * Camera.main.orthographicSize * 2;

        // Calculate the scale factor to fit the safe area
        float scaleFactor = Mathf.Max(safeWidthInWorld / spriteSize.x, safeHeightInWorld / spriteSize.y);

        // Apply the scale to the GameObject
        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // Convert the safe area center from screen space to world space
        Vector3 safeAreaCenter = Camera.main.ScreenToWorldPoint(new Vector3(
            safeArea.x + safeArea.width / 2,  // X center of the safe area
            safeArea.y + safeArea.height / 2, // Y center of the safe area
            Camera.main.nearClipPlane         // Z position for the camera plane
        ));

        // Get the screen center in world space
        Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width / 2,  // X center of the screen
            Screen.height / 2, // Y center of the screen
            Camera.main.nearClipPlane // Z position for the camera plane
        ));

        // Calculate the offset between the safe area center and the screen center
        Vector3 offset = safeAreaCenter - screenCenter;

        // Move the background by the calculated offset
        transform.localPosition += offset;

    }
}