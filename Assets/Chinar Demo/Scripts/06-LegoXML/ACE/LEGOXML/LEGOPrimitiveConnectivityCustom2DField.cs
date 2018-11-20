// Decompiled with JetBrains decompiler
// Type: LEGOPrimitiveConnectivityCustom2DField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

/// <summary>
/// 乐高基础连通性自定义2D字段
/// </summary>
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[Serializable]
public class LEGOPrimitiveConnectivityCustom2DField
{
    [XmlAttribute]
    public byte type;
    [XmlAttribute]
    public byte width;
    [XmlAttribute]
    public byte height;
    [XmlAttribute]
    public float angle;
    [XmlAttribute]
    public float ax;
    [XmlAttribute]
    public float ay;
    [XmlAttribute]
    public float az;
    [XmlAttribute]
    public float tx;
    [XmlAttribute]
    public float ty;
    [XmlAttribute]
    public float tz;
    [XmlText]
    public string Value;//节点值
    private List<FieldValue> nodes;//节点组
    private bool haveHole;
    public bool HaveHole //是否已存在洞
    {
        get
        {
            return this.haveHole;
        }
    }

    /// <summary>
    /// 移动、安置节点
    /// </summary>
    public void PutNodes()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (FieldValue node in this.nodes)
        {
            stringBuilder.Append(",");
            stringBuilder.Append(node.V1);
        }
        this.Value = stringBuilder.Remove(0, 1).ToString();
    }


    /// <summary>
    /// 获取节点
    /// </summary>
    /// <returns>节点组</returns>
    public List<FieldValue> GetNodes()
    {
        if (this.nodes == null)
        {
            string[] strArray1 = this.Value.Replace(" ", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty).Split(',');
            this.nodes = new List<FieldValue>();
            foreach (string str in strArray1)
            {
                char[] chArray = new char[1] { ':' };
                string[] strArray2 = str.Split(chArray);
                FieldValue fieldValue = new FieldValue();
                fieldValue.V1 = int.Parse(strArray2[0]);
                if (strArray2.Length > 1)
                    fieldValue.V2 = int.Parse(strArray2[1]);
                if (strArray2.Length > 2)
                    fieldValue.V3 = int.Parse(strArray2[2]);
                if (fieldValue.V1 == 29)
                {
                    int num1 = this.nodes.Count % ((int)this.width + 1);
                    int num2 = this.nodes.Count / ((int)this.width + 1);
                    if (num1 != 0 && num2 != 0 && (num1 != (int)this.width && num2 != (int)this.height))
                        this.haveHole = true;
                }
                this.nodes.Add(fieldValue);
            }
        }
        return this.nodes;
    }
}
