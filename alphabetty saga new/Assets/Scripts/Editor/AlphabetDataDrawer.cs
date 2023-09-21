using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AlphabetData))]
[CanEditMultipleObjects]
[System.Serializable]
public class AlphabetDataDrawer : Editor
{
    private ReorderableList AlphabetPlainList;
    private ReorderableList AlphabetNormalList;
    private ReorderableList AlphabetHighlightedList;
    private ReorderableList AlphabetWrongList;

    private void OnEnable()
    {
        InitializeReordableList(ref AlphabetPlainList, propertyName: "AlphabetPlain", listLabel: "Alphabet Plain");
        InitializeReordableList(ref AlphabetNormalList, propertyName: "AlphabetNormal", listLabel: "Alphabet Normal");
        InitializeReordableList(ref AlphabetHighlightedList, propertyName: "AlphabetHighlighted", listLabel: "Alphabet Highlighted");
        InitializeReordableList(ref AlphabetWrongList, propertyName: "AlphabetWrong", listLabel: "Alphabet Wrong");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        AlphabetPlainList.DoLayoutList();
        AlphabetNormalList.DoLayoutList();
        AlphabetHighlightedList.DoLayoutList();
        AlphabetWrongList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void InitializeReordableList(ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, elements: serializedObject.FindProperty(propertyName),
            draggable: true, displayHeader: true, displayAddButton: true, displayRemoveButton: true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };

        var l = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(
                position: new Rect(rect.x, rect.y, width: 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("letter"), GUIContent.none);

            EditorGUI.PropertyField(
                position: new Rect(x: rect.x + 70, rect.y, width: rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("image"), GUIContent.none);
        };
    }
}
