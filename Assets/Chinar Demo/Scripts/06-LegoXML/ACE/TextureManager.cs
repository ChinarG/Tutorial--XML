// Decompiled with JetBrains decompiler
// Type: TextureManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TextureManager
{
  private static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();

  public static Texture Get(string id)
  {
    if (!TextureManager.Textures.ContainsKey(id))
    {
      if (File.Exists(TextureManager.DecalPath + id + ".png"))
      {
        Stream stream = FileManager.Open(TextureManager.DecalPath + id + ".png", false);
        byte[] numArray = new byte[stream.Length];
        stream.Read(numArray, 0, (int) stream.Length);
        stream.Close();
        Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false, true);
        tex.LoadImage(numArray);
        TextureManager.Textures[id] = (Texture) tex;
      }
      else
      {
        if (!File.Exists(TextureManager.CustomDecalPath + id + ".png"))
          return (Texture) null;
        Stream stream = FileManager.Open(TextureManager.CustomDecalPath + id + ".png", false);
        byte[] numArray = new byte[stream.Length];
        stream.Read(numArray, 0, (int) stream.Length);
        stream.Close();
        Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false, true);
        tex.LoadImage(numArray);
        TextureManager.Textures[id] = (Texture) tex;
      }
    }
    return TextureManager.Textures[id];
  }

  public static string[] GetBlockDecalList(string block)
  {
    string[] files = Directory.GetFiles(TextureManager.DecalPath, block + "*.png", SearchOption.AllDirectories);
    for (int index = 0; index < files.Length; ++index)
      files[index] = Path.GetFileNameWithoutExtension(files[index]);
    return files;
  }

  public static string DecalPath
  {
    get
    {
      return Application.dataPath + "\\StreamingAssets\\Decals\\";
    }
  }

  public static string CustomDecalPath
  {
    get
    {
      return Application.dataPath + "\\StreamingAssets\\Decals\\Custom\\";
    }
  }
}
