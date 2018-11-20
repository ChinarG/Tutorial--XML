// Decompiled with JetBrains decompiler
// Type: LEGOPrimitiveConnectivity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: I:\Unity Files\Git\ECA Decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;


/// <summary>
/// 乐高 —— 基础连通性
/// </summary>
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[Serializable]
public class LEGOPrimitiveConnectivity
{
    /// <summary>
    /// 等待测试
    /// </summary>
    [XmlElement("Axel", typeof(LEGOPrimitiveConnectivityAxel))]
    [XmlElement("Ball", typeof(LEGOPrimitiveConnectivityBall))]
    [XmlElement("Custom2DField", typeof(LEGOPrimitiveConnectivityCustom2DField))]
    [XmlElement("Fixed", typeof(LEGOPrimitiveConnectivityFixed))]
    [XmlElement("Gear", typeof(LEGOPrimitiveConnectivityGear))]
    [XmlElement("Hinge", typeof(LEGOPrimitiveConnectivityHinge))]
    [XmlElement("Rail", typeof(LEGOPrimitiveConnectivityRail))]
    [XmlElement("Slider", typeof(LEGOPrimitiveConnectivitySlider))]
    public object[] Items;
}
