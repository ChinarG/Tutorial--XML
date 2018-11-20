// Decompiled with JetBrains decompiler
// Type: Block
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\ACE Decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// ？颗粒块数据信息
/// </summary>
public class Block
{
    public Vector3 MeshPosition = new Vector3();//网格位置
    public Vector3 MeshRotation = new Vector3();//网格旋转
    public Vector3 MeshScale = new Vector3(1f, 1f, 1f);//比例
    public Vector3 ParentPosition = new Vector3();//父级位置
    public Vector3 ParentRotation = new Vector3();//父级旋转
    public Vector3 ParentScale = new Vector3(1f, 1f, 1f);//父级比例：one
    [XmlIgnore]
    [NonSerialized]
    public Vector3 DecorPosition = new Vector3();//布置位置
    [XmlIgnore]
    [NonSerialized]
    public Vector3 DecorRotation = new Vector3();//布置旋转
    [XmlIgnore]
    [NonSerialized]
    public Vector3 DecorScale = new Vector3(1f, 1f, 1f);//one比
    [XmlIgnore]
    [NonSerialized]
    public Vector3 DetailPosition = new Vector3();//细节位置
    [XmlIgnore]
    [NonSerialized]
    public Vector3 DetailRotation = new Vector3();//细节旋转
    [XmlIgnore]
    [NonSerialized]
    public Vector3 DetailScale = new Vector3(1f, 1f, 1f);//细节比例
    public int Version;//版本
    public string ID;//ID
    public string Name;//名称
    public string Category;//种类
    public string[] Tags;//标签集
    public byte[] ColorTable;//颜色表
    public string LegoID;//乐高ID
    public string LdrawID;//乐高原ID
    [XmlIgnore]
    [NonSerialized]
    public LEGOPrimitiveCollision Collision;//碰撞组数据
    [XmlIgnore]
    [NonSerialized]
    public LEGOPrimitiveConnectivity Connectivity;//连通性组数据
    [XmlIgnore]
    [NonSerialized]
    public LEGOPrimitivePhysicsAttributes Physics;//物理属性
    [XmlIgnore]
    [NonSerialized]
    public LEGOPrimitiveBounding Bounding;//边界框
    [XmlIgnore]
    [NonSerialized]
    public LEGOPrimitiveGeometryBounding GeometryBounding;//几何边界
    private Vector3[] boundingPoints;//边界点
    [XmlIgnore]
    [NonSerialized]
    public Mesh Mesh;//蒙皮网格
    [XmlIgnore]
    [NonSerialized]
    public Mesh BoundingMesh;//边界网格
    [XmlIgnore]
    [NonSerialized]
    public Mesh EdgeMesh;//边缘网格
    [XmlIgnore]
    [NonSerialized]
    public Mesh DecorMesh;//装饰
    [XmlIgnore]
    [NonSerialized]
    public Mesh DecorMeshFlip;//轻击装饰网格
    [XmlIgnore]
    [NonSerialized]
    public Mesh DetailMesh;//细节网格


    /// <summary>
    /// 8个顶点
    /// </summary>
    public Vector3[] BoundingPoints
    {
        get
        {
            if (this.boundingPoints == null)
            {
                this.boundingPoints = new Vector3[8];
                this.boundingPoints[0] = new Vector3(this.GeometryBounding.AABB.minX, this.GeometryBounding.AABB.minY, this.GeometryBounding.AABB.minZ);
                this.boundingPoints[1] = new Vector3(this.GeometryBounding.AABB.minX, this.GeometryBounding.AABB.minY, this.GeometryBounding.AABB.maxZ);
                this.boundingPoints[2] = new Vector3(this.GeometryBounding.AABB.minX, this.GeometryBounding.AABB.maxY, this.GeometryBounding.AABB.minZ);
                this.boundingPoints[3] = new Vector3(this.GeometryBounding.AABB.minX, this.GeometryBounding.AABB.maxY, this.GeometryBounding.AABB.maxZ);
                this.boundingPoints[4] = new Vector3(this.GeometryBounding.AABB.maxX, this.GeometryBounding.AABB.minY, this.GeometryBounding.AABB.minZ);
                this.boundingPoints[5] = new Vector3(this.GeometryBounding.AABB.maxX, this.GeometryBounding.AABB.minY, this.GeometryBounding.AABB.maxZ);
                this.boundingPoints[6] = new Vector3(this.GeometryBounding.AABB.maxX, this.GeometryBounding.AABB.maxY, this.GeometryBounding.AABB.minZ);
                this.boundingPoints[7] = new Vector3(this.GeometryBounding.AABB.maxX, this.GeometryBounding.AABB.maxY, this.GeometryBounding.AABB.maxZ);
            }
            return this.boundingPoints;
        }
    }

    /// <summary>
    /// 镜面取反
    /// </summary>
    internal void Mirror()
    {
        float num = -this.GeometryBounding.AABB.maxZ;//取Z值最大值，取反，临时变量
        this.GeometryBounding.AABB.maxZ = -this.GeometryBounding.AABB.minZ;//最大Z变最小Z
        this.GeometryBounding.AABB.minZ = num;//最小Z取 临时

        //遍历碰撞数组中，储存的所有碰撞类型
        foreach (object obj in this.Collision.Items)
        {
            
            if (obj is LEGOPrimitiveCollisionBox)//碰撞盒
            {
                //强转Object，将数据取反
                LEGOPrimitiveCollisionBox primitiveCollisionBox = obj as LEGOPrimitiveCollisionBox;
                primitiveCollisionBox.ax = -primitiveCollisionBox.ax;
                primitiveCollisionBox.ay = -primitiveCollisionBox.ay;
                primitiveCollisionBox.tz = -primitiveCollisionBox.tz;
            }
            if (obj is LEGOPrimitiveCollisionSphere)//碰撞圆
            {
                LEGOPrimitiveCollisionSphere primitiveCollisionSphere = obj as LEGOPrimitiveCollisionSphere;
                primitiveCollisionSphere.ax = -primitiveCollisionSphere.ax;
                primitiveCollisionSphere.ay = -primitiveCollisionSphere.ay;
                primitiveCollisionSphere.tz = -primitiveCollisionSphere.tz;
            }
        }

        //遍历联通组所有项目
        foreach (object obj in this.Connectivity.Items)
        {
            if (obj is LEGOPrimitiveConnectivityCustom2DField)
            {
                LEGOPrimitiveConnectivityCustom2DField connectivityCustom2Dfield = obj as LEGOPrimitiveConnectivityCustom2DField;
                connectivityCustom2Dfield.ax = -connectivityCustom2Dfield.ax;
                connectivityCustom2Dfield.ay = -connectivityCustom2Dfield.ay;
                connectivityCustom2Dfield.tz = -connectivityCustom2Dfield.tz;
            }
            if (obj is LEGOPrimitiveConnectivityAxel)
            {
                LEGOPrimitiveConnectivityAxel connectivityAxel = obj as LEGOPrimitiveConnectivityAxel;
                connectivityAxel.ax = -connectivityAxel.ax;
                connectivityAxel.ay = -connectivityAxel.ay;
                connectivityAxel.tz = -connectivityAxel.tz;
            }
            if (obj is LEGOPrimitiveConnectivityFixed)
            {
                LEGOPrimitiveConnectivityFixed connectivityFixed = obj as LEGOPrimitiveConnectivityFixed;
                connectivityFixed.ax = -connectivityFixed.ax;
                connectivityFixed.ay = -connectivityFixed.ay;
                connectivityFixed.tz = -connectivityFixed.tz;
            }
            if (obj is LEGOPrimitiveConnectivityHinge)
            {
                LEGOPrimitiveConnectivityHinge connectivityHinge = obj as LEGOPrimitiveConnectivityHinge;
                connectivityHinge.ax = -connectivityHinge.ax;
                connectivityHinge.ay = -connectivityHinge.ay;
                connectivityHinge.tz = -connectivityHinge.tz;
            }
            if (obj is LEGOPrimitiveConnectivitySlider)
            {
                LEGOPrimitiveConnectivitySlider connectivitySlider = obj as LEGOPrimitiveConnectivitySlider;
                connectivitySlider.ax = -connectivitySlider.ax;
                connectivitySlider.ay = -connectivitySlider.ay;
                connectivitySlider.tz = -connectivitySlider.tz;
            }
            if (obj is LEGOPrimitiveConnectivityBall)
            {
                LEGOPrimitiveConnectivityBall connectivityBall = obj as LEGOPrimitiveConnectivityBall;
                connectivityBall.ax = -connectivityBall.ax;
                connectivityBall.ay = -connectivityBall.ay;
                connectivityBall.tz = -connectivityBall.tz;
            }
        }
    }
}
