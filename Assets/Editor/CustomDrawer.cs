using UnityEditor;
using UnityEngine;
using GridRelated;

namespace Editor
{
    [CustomPropertyDrawer(typeof(CustomGrid))]
    public class CustomGridDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PrefixLabel(position, label);

            var rowsProperty = property.FindPropertyRelative("height");
            var columnsProperty = property.FindPropertyRelative("width");
            var dataProperty = property.FindPropertyRelative("cellData");

            position.y += EditorGUIUtility.singleLineHeight;

            rowsProperty.intValue = EditorGUI.IntField(
                new Rect(position.x, position.y, position.width / 2 - 5, EditorGUIUtility.singleLineHeight),
                "Rows", rowsProperty.intValue);

            columnsProperty.intValue = EditorGUI.IntField(
                new Rect(position.x + position.width / 2 + 5, position.y, position.width / 2 - 5,
                    EditorGUIUtility.singleLineHeight),
                "Columns", columnsProperty.intValue);

            int totalSize = Mathf.Max(1, rowsProperty.intValue * columnsProperty.intValue);

            if (dataProperty.arraySize != totalSize)
            {
                dataProperty.arraySize = totalSize;
                for (int i = 0; i < totalSize; i++)
                {
                    SerializedProperty cellProperty = dataProperty.GetArrayElementAtIndex(i);
                    if (cellProperty.managedReferenceValue == null)
                    {
                        cellProperty.managedReferenceValue = new CustomGridCell();
                    }
                }
            }

            position.y += EditorGUIUtility.singleLineHeight + 5;
            float cellWidth = 120;
            float cellHeight = EditorGUIUtility.singleLineHeight * 2 + 5;

            float startY = position.y + (rowsProperty.intValue - 1) * cellHeight; // Start from the bottom

            for (int row = 0; row < rowsProperty.intValue; row++) // Start from 0, move up
            {
                float rowY = startY - row * cellHeight; // Move upwards instead of downwards

                for (int col = 0; col < columnsProperty.intValue; col++)
                {
                    int index = row * columnsProperty.intValue + col;
                    SerializedProperty cellProperty = dataProperty.GetArrayElementAtIndex(index);
                    SerializedProperty cellTypeProperty = cellProperty.FindPropertyRelative("cellType");
                    SerializedProperty pieceTypeProperty = cellProperty.FindPropertyRelative("pieceType");

                    float cellX = position.x + col * cellWidth;

                    // Draw cell type
                    Rect cellRect = new Rect(cellX, rowY, cellWidth - 5, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(cellRect, cellTypeProperty, GUIContent.none);

                    // Draw piece type below it
                    Rect pieceRect = new Rect(cellX, rowY + EditorGUIUtility.singleLineHeight, cellWidth - 5,
                        EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(pieceRect, pieceTypeProperty, GUIContent.none);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var rowsProperty = property.FindPropertyRelative("height");
            return EditorGUIUtility.singleLineHeight * (rowsProperty.intValue * 2 + 3);
        }
    }
}