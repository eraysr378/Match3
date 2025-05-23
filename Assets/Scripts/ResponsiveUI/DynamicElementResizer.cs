using GoalRelated;
using UnityEngine;
using UnityEngine.UI;

namespace ResponsiveUI
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class DynamicElementResizer : MonoBehaviour
    {
        private HorizontalLayoutGroup _horizontalLayoutGroup;
        private RectTransform _container;
        private int startSpace = 70;
        void Awake()
        {
            _container = GetComponent<RectTransform>();
            _horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
        }

        public Goal AddItem(Goal itemPrefab)
        {
            Goal newItem = Instantiate(itemPrefab, _container);
            return newItem;
        }

        public void ResizeElements()
        {
            int childCount = _container.childCount;
            if (childCount == 0) return;
            float containerWidth = _container.sizeDelta.x;
            float containerHeight = _container.sizeDelta.y;
            
            float spacedContainerWidth =containerWidth - startSpace * _container.transform.childCount;

            // Calculate total original width and maximum original height
            float totalOriginalWidth = 0f;

            foreach (RectTransform child in _container)
            {
                Image image = child.GetComponent<Image>();
                if (image != null && image.sprite != null)
                {
                    totalOriginalWidth += child.sizeDelta.x;
                }
            }

            float totalUpdatedWidth = 0;
            // Resize elements to fit within the container
            foreach (RectTransform child in _container)
            {
                Image image = child.GetComponent<Image>();
                if (image != null && image.sprite != null)
                {
                    // Calculate the aspect ratio of the sprite
                    float aspectRatio = (float)image.sprite.rect.width / image.sprite.rect.height;

                    // Firstly, calculate what element width will be, height is not changed yet
                    float elementWidth = (spacedContainerWidth / totalOriginalWidth) * (child.sizeDelta.y * aspectRatio);
                    // Update the height to not lose aspect ratio
                    float elementHeight = elementWidth / aspectRatio;

                    // After calculating width, ensure the height does not exceed containerHeight
                    if (elementHeight > containerHeight)
                    {
                        elementHeight = containerHeight;
                        elementWidth = elementHeight * aspectRatio;
                    }
                    ScaleChildObjects(child, elementWidth / child.sizeDelta.x, elementHeight / child.sizeDelta.y);

                    // Apply the calculated size to the child
                    child.sizeDelta = new Vector2(elementWidth, elementHeight);
                    totalUpdatedWidth += elementWidth;

                }
            }
            // If the elements fit in and there is space, then place the spaces
            float spacing = (containerWidth - totalUpdatedWidth) / childCount;
            if (spacing > 0)
            {
                _horizontalLayoutGroup.spacing = spacing;
            }
            else
            {
                _horizontalLayoutGroup.spacing = 0;

            }
        }
        // Scale the child objects proportionally
        private void ScaleChildObjects(RectTransform parent, float widthScale, float heightScale)
        {

            foreach (Transform subChild in parent)
            {
                RectTransform rectChild = subChild as RectTransform;
                if (rectChild != null)
                {
                    // Scale the sizeDelta proportionally for each sub-child
                    rectChild.sizeDelta = new Vector2(
                        rectChild.sizeDelta.x * widthScale,
                        rectChild.sizeDelta.y * heightScale
                    );
                }
                else
                {
                    subChild.localScale *= widthScale;
                }

            }
        }
    }
}
