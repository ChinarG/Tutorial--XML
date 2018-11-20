// Decompiled with JetBrains decompiler
// Type: LEGOPrimitive
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
[XmlRoot(IsNullable = false, Namespace = "")]
[Serializable]
public class LEGOPrimitive
{
  [XmlArrayItem("Annotation", IsNullable = false)]
  public LEGOPrimitiveAnnotation[] Annotations;
  public LEGOPrimitiveCollision Collision;
  public LEGOPrimitiveConnectivity Connectivity;
  public LEGOPrimitivePhysicsAttributes PhysicsAttributes;
  [XmlArrayItem("Path", IsNullable = true)]
  public LEGOPrimitivePath[] Paths;
  public LEGOPrimitiveBounding Bounding;
  public LEGOPrimitiveGeometryBounding GeometryBounding;
  [XmlAttribute]
  public int versionMajor;
  [XmlAttribute]
  public int versionMinor;
}
