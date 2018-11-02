using System.Xml.Linq;
using UnityEditor;
using UnityEngine;



namespace Assets.Scripts
{
    /// <summary>
    /// 创建Xml
    /// 用LinQ
    /// </summary>
    public class ChinarCreateXml : MonoBehaviour
    {
        void Start()
        {
            //XmlDocument xmlDoc=new XmlDocument();//创建Xml对象
            //XmlElement root = xmlDoc.CreateElement("Root");
        }


        int index; //文件生成标志


        private void OnGUI()
        {
            if (!GUI.Button(new Rect(Screen.width / 2 - 600 / 2, Screen.height / 2 - 600 / 2, 600, 300), "创建Xml文件")) return;

            PlayerPrefs.SetInt("index", index++);
            XElement xmlDoc = new XElement("Root");
            for (int i = 1; i < 5; i++)
            {
                //新增元素：书籍
                XElement carElement = new XElement("Books",
                    new XAttribute("Id", i),
                    new XAttribute("Width", 20),
                    new XAttribute("Height", 40),
                    new XAttribute("IsSelected", 4),
                    new XAttribute("Type", "科幻"      + i),
                    new XAttribute("GroupName", "叙事" + i),
                    new XElement("子元素", 666),
                    new XElement("位置", i + "楼"),
                    new XElement("价格", i * 22),
                    new XElement("页数", i * 50)
                );
                xmlDoc.Add(carElement);
            }

            xmlDoc.Save(Application.dataPath + "/Resources/Files/NewChinarXml" + PlayerPrefs.GetInt("index") + ".xml");
            AssetDatabase.Refresh(); //强制刷新（编辑器类，打包时报错，删除即可）
        }
    }
}