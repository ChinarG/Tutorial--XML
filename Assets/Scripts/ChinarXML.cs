using System.Xml.Linq;
using UnityEngine;


namespace Assets.ChinarScripts
{
    /// <summary>
    /// XML 测试类 LInQ
    /// </summary>
    public class ChinarXML : MonoBehaviour
    {
        public static string   filePath = @"D:\Chinar\TestXml.xml";
        private       XElement xmlDoc   = XElement.Load(filePath);


        void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                XElement carElement = new XElement("Car", //新增元素
                    new XAttribute("Id", 1),
                    new XAttribute("Width", 2),
                    new XAttribute("Height", 3),
                    new XAttribute("IsSelected", 4),
                    new XAttribute("CollectionName", 5),
                    new XAttribute("GroupName", 6),
                    new XElement("测试", 7),
                    new XElement("IP", 8),
                    new XElement("X", 9),
                    new XElement("Y", 10)
                );
                this.xmlDoc.Add(carElement);
            }

            this.xmlDoc.Save(filePath);
        }


        void Update()
        {
        }
    }
}