// Decompiled with JetBrains decompiler
// Type: Global
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using UnityEngine;

/// <summary>
/// 全局变量，公共函数
/// </summary>
public class Global
{
  public static int BlockCount = 0;
  public static string CurrentColor = "2";
  public static string CurrentPaintColor = "2";
  public static string SaveFilePath = string.Empty;
  public static float MouseScrollWheelSpeed = 3f;
  public static float MouseTranslationSpeed = 3f;
  public static bool OpenOrSaveFile = false;
  public static bool ShowWelcome = true;
  public static bool ShowOutline = false;
  public static bool ShowCollider = false;
  public static bool ShowConnectiveData = false;
  public static float HalfUnitSize = 0.2f;
  public static float UnitSize = 0.4f;
  public static float FullUnitSize = 0.8f;
  public static bool ContestMode = false;
  public static bool DebugMode = false;
  public static bool GuideMode = false;
  public static bool EditMode = false;
  public static bool TouchMode = false;
  public static bool HoldMode = false;
  public static bool RenderMode = false;
  public static bool NoteMode = false;
  public static bool FreeCamera = false;
  public static bool NeedRecoverScene = false;
  public static bool NeedRecoverGuide = false;
  public static bool ShowAbout = false;
  public static string P1 = "wange";
  public static string P2 = "drluck";
  public static Texture2D CursorTexture = (Texture2D) null;
  public static Vector2 CursorHotspot = new Vector2();
  public static bool HoldCursor = false;

  public static void SetViewSize(float top, float bottom, float left, float right)
  {
    Camera.main.rect = new Rect(left / (float) Screen.width, bottom / (float) Screen.height, ((float) Screen.width - left - right) / (float) Screen.width, ((float) Screen.height - top - bottom) / (float) Screen.height);
  }

  public static int Fit(int stud, int hole)
  {
    if (stud == 29 || hole == 29)
      return 0;
    if ((stud == 0 || stud == 1 || (stud == 2 || stud == 3)) && (hole == 5 || hole == 7 || (hole == 15 || hole == 16) || hole == 17) || ((stud == 1 || stud == 3) && hole == 9 || stud == 18 && hole == 5))
      return 1;
    return stud == 18 || stud == 23 ? 0 : -1;
  }

  public static string ComputeKey(float x, float y, float z)
  {
    return (Mathf.RoundToInt(x / 0.4f) + 10000).ToString("D5") + (Mathf.RoundToInt(y / 0.32f) + 10000).ToString("D5") + (Mathf.RoundToInt(z / 0.4f) + 10000).ToString("D5");
  }

  public static string ComputeKey(Vector3 pos)
  {
    return Global.ComputeKey(pos.x, pos.y, pos.z);
  }

  public static string PW
  {
    get
    {
      return Global.P1 + "&" + Global.P2;
    }
  }

  public static long DateTimeToUnixTimeStamp(DateTime dateTime)
  {
    dateTime = dateTime.ToUniversalTime();
    DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    return Convert.ToInt64((dateTime - dateTime1).TotalSeconds);
  }

  public static DateTime UnixTimeStampToDateTime(long timestamp)
  {
    return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds((double) timestamp);
  }

  public static void SetCursor(Texture2D tex, Vector2 hotspot, bool hold = false)
  {
    Cursor.SetCursor(tex, hotspot, CursorMode.ForceSoftware);
    Global.HoldCursor = hold;
    if (hold)
      return;
    Global.CursorTexture = tex;
    Global.CursorHotspot = hotspot;
  }
}
