using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TakeScreenshotInEditor : ScriptableObject
{
    public static string fileName = "BM2000 ";
    public static int startNumber = 1;

    [MenuItem("Grimanda/Take Screenshot of Game View %^s")]

   /* void scshot(int w, int h){
        int number = startNumber;
        string name = "" + number;

        while (System.IO.File.Exists(fileName + name + ".png"))
        {
            number++;
            name = "" + number;
        }

        startNumber = number + 1;
        fileName = "BM2000 " + w + "x" + h + name;
        Screen.SetResolution(w, h, false);
        ScreenCapture.CaptureScreenshot(fileName + name + ".png");

    }*/
    static void TakeScreenshot()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        Vector2 v = (Vector2)Res;
        int w = (int)v.x;
        int h = (int)v.y;
        //scshot(1920,1080);
        int number = startNumber;
        string name = "" + number;

        while (System.IO.File.Exists(fileName + name + ".png"))
        {
            number++;
            name = "" + number;
        }

        startNumber = number + 1;
        fileName = "BM2000 " + w + "x" + h + " " +name;
        ScreenCapture.CaptureScreenshot(fileName + name + ".png");
    }
}