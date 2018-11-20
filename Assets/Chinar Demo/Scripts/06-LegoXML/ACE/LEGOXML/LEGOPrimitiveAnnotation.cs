// Decompiled with JetBrains decompiler
// Type: LEGOPrimitiveAnnotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[Serializable]
public class LEGOPrimitiveAnnotation
{
  [XmlAttribute]
  public string aliases;
  [XmlAttribute]
  public string designname;
  [XmlAttribute]
  public int maingroupid;
  [XmlIgnore]
  public bool maingroupidSpecified;
  [XmlAttribute]
  public string maingroupname;
  [XmlAttribute]
  public int platformid;
  [XmlIgnore]
  public bool platformidSpecified;
  [XmlAttribute]
  public string platformname;
  [XmlAttribute]
  public int version;
  [XmlIgnore]
  public bool versionSpecified;
}
