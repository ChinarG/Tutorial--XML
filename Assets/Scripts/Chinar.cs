using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


/// <summary>
/// 存放所有公用函数 —— 静态类，用于简化逻辑、代码
/// </summary>
public class Chinar
{
    /// <summary>
    /// 屏幕中心，画一个按钮
    /// </summary>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <param name="buttonName">按钮名</param>
    /// <returns></returns>
    public static bool Gui按钮(float width, float height, string buttonName)
    {
        return GUI.Button(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - height / 2, width, height), buttonName);
    }


    /// <summary>
    /// 获取目录下所有指定格式文件，并返回相应类型（返回 GameData数组，只需调用这一函数）
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="path">目录路径</param>
    /// <param name="fileSuffix">后缀名格式</param>
    /// <param name="state">搜索状态</param>
    /// <returns></returns>
    public static List<T> GetFiles<T>(string path, string fileSuffix, SearchOption state)
    {
        return Directory.GetFiles(path, fileSuffix, state).Select(LoadXml<T>).ToList(); //LinQ 简化代码
    }


    /// <summary>
    /// 创建Xml文件 
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="t">数据类型对象</param>
    public static void CreateQm<T>(string fileName, T t)
    {
        StreamWriter    writer = File.CreateText(fileName);
        RijndaelManaged rDel   = new RijndaelManaged //加密数据；加密和解密采用相同的key,具体自己填，但是必须为32位//
        {
            Key     = Encoding.UTF8.GetBytes("12348578902223367877723456789012"), //用32位输出为UTF8编码格式的key
            Mode    = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };

        //序列化数据，将类型转为数据流
        MemoryStream  memoryStream  = new MemoryStream();
        XmlSerializer xs            = new XmlSerializer(typeof(T));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, t);
        memoryStream          = (MemoryStream) xmlTextWriter.BaseStream;
        UTF8Encoding encoding = new UTF8Encoding();

        //写入加密后的信息到  Xml 文件中
        writer.Write(Convert.ToBase64String(rDel.CreateEncryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())), 0, Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())).Length), 0, rDel.CreateEncryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())), 0, Encoding.UTF8.GetBytes(encoding.GetString(memoryStream.ToArray())).Length).Length));
        writer.Close();
    }


    /// <summary>
    /// 读取Xml文件
    /// </summary>
    /// <param name="path">qm文件路径</param>
    /// <returns>GameData类型对象</returns>
    public static T LoadXml<T>(string path)
    {
        StreamReader sReader    = File.OpenText(path);
        string       dataString = sReader.ReadToEnd();
        sReader.Close();
        RijndaelManaged rDel = new RijndaelManaged //解密xml中的数据
        {
            Key     = Encoding.UTF8.GetBytes("12348578902223367877723456789012"), //加密和解密采用相同的key,具体值自己填，但是必须为32位// 
            Mode    = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        XmlSerializer xs       = new XmlSerializer(typeof(T)); //反序列化
        UTF8Encoding  encoding = new UTF8Encoding();
        //解密xml数据，并反序列化，返回对应数据类型
        MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(Encoding.UTF8.GetString(rDel.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(dataString), 0, Convert.FromBase64String(dataString).Length)))); //字符串转字节组
        return (T) xs.Deserialize(memoryStream);
    }
}