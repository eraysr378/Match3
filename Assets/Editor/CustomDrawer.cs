using GridRelated;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using Misc;

namespace Editor
{
    [CustomPropertyDrawer(typeof(CustomGrid))]
    public class CustomGridDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the label
            EditorGUI.PrefixLabel(position, label);

            // Retrieve properties
            var rowsProperty = property.FindPropertyRelative("height");
            var columnsProperty = property.FindPropertyRelative("width");
            var dataProperty = property.FindPropertyRelative("data");

            // Adjust position for fields
            position.y += EditorGUIUtility.singleLineHeight;

            // Draw row and column fields
            rowsProperty.intValue = EditorGUI.IntField(
                new Rect(position.x, position.y, position.width / 2 - 5, EditorGUIUtility.singleLineHeight),
                "Rows", rowsProperty.intValue);
            
            columnsProperty.intValue = EditorGUI.IntField(
                new Rect(position.x + position.width / 2 + 5, position.y, position.width / 2 - 5, EditorGUIUtility.singleLineHeight),
                "Columns", columnsProperty.intValue);

            // Update array size if rows/columns change
            int totalSize = Mathf.Max(1, rowsProperty.intValue * columnsProperty.intValue);
            if (dataProperty.arraySize != totalSize)
            {
                dataProperty.arraySize = totalSize;
            }

            // Prepare enum data for drawing popups
            CellType[] cellTypes = (CellType[])Enum.GetValues(typeof(CellType));
            string[] displayNames = cellTypes.Select(ct => ct.ToString()).ToArray();
            int[] values = cellTypes.Select(ct => (int)ct).ToArray();

            // Draw the grid
            position.y += EditorGUIUtility.singleLineHeight + 5;
            float cellWidth = 100;
            for (int row = rowsProperty.intValue - 1; row >= 0; row--) // Reverse row iteration
            {
                for (int col = 0; col < columnsProperty.intValue; col++)
                {
                    int index = row * columnsProperty.intValue + col;
                    SerializedProperty element = dataProperty.GetArrayElementAtIndex(index);

                    // Draw each cell as a popup for the enum
                    Rect cellRect = new Rect(position.x + col * cellWidth, position.y, cellWidth - 5, EditorGUIUtility.singleLineHeight);
                    
                    // Fix: Use intValue instead of enumValueIndex
                    int currentValue = element.intValue;
                    int selectedIndex = Array.IndexOf(values, currentValue);
                    if (selectedIndex == -1) selectedIndex = 0; // Default to first item if not found
                    
                    int newIndex = EditorGUI.Popup(cellRect, selectedIndex, displayNames);
                    element.intValue = values[newIndex]; // Assign corresponding int value
                }
                position.y += EditorGUIUtility.singleLineHeight + 5;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var rowsProperty = property.FindPropertyRelative("height");
            return EditorGUIUtility.singleLineHeight * (rowsProperty.intValue + 3);
        }
    }
}
