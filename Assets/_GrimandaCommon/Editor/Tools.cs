using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tools : ScriptableObject
{
    public static string fileName = "BM2000 ";
    public static int startNumber = 1;


    private static Vector3 pos;
    private static Quaternion rotation;
    private static Vector3 localScale;

    private static bool includeRotation = false;
    private static bool includeScale = false;

    [MenuItem("Grimanda/Transform/Copy Position %&c")]
    private static void CopyPosition()
    {
        pos = ((GameObject)Selection.activeObject).transform.position;
        includeRotation = false;
        includeScale = false;
    }

    [MenuItem("Grimanda/Transform/Copy Transform %&a")]
    private static void CopyTransform()
    {
        GameObject selectedObject = ((GameObject)Selection.activeObject);
        pos = selectedObject.transform.position;
        rotation = selectedObject.transform.rotation;
        localScale = selectedObject.transform.localScale;
        includeRotation = true;
        includeScale = true;
    }

    [MenuItem("Grimanda/Transform/Paste Position %&v")]
    public static void PastePosition()
    {
        Transform transform = ((GameObject)Selection.activeObject).transform;
        if (!includeRotation && !includeScale)
        {
            Undo.RecordObject(transform, "Paste position");
            transform.position = pos;
        }
        else
        {
            Undo.RecordObject(transform, "Paste Transform");
            transform.position = pos;
            transform.rotation = rotation;
            transform.localScale = localScale;
        }

    }

    [MenuItem("CONTEXT/Transform/Copy Rotation")]
    public static void CopyRotation(MenuCommand command)
    {
        rotation = (command.context as Transform).rotation;
    }

    [MenuItem("CONTEXT/Transform/Paste Rotation")]
    public static void PasteRotation(MenuCommand command)
    {
        Transform transform = (command.context as Transform);
        Undo.RecordObject(transform, "Paste rotation");
        transform.rotation = rotation;
    }

    [MenuItem("CONTEXT/Transform/Copy Scale")]
    public static void CopyScale(MenuCommand command)
    {
        localScale = (command.context as Transform).localScale;
    }

    [MenuItem("CONTEXT/Transform/Paste Scale")]
    public static void PasteScale(MenuCommand command)
    {
        Transform transform = (command.context as Transform);
        Undo.RecordObject(transform, "Paste scale");
        transform.localScale = localScale;
    }

    //static AnchorToolsEditor()
    //{
    //  SceneView.onSceneGUIDelegate += OnScene;
    //}

    //private static void OnScene(SceneView sceneview)
    //{
    //  if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.U)
    //  {
    //      UpdateAnchors();
    //  }
    //}

    //public void OnDestroy()
    //{
    //  SceneView.onSceneGUIDelegate -= OnScene;
    //}

    static public Rect anchorRect;
    static public Vector2 anchorVector;
    static private Rect anchorRectOld;
    static private Vector2 anchorVectorOld;
    static private RectTransform currentRectTransform;
    static private RectTransform parentRectTransform;
    static private Vector2 pivotOld;
    static private Vector2 offsetMinOld;
    static private Vector2 offsetMaxOld;

    [MenuItem("Grimanda/Transform/Update Anchors %&u")]
    static public void UpdateAnchors()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            currentRectTransform = Selection.gameObjects[i].GetComponent<RectTransform>();
            parentRectTransform = currentRectTransform.parent.gameObject.GetComponent<RectTransform>();

            //            currentRectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
            if (currentRectTransform != null && parentRectTransform != null && ShouldStick())
            {
                Stick();
            }
        }
    }

    static private bool ShouldStick()
    {
        return (
            currentRectTransform.offsetMin != offsetMinOld ||
            currentRectTransform.offsetMax != offsetMaxOld ||
            currentRectTransform.pivot != pivotOld ||
            anchorVector != anchorVectorOld ||
            anchorRect != anchorRectOld
            );
    }

    static private void Stick()
    {
        CalculateCurrentWH();
        CalculateCurrentXY();

        CalculateCurrentXY();
        pivotOld = currentRectTransform.pivot;
        anchorVectorOld = anchorVector;

        AnchorsToCorners();
        anchorRectOld = anchorRect;

        EditorUtility.SetDirty(currentRectTransform.gameObject);
    }

    static private void TryToGetRectTransform()
    {
        currentRectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
        parentRectTransform = currentRectTransform.parent.gameObject.GetComponent<RectTransform>();

    }

    static private void CalculateCurrentXY()
    {
        float pivotX = anchorRect.width * currentRectTransform.pivot.x;
        float pivotY = anchorRect.height * (1 - currentRectTransform.pivot.y);
        Vector2 newXY = new Vector2(currentRectTransform.anchorMin.x * parentRectTransform.rect.width + currentRectTransform.offsetMin.x + pivotX - parentRectTransform.rect.width * anchorVector.x,
                                  -(1 - currentRectTransform.anchorMax.y) * parentRectTransform.rect.height + currentRectTransform.offsetMax.y - pivotY + parentRectTransform.rect.height * (1 - anchorVector.y));
        anchorRect.x = newXY.x;
        anchorRect.y = newXY.y;
        anchorRectOld = anchorRect;
    }

    static private void CalculateCurrentWH()
    {
        anchorRect.width = currentRectTransform.rect.width;
        anchorRect.height = currentRectTransform.rect.height;
        anchorRectOld = anchorRect;
    }

    static private void AnchorsToCorners()
    {
        float pivotX = anchorRect.width * currentRectTransform.pivot.x;
        float pivotY = anchorRect.height * (1 - currentRectTransform.pivot.y);
        currentRectTransform.anchorMin = new Vector2(0f, 1f);
        currentRectTransform.anchorMax = new Vector2(0f, 1f);
        currentRectTransform.offsetMin = new Vector2(anchorRect.x / currentRectTransform.localScale.x, anchorRect.y / currentRectTransform.localScale.y - anchorRect.height);
        currentRectTransform.offsetMax = new Vector2(anchorRect.x / currentRectTransform.localScale.x + anchorRect.width, anchorRect.y / currentRectTransform.localScale.y);
        currentRectTransform.anchorMin = new Vector2(currentRectTransform.anchorMin.x + anchorVector.x + (currentRectTransform.offsetMin.x - pivotX) / parentRectTransform.rect.width * currentRectTransform.localScale.x,
                                                 currentRectTransform.anchorMin.y - (1 - anchorVector.y) + (currentRectTransform.offsetMin.y + pivotY) / parentRectTransform.rect.height * currentRectTransform.localScale.y);
        currentRectTransform.anchorMax = new Vector2(currentRectTransform.anchorMax.x + anchorVector.x + (currentRectTransform.offsetMax.x - pivotX) / parentRectTransform.rect.width * currentRectTransform.localScale.x,
                                                 currentRectTransform.anchorMax.y - (1 - anchorVector.y) + (currentRectTransform.offsetMax.y + pivotY) / parentRectTransform.rect.height * currentRectTransform.localScale.y);
        currentRectTransform.offsetMin = new Vector2((0 - currentRectTransform.pivot.x) * anchorRect.width * (1 - currentRectTransform.localScale.x), (0 - currentRectTransform.pivot.y) * anchorRect.height * (1 - currentRectTransform.localScale.y));
        currentRectTransform.offsetMax = new Vector2((1 - currentRectTransform.pivot.x) * anchorRect.width * (1 - currentRectTransform.localScale.x), (1 - currentRectTransform.pivot.y) * anchorRect.height * (1 - currentRectTransform.localScale.y));

        offsetMinOld = currentRectTransform.offsetMin;
        offsetMaxOld = currentRectTransform.offsetMax;
    }

    [MenuItem("Grimanda/Info/Polygoncount")]
    private static void GetPolygonCount()
    {
        int polygonCount=0;
        for(int i=0;i<Selection.gameObjects.Length;i++)
        {
            if (Selection.gameObjects[i].GetComponent<MeshFilter>() != null)
            {
                polygonCount += Selection.gameObjects[i].GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
            }
        }
        Debug.Log("PolygonCount : " + polygonCount);
    }

    [MenuItem("Grimanda/Prefab/ApplyToPrefabAll")]
    private static void ApplyToPrefab()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(PrefabUtility.GetCorrespondingObjectFromOriginalSource(Selection.gameObjects[i]));
            Debug.Log(path);
            PrefabUtility.ApplyPrefabInstance(Selection.gameObjects[i],InteractionMode.AutomatedAction);
            

        }
        Debug.Log("Completed");
    }

    [MenuItem("Grimanda/PlayMaınScene")]
    private static void PlayMainScene()
    {
        EditorApplication.EnterPlaymode();
    }

}