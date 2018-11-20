// Decompiled with JetBrains decompiler
// Type: LEGOPrimitiveCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\ACE Decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;



/// <summary>
/// 乐高 —— 基础碰撞
/// </summary>
[GeneratedCode("xsd", "4.6.1055.0")] //自动通过生成代码
[DebuggerStepThrough]                //调试器跳出
[DesignerCategory("code")]           //设计种类 Code
[XmlType(AnonymousType = true)]      //XSD匿名类型
[Serializable]                       //序列化
public class LEGOPrimitiveCollision
{
    [XmlElement("Box", typeof(LEGOPrimitiveCollisionBox))]       //基础盒
    [XmlElement("Sphere", typeof(LEGOPrimitiveCollisionSphere))] //基础圆
    [XmlElement("Tube", typeof(LEGOPrimitiveCollisionTube))]     //基础管
    public object[] Items;                                       //？未知项目
}
