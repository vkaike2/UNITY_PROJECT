using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Kit.Editor
{
    public class ReorderableListExtend
    {
        //#region variable
        //SerializedObject serializedObject;
        //SerializedProperty property;
        //private string propertyName;

        //List<float> elementHeights;
        //ReorderableList orderList;

        //Texture2D backgroundImage;
        //#endregion

        //#region System
        //public ReorderableListExtend(SerializedObject serializedObject, string propertyName,
        //    bool dragable = true, bool displayHeader = true, bool displayAddButton = true, bool displayRemoveButton = true)
        //{
        //    this.propertyName = propertyName;
        //    this.serializedObject = serializedObject;
        //    this.property = serializedObject.FindProperty(this.propertyName);
        //    elementHeights = new List<float>(property.arraySize);
        //    SetHightLightBackgroundImage();

        //    orderList = new ReorderableList(serializedObject, property, dragable, displayHeader, displayAddButton, displayRemoveButton);
        //    orderList.onAddCallback += OnAdd;
        //    orderList.onSelectCallback += OnSelect;
        //    orderList.onRemoveCallback += OnRemove;
        //    orderList.drawHeaderCallback += OnDrawHeader;
        //    orderList.drawElementCallback += OnDrawElement;
        //    orderList.drawElementBackgroundCallback += OnDrawElementBackground;
        //    orderList.elementHeightCallback += OnCalculateItemHeight;
        //}

        //~ReorderableListExtend()
        //{
        //    orderList.onAddCallback -= OnAdd;
        //    orderList.onSelectCallback -= OnSelect;
        //    orderList.onRemoveCallback -= OnRemove;
        //    orderList.drawHeaderCallback -= OnDrawHeader;
        //    orderList.drawElementCallback -= OnDrawElement;
        //    orderList.drawElementBackgroundCallback -= OnDrawElementBackground;
        //    orderList.elementHeightCallback -= OnCalculateItemHeight;
        //    backgroundImage = null;
        //}
        //#endregion

        //#region API
        //public virtual void SetHightLightBackgroundImage()
        //{
        //    backgroundImage = new Texture2D(3, 1);
        //    backgroundImage.SetPixel(0, 0, new Color(0f, .8f, .7f));
        //    backgroundImage.hideFlags = HideFlags.DontSave;
        //    backgroundImage.wrapMode = TextureWrapMode.Clamp;
        //    backgroundImage.Apply();
        //}

        //public void DoLayoutList()
        //{
        //    orderList.DoLayoutList();
        //}

        //public void DoList(Rect rect)
        //{
        //    orderList.DoList(rect);
        //}
        //#endregion

        //#region listener
        //protected virtual void OnDrawHeader(Rect rect)
        //{
        //    EditorGUI.LabelField(rect, property.displayName);
        //}

        //private void OnAdd(ReorderableList list)
        //{
        //    int index = list.serializedProperty.arraySize;
        //    list.serializedProperty.arraySize++;
        //    list.index = index;
        //    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        //    OnAdd(list, element);
        //}

        //private void OnRemove(ReorderableList list)
        //{
        //    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
        //    OnRemove(list, element);
        //}

        //private void OnSelect(ReorderableList list)
        //{
        //    SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
        //    OnSelect(list, element);
        //}

        //private void OnDrawElement(Rect rect, int index, bool active, bool focused)
        //{
        //    if (property == null || property.arraySize <= index)
        //        return;

        //    SerializedProperty element = property.GetArrayElementAtIndex(index);

        //    float height = EditorGUI.GetPropertyHeight(element) + EditorGUIUtility.standardVerticalSpacing;
        //    RenewElementHeight(index, height);
        //    rect.height = height;
        //    rect.width -= 40;
        //    rect.x += 20;

        //    OnDrawElement(rect, index, active, focused, element);
        //}
        //private void OnDrawElementBackground(Rect rect, int index, bool active, bool focused)
        //{
        //    if (property == null || property.arraySize <= index || index < 0)
        //        return;

        //    SerializedProperty element = property.GetArrayElementAtIndex(index);
        //    float height = elementHeights[index];
        //    rect.height = height;
        //    rect.width -= 4;
        //    rect.x += 2;

        //    OnDrawElementBackground(rect, index, active, focused, element, height);
        //}
        //#endregion

        //#region Template
        //protected virtual void OnAdd(ReorderableList list, SerializedProperty newElement) { }
        //protected virtual void OnSelect(ReorderableList list, SerializedProperty selectedElement) { }
        //protected virtual void OnRemove(ReorderableList list, SerializedProperty deleteElement)
        //{
        //    if (EditorUtility.DisplayDialog(
        //        "Warning !",
        //        "Are you sure you want to delete:\n\r[ " + deleteElement.displayName + " ] ?",
        //        "Yes", "No"))
        //    {
        //        ReorderableList.defaultBehaviours.DoRemoveButton(list);
        //    }
        //}
        //protected virtual void OnDrawElement(Rect rect, int index, bool active, bool focused, SerializedProperty element)
        //{
        //    EditorGUI.PropertyField(rect, element, true);
        //}
        //protected virtual void OnDrawElementBackground(Rect rect, int index, bool active, bool focused, SerializedProperty element, float height)
        //{
        //    if (active)
        //        EditorGUI.DrawTextureTransparent(rect, backgroundImage, ScaleMode.ScaleAndCrop);
        //}
        //#endregion

        //#region height hotfix
        //private void RenewElementHeight(int index, float height)
        //{
        //    try
        //    {
        //        elementHeights[index] = height;
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        ElementListOverflowFix();
        //    }
        //}
        //private float OnCalculateItemHeight(int index)
        //{
        //    float height = 0f;
        //    try
        //    {
        //        if (height != elementHeights[index])
        //        {
        //            height = elementHeights[index];
        //            EditorUtility.SetDirty(serializedObject.targetObject);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {
        //        ElementListOverflowFix();
        //    }
        //    return height;
        //}
        //private void ElementListOverflowFix()
        //{
        //    if (property.arraySize != elementHeights.Count)
        //    {
        //        float[] floats = elementHeights.ToArray();
        //        Array.Resize(ref floats, property.arraySize);
        //        elementHeights = floats.ToList();
        //        EditorUtility.SetDirty(serializedObject.targetObject);
        //    }
        //}
        //#endregion
    }
}