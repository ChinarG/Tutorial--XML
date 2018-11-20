// Decompiled with JetBrains decompiler
// Type: LEGOPrimitiveConnectivityFixed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;



/// <summary>
/// 乐高基础连通性字段
/// </summary>
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[Serializable]
public class LEGOPrimitiveConnectivityFixed
{
  [XmlAttribute]
  public int type;//类型
  [XmlAttribute]
  public byte axes;//轴心
  [XmlAttribute]
  public string tag;//标签
  [XmlAttribute]
  public float angle;//角度
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
}
