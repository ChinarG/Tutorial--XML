// Decompiled with JetBrains decompiler
// Type: LEGOPrimitiveGeometryBoundingAABB
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;


/// <summary>
/// 几何边界？AABB未知
/// </summary>
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[Serializable]
public class LEGOPrimitiveGeometryBoundingAABB
{
  [XmlAttribute]
  public float minX;
  [XmlAttribute]
  public float minY;
  [XmlAttribute]
  public float minZ;
  [XmlAttribute]
  public float maxX;
  [XmlAttribute]
  public float maxY;
  [XmlAttribute]
  public float maxZ;
}
