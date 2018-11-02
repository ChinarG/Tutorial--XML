using System.IO;
using System.Xml;
using UnityEngine;



namespace Assets.Scripts
{
    /// <summary>
    /// 读取目录中的所有Xml文件
    /// 第一种：用XPath语法
    /// </summary>
    public class ChinarFecthAllXml : MonoBehaviour
    {
        void Start()
        {
            string[] filePaths = Directory.GetFiles(Application.dataPath + "/Resources/Files/", "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var filepath in filePaths)
            {
                XmlDocument doc = new XmlDocument();                     //实例化一个XmlDocument类对象
                doc.Load(filepath);                                      //传入路径，读取Xml文档
                XmlNodeList list = doc.SelectNodes("/Root/People/Name"); //“/根节点Root/第一节点People/第二节点Name”，会找到所有 Name节点中的名称
                foreach (XmlElement ele in list)                         //遍历找到的所有 Name 集合中的元素
                {
                    print(ele.InnerText); //打印元素文本
                }
            }
        }
    }
}