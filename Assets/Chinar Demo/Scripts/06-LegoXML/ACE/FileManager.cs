// Decompiled with JetBrains decompiler
// Type: FileManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using Ionic.Zip;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// 文件管理
/// </summary>
public static class FileManager
{
    private static Dictionary<string, ZipFile> bundles;//Zip散表


    /// <summary>
    /// 加载AB包
    /// </summary>
    public static void LoadBundle()
    {
        FileManager.bundles = new Dictionary<string, ZipFile>();
        if (!File.Exists(Application.dataPath + "\\StreamingAssets\\Blocks.bundle"))
            return;
        ReadOptions readOptions = new ReadOptions();
        readOptions.Encoding=Encoding.UTF8;
        ZipFile zipFile = ZipFile.Read(Application.dataPath + "\\StreamingAssets\\Blocks.bundle", readOptions);
        zipFile.Password = Global.PW;
        var v=ZipFile.CheckZipPassword(Application.dataPath + "\\StreamingAssets\\Blocks.bundle", Global.PW);
        Debug.Log(v);
        Debug.Log("ssss");

        if (zipFile == null)
            return;
        
        FileManager.bundles["\\blocks\\"] = zipFile;
        //zipFile.Save(Application.dataPath+"/1.zip");

    }


    /// <summary>
    /// 打开贴花 —— 猜，可能为选择自定义贴纸路径，返回流
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Stream OpenDecal(string name)
    {
        return FileManager.Open(TextureManager.CustomDecalPath + name + ".png", false);
    }


    /// <summary>
    /// 打开某个路径下的文件，创建流文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="create"></param>
    /// <returns></returns>
    public static Stream Open(string path, bool create = false)
    {
        if (FileManager.bundles == null)
            FileManager.LoadBundle();
        if (create)
            return (Stream)File.Open(path, FileMode.OpenOrCreate);
        if (File.Exists(path))
            return (Stream)File.Open(path, FileMode.Open, FileAccess.Read);
        using (Dictionary<string, ZipFile>.KeyCollection.Enumerator enumerator = FileManager.bundles.Keys.GetEnumerator())
        {
            Debug.Log(path);
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                if (path.ToLower().IndexOf(current) >= 0)
                {
                    string fileName = Path.GetFileName(path);
                    if (FileManager.bundles[current].ContainsEntry(fileName))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        //FileManager.bundles[current].get_Item(fileName).Extract((Stream)memoryStream);
                        FileManager.bundles[current].AddItem(fileName).Extract((Stream)memoryStream);

                        memoryStream.Position = 0L;
                        return (Stream)memoryStream;
                    }
                    break;
                }
            }
        }
        return (Stream)null;
    }
}
