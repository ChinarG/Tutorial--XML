using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;



namespace Assets.Scripts
{
    /// <summary>
    /// 读取Xml文件
    /// 第二种：用LinQ
    /// </summary>
    public class ChinarLoadXmlLinQ : MonoBehaviour
    {
        void Start()
        {
            string                filePath = Application.dataPath + "/Resources/Files/TestXml.xml"; //获取文件路径
            XElement              xmlDoc   = XElement.Load(filePath);                               //读取文件
            IEnumerable<XElement> elements = xmlDoc.Elements("People");                             //获取 People 集合
            IEnumerable<XElement> peoples  = from p in elements select p;                           //获取集合中的数据，LinQ简化代码
            foreach (var people in peoples)                                                         //遍历返回的 peoples 节点集合
            {
                print(people.Element("Age").Value); //people节点下的 "Age" 节点的值
                //print(people.FirstAttribute);//第一个属性： ID="1"
                //print(people.FirstNode);//第一个节点： <Name>我的小鱼你醒了</Name>
                //print(people.Value);//值：我的小鱼你醒了11     注意此处是  Name 和 Age 都被打印出来
                //print(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(people.Value)));//如果xml文件是 ANSI 格式保存的，请转UTF-8
                //具体值，请自己测试；有很多内部方法
            }
        }
    }
}