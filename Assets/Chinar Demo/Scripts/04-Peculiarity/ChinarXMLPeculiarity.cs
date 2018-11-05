using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


/// <summary>
/// <para>作用：测试XML特性的用法</para>
/// <para>作者：Chinar</para>
/// <para>创建日期：2018-11-05</para>
/// </summary>
public class ChinarXMLPeculiarity : MonoBehaviour
{
    /// <summary>
    /// 初始化函数
    /// </summary>
    void Start()
    {
        ChinarXmlData xml = new ChinarXmlData
        {
            Id         = "这是ID-123456",
            Level      = 88,
            Qian       = 666,
            TestIgnore = "忽略重新赋值",
            Second     = 60
        };
        Person person1 = new Person("人类1");
        Person person2 = new Person("人类2");
        xml.Persons    = new[] {person1, person2};            //数组中添加信息
        xml.PersonList = new List<Person> {person1, person2}; //列表添加
        ChinarBox    lb1 = new ChinarBox(1, 2, 3);
        ChinarSphere lb2 = new ChinarSphere(4, 5, 6);
        ChinarTube   lb3 = new ChinarTube(7, 8, 9);
        xml.Items = new[] {(object) lb1, lb2, lb3}; //数组添加

        //生成XML
        CreateXml(Application.dataPath + @"\Chinar Demo\Resources\Files\Serializefield.txt", xml, typeof(ChinarXmlData), "root");

        //读取
        var v = LoadXml<ChinarXmlData>(Application.dataPath + @"\Chinar Demo\Resources\Files\Serializefield.txt");
        print(v); //测试输出
        print(v.PersonList[0]);
        print(v.PersonList[1]);
        print(v.Qian);
    }


    /// <summary>
    /// 创建XML
    /// </summary>
    /// <param name="savePath">保存路径</param>
    /// <param name="targetObjcet">要储存的对象</param>
    /// <param name="targetType">类型</param>
    /// <param name="xmlRootName">根节点名称</param>
    public void CreateXml(string savePath, object targetObjcet, Type targetType, string xmlRootName)
    {
        if (!string.IsNullOrEmpty(savePath) && targetObjcet != null)
        {
            targetType = targetType ?? targetObjcet.GetType();
            using (StreamWriter writer = new StreamWriter(savePath))
            {
                System.Xml.Serialization.XmlSerializer xmlSerializer = string.IsNullOrEmpty(xmlRootName) ? new System.Xml.Serialization.XmlSerializer(targetType) : new System.Xml.Serialization.XmlSerializer(targetType, new XmlRootAttribute(xmlRootName));
                xmlSerializer.Serialize(writer, targetObjcet);
            }
        }
    }


    /// <summary>
    /// 加载XML
    /// </summary>
    /// <param name="loadPath">加载路径</param>
    /// <param name="TChinar">泛型</param>
    /// <returns>Object</returns>
    public TChinar LoadXml<TChinar>(string loadPath)
    {
        object result = null;
        if (!File.Exists(loadPath)) return default(TChinar);
        using (StreamReader reader = new StreamReader(loadPath))
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(TChinar), new XmlRootAttribute("root"));
            result = xmlSerializer.Deserialize(reader);
        }

        return (TChinar) result;
    }
}


/// <summary>
/// 数据1 —— 测试XML的类
/// </summary>
[SerializeField] //必须设置这个，要不然不能进行序列化
public class ChinarXmlData
{
    public string Id;

    //XML中设置为当前类的属性
    [XmlAttribute] public int Level;

    [XmlElement("Money", IsNullable = false)]
    public float Qian;

    //序列化中忽略掉
    [XmlIgnore] public string TestIgnore = "忽略";

    //get set设置的方式也可以进行序列化，但是私有不会被生成出去
    private int second = 10;
    public int Second
    {
        get { return second; }
        set { second = value; }
    }

    //可以设置数组结点和子结点的名字，如果不设置则为变量名和类型名字
    [XmlArray("Items"), XmlArrayItem("item")]
    public Person[] Persons;


    //支持泛型
    public List<Person> PersonList;

    /// <summary>
    /// 依照顺序，指出XML元素内容节点
    /// </summary>
    [XmlElement("Box", typeof(ChinarBox))] [XmlElement("Sphere", typeof(ChinarSphere))] [XmlElement("Tube", typeof(ChinarTube))]
    //基础管
    public object[] Items;
}


/// <summary>
/// 数据2 —— 人类
/// </summary>
[Serializable]
public class Person
{
    //XML创建时，生成当前类的属性 Name
    [XmlAttribute] public string Name;


    public Person()
    {
    }


    /// <summary>
    /// 构造函数
    /// </summary>
    public Person(string parameter)
    {
        Name = parameter;
    }
}


/// <summary>
/// 数据3 —— 基础盒
/// </summary>
[Serializable]
public class ChinarBox
{
    /// <summary>
    /// 属性
    /// </summary>
    [XmlAttribute] public float BoxX;
    [XmlAttribute] public float BoxY;
    [XmlAttribute] public float BoxZ;


    public ChinarBox()
    {
    }


    public ChinarBox(float boxX, float boxY, float boxZ)
    {
        BoxX = boxX;
        BoxY = boxY;
        BoxZ = boxZ;
    }
}


/// <summary>
/// 数据4 —— 基础圆
/// </summary>
[Serializable]
public class ChinarSphere
{
    [XmlAttribute] public float SphereX;
    [XmlAttribute] public float SphereY;
    [XmlAttribute] public float SphereZ;


    public ChinarSphere()
    {
    }


    public ChinarSphere(float sphereX, float sphereY, float sphereZ)
    {
        SphereX = sphereX;
        SphereY = sphereY;
        SphereZ = sphereZ;
    }
}


/// <summary>
/// 数据5 —— 基础管
/// </summary>
[Serializable]
public class ChinarTube
{
    [XmlAttribute] public float TubeX;
    [XmlAttribute] public float TubeY;
    [XmlAttribute] public float TubeZ;


    public ChinarTube()
    {
    }


    public ChinarTube(float tubeX, float tubeY, float tubeZ)
    {
        TubeX = tubeX;
        TubeY = tubeY;
        TubeZ = tubeZ;
    }
}