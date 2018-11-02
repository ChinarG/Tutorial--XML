using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


/// <summary>
/// XML——Xpath
/// </summary>
public class ChinarXmlXpath : MonoBehaviour
{
    public string filePath = @"D:\a.xml"; //路径


    void Start()
    {
        SecondMethod();
    }


    /// <summary>
    /// 第二种读取方法
    /// </summary>
    void SecondMethod()
    {
        XmlDocument doc = new XmlDocument();                     //实例化一个XmlDocument类对象 :创建一个XML文档
        doc.Load(filePath);                                      //读取XML文档
        XmlNodeList list = doc.SelectNodes("/GameData/s/cubeName"); //“/根节点Root/第一节点People/第二节点Name”
        foreach (XmlElement ele in list)                         //遍历集合中的元素
        {
            print(ele.InnerText); //打印元素文本
        }

        //XmlNode root = doc.SelectSingleNode("Root");
        ////下面的东西就跟上面创建xml元素是一样的。我们把他复制过来就行了
        //XmlElement element = doc.CreateElement("messages");
        ////设置节点的属性
        //element.SetAttribute("id", "2");
        //XmlElement elementChild1 = doc.CreateElement("contents");

        //elementChild1.SetAttribute("name", "b");
        ////设置节点内面的内容
        //elementChild1.InnerText  = "天狼，你的梦想就是。。。。。";
        //XmlElement elementChild2 = doc.CreateElement("mission");
        //elementChild2.SetAttribute("map", "def");
        //elementChild2.InnerText = "我要妹子。。。。。。。。。。";
        ////把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
        //element.AppendChild(elementChild1);
        //element.AppendChild(elementChild2);

        //root.AppendChild(element);

        //doc.AppendChild(root);
        ////最后保存文件
        //doc.Save(filePath);




    }
}