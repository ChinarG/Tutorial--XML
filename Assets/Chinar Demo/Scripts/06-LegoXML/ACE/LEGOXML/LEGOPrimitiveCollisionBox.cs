﻿// Decompiled with JetBrains decompiler
// Type: LEGOPrimitiveCollisionBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\ACE Decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/// <summary>
/// 乐高 —— 基础盒
/// </summary>
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[Serializable]
public class LEGOPrimitiveCollisionBox
{
    /// <summary>
    /// 属性
    /// </summary>
    [XmlAttribute]
    public float sX;
    [XmlAttribute]
    public float sY;
    [XmlAttribute]
    public float sZ;
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
}
