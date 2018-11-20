// Decompiled with JetBrains decompiler
// Type: BlockManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
/// <summary>
/// 积木管理器
/// </summary>
public static class BlockManager
{
    private static Dictionary<string, Block> Blocks = new Dictionary<string, Block>();//积木字典
    private static Dictionary<string, Block> MirrorBlocks = new Dictionary<string, Block>();//反积木

    /// <summary>
    /// 根据id，获取积
    /// </summary>
    /// <param name="id">ID</param>
    /// <param name="mirror">反向镜面？</param>
    /// <returns>积木对象</returns>
    public static Block Get(string id, bool mirror = false)
    {
        if (mirror)//否
        {
            if (!BlockManager.MirrorBlocks.ContainsKey(id))
            {
                string path1 = BlockManager.BlockPath + id + ".xml";
                using (Stream stream = FileManager.Open(path1, false))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Block));
                    Block block;
                    try
                    {
                        block = xmlSerializer.Deserialize(stream) as Block;
                    }
                    catch (Exception ex)
                    {
                        Debug.Log((object)("load error:" + id));
                        return (Block)null;
                    }
                    BlockManager.MirrorBlocks[id] = block;
                    stream.Close();
                    string path2 = path1.Replace(".xml", ".a");
                    string path3 = path1.Replace(".xml", ".m");
                    BlockManager.ReadBlockAssemble(path2, block);
                    block.Mirror();
                    block.ID = id;
                    BlockManager.ReadBlockMesh(path3, block);
                }
            }
            return BlockManager.MirrorBlocks[id];
        }
        if (!BlockManager.Blocks.ContainsKey(id))//如果没包含该积木
        {

            //以下为数据读取，但有些.xml文件对应的id是没有的
            //都是些.a .m文件
            string path1 = BlockManager.BlockPath + id + ".xml";
            using (Stream stream = FileManager.Open(path1, false))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Block));
                Block block;
                try
                {
                    block = xmlSerializer.Deserialize(stream) as Block;
                }
                catch (Exception ex)
                {
                    Debug.Log((object)("load error:" + id));
                    return (Block)null;
                }
                BlockManager.Blocks[id] = block;
                stream.Close();
                string path2 = path1.Replace(".xml", ".a");
                string path3 = path1.Replace(".xml", ".m");
                BlockManager.ReadBlockAssemble(path2, block);
                BlockManager.ReadBlockMesh(path3, block);
            }
        }
        return BlockManager.Blocks[id];
    }

    public static List<string> GetAllBlockID()
    {
        string[] files = Directory.GetFiles(BlockManager.BlockPath, "*.xml");
        List<string> stringList = new List<string>();
        foreach (string path in files)
            stringList.Add(Path.GetFileNameWithoutExtension(path));
        return stringList;
    }
    /// <summary>
    /// 积木路径，流原路径积木下的未知文件？
    /// </summary>
    public static string BlockPath
    {
        get
        {
            return Application.dataPath + "\\StreamingAssets\\Blocks\\";
        }
    }

    public static void ReadBlockAssemble(string path, Block block)
    {
        Stream input = FileManager.Open(path, false);
        BinaryReader reader = new BinaryReader(input);
        block.Collision = reader.ReadCollision();
        block.Connectivity = reader.ReadConnctivity();
        block.Physics = reader.ReadPhysicsAttributes();
        block.Bounding = reader.ReadBounding();
        block.GeometryBounding = reader.ReadGeometryBounding();
        reader.Close();
        input.Close();
    }

    public static void WriteBlockAssemble(string path, Block block)
    {
        Stream output = FileManager.Open(path, true);
        BinaryWriter writer = new BinaryWriter(output);
        writer.WriteCollision(block.Collision);
        writer.WriteConnectivity(block.Connectivity);
        writer.WritePhysicsAttributes(block.Physics);
        writer.WriteBounding(block.Bounding);
        writer.WriteGeometryBounding(block.GeometryBounding);
        writer.Close();
        output.Close();
    }

    public static void ReadBlockMesh(string path, Block block)
    {
        Stream input = FileManager.Open(path, false);
        BinaryReader reader = new BinaryReader(input);
        int length1 = reader.ReadInt32();
        Vector3[] vertexList = new Vector3[length1];
        Vector3[] vector3Array = new Vector3[length1];
        for (int index = 0; index < length1; ++index)
        {
            reader.ReadVector3(ref vertexList[index]);
            reader.ReadVector3(ref vector3Array[index]);
        }
        int length2 = reader.ReadInt32();
        int[] triangleList = new int[length2];
        for (int index = 0; index < length2; ++index)
            triangleList[index] = reader.ReadInt32();
        if ((UnityEngine.Object)block.Mesh == (UnityEngine.Object)null)
            block.Mesh = new Mesh();
        block.Mesh.Clear();
        block.Mesh.vertices = vertexList;
        block.Mesh.normals = vector3Array;
        block.Mesh.triangles = triangleList;
        block.Mesh.UploadMeshData(false);
        if ((UnityEngine.Object)block.EdgeMesh == (UnityEngine.Object)null && !Global.DebugMode)
        {
            string path1 = path.Replace(".m", ".e");
            if (!BlockManager.ReadBlockEdgeMesh(path1, block))
            {
                block.EdgeMesh = EdgeExtractor.Extract(vertexList, triangleList);
                BlockManager.WriteBlockEdgeMesh(path1, block);
            }
        }
        if ((UnityEngine.Object)block.BoundingMesh == (UnityEngine.Object)null)
            block.BoundingMesh = new Mesh();
        block.BoundingMesh.Clear();
        block.BoundingMesh.vertices = block.BoundingPoints;
        int[] indices = new int[24]
        {
      0,
      1,
      2,
      3,
      4,
      5,
      6,
      7,
      0,
      2,
      1,
      3,
      4,
      6,
      5,
      7,
      0,
      4,
      1,
      5,
      2,
      6,
      3,
      7
        };
        block.BoundingMesh.SetIndices(indices, MeshTopology.Lines, 0);
        block.BoundingMesh.UploadMeshData(false);
        reader.Close();
        input.Close();
        ///布置网格
        BlockManager.ReadDecorMesh(path.Replace(".m", ".d"), block);
        if (!Global.EditMode)
            return;
        ///详细
        BlockManager.ReadDetailMesh(path.Replace(".m", ".h"), block);
    }

    public static void WriteBlockMesh(string path, Block block)
    {
        Stream output = FileManager.Open(path, true);
        BinaryWriter writer = new BinaryWriter(output);
        writer.Write(block.Mesh.vertices.Length);
        for (int index = 0; index < block.Mesh.vertices.Length; ++index)
        {
            writer.WriteVector3(ref block.Mesh.vertices[index]);
            writer.WriteVector3(ref block.Mesh.normals[index]);
        }
        writer.Write(block.Mesh.triangles.Length);
        for (int index = 0; index < block.Mesh.triangles.Length; ++index)
            writer.Write(block.Mesh.triangles[index]);
        writer.Close();
        output.Close();
    }

    public static bool ReadBlockEdgeMesh(string path, Block block)
    {
        Stream input = FileManager.Open(path, false);
        if (input == null)
            return false;
        BinaryReader reader = new BinaryReader(input);
        int length1 = reader.ReadInt32();
        Vector3[] vector3Array = new Vector3[length1];
        for (int index = 0; index < length1; ++index)
            reader.ReadVector3(ref vector3Array[index]);
        int length2 = reader.ReadInt32();
        int[] indices = new int[length2];
        for (int index = 0; index < length2; ++index)
            indices[index] = reader.ReadInt32();
        if ((UnityEngine.Object)block.EdgeMesh == (UnityEngine.Object)null)
            block.EdgeMesh = new Mesh();
        block.EdgeMesh.Clear();
        block.EdgeMesh.vertices = vector3Array;
        block.EdgeMesh.SetIndices(indices, MeshTopology.Lines, 0);
        block.EdgeMesh.UploadMeshData(false);
        return true;
    }

    public static void WriteBlockEdgeMesh(string path, Block block)
    {
        Stream output = FileManager.Open(path, true);
        BinaryWriter writer = new BinaryWriter(output);
        writer.Write(block.EdgeMesh.vertices.Length);
        for (int index = 0; index < block.EdgeMesh.vertices.Length; ++index)
            writer.WriteVector3(ref block.EdgeMesh.vertices[index]);
        int[] indices = block.EdgeMesh.GetIndices(0);
        writer.Write(indices.Length);
        for (int index = 0; index < indices.Length; ++index)
            writer.Write(indices[index]);
        writer.Close();
        output.Close();
    }

    public static bool ReadDecorMesh(string path, Block block)
    {
        Stream input = FileManager.Open(path, false);
        if (input == null)
            return false;
        BinaryReader reader = new BinaryReader(input);
        int length1 = reader.ReadInt32();
        Vector3[] vector3Array1 = new Vector3[length1];
        Vector3[] vector3Array2 = new Vector3[length1];
        Vector2[] vector2Array = new Vector2[length1];
        for (int index = 0; index < length1; ++index)
        {
            reader.ReadVector3(ref vector3Array1[index]);
            reader.ReadVector3(ref vector3Array2[index]);
            reader.ReadVector2(ref vector2Array[index]);
        }
        int length2 = reader.ReadInt32();
        int[] numArray = new int[length2];
        for (int index = 0; index < length2; ++index)
            numArray[index] = reader.ReadInt32();
        if ((UnityEngine.Object)block.DecorMesh == (UnityEngine.Object)null)
            block.DecorMesh = new Mesh();
        block.DecorMesh.Clear();
        block.DecorMesh.vertices = vector3Array1;
        block.DecorMesh.normals = vector3Array2;
        block.DecorMesh.uv = vector2Array;
        block.DecorMesh.triangles = numArray;
        block.DecorMesh.UploadMeshData(true);
        if ((UnityEngine.Object)block.DecorMeshFlip == (UnityEngine.Object)null)
            block.DecorMeshFlip = new Mesh();
        for (int index = 0; index < vector2Array.Length; ++index)
            vector2Array[index].y = 1f - vector2Array[index].y;
        block.DecorMeshFlip.Clear();
        block.DecorMeshFlip.vertices = vector3Array1;
        block.DecorMeshFlip.normals = vector3Array2;
        block.DecorMeshFlip.uv = vector2Array;
        block.DecorMeshFlip.triangles = numArray;
        block.DecorMeshFlip.UploadMeshData(true);
        reader.ReadVector3(ref block.DecorPosition);
        reader.ReadVector3(ref block.DecorRotation);
        reader.ReadVector3(ref block.DecorScale);
        reader.Close();
        input.Close();
        return true;
    }

    public static void WriteDecorMesh(string path, Block block)
    {
        Stream output = FileManager.Open(path, true);
        BinaryWriter writer = new BinaryWriter(output);
        writer.Write(block.DecorMesh.vertices.Length);
        for (int index = 0; index < block.DecorMesh.vertices.Length; ++index)
        {
            writer.WriteVector3(ref block.DecorMesh.vertices[index]);
            writer.WriteVector3(ref block.DecorMesh.normals[index]);
            writer.WriteVector2(ref block.DecorMesh.uv[index]);
        }
        writer.Write(block.DecorMesh.triangles.Length);
        for (int index = 0; index < block.DecorMesh.triangles.Length; ++index)
            writer.Write(block.DecorMesh.triangles[index]);
        writer.WriteVector3(ref block.DecorPosition);
        writer.WriteVector3(ref block.DecorRotation);
        writer.WriteVector3(ref block.DecorScale);
        writer.Close();
        output.Close();
        Debug.Log((object)"Write Decor");
    }

    public static bool ReadDetailMesh(string path, Block block)
    {
        Stream input = FileManager.Open(path, false);
        if (input == null)
            return false;
        BinaryReader reader = new BinaryReader(input);
        int length1 = reader.ReadInt32();
        Vector3[] vector3Array1 = new Vector3[length1];
        Vector3[] vector3Array2 = new Vector3[length1];
        for (int index = 0; index < length1; ++index)
        {
            reader.ReadVector3(ref vector3Array1[index]);
            reader.ReadVector3(ref vector3Array2[index]);
        }
        int length2 = reader.ReadInt32();
        int[] numArray = new int[length2];
        for (int index = 0; index < length2; ++index)
            numArray[index] = reader.ReadInt32();
        if ((UnityEngine.Object)block.DetailMesh == (UnityEngine.Object)null)
            block.DetailMesh = new Mesh();
        block.DetailMesh.Clear();
        block.DetailMesh.vertices = vector3Array1;
        block.DetailMesh.normals = vector3Array2;
        block.DetailMesh.triangles = numArray;
        block.DetailMesh.UploadMeshData(true);
        reader.ReadVector3(ref block.DetailPosition);
        reader.ReadVector3(ref block.DetailRotation);
        reader.ReadVector3(ref block.DetailScale);
        reader.Close();
        input.Close();
        return true;
    }

    public static void WriteDetailMesh(string path, Block block)
    {
        Stream output = FileManager.Open(path, true);
        BinaryWriter writer = new BinaryWriter(output);
        writer.Write(block.DetailMesh.vertices.Length);
        for (int index = 0; index < block.DetailMesh.vertices.Length; ++index)
        {
            writer.WriteVector3(ref block.DetailMesh.vertices[index]);
            writer.WriteVector3(ref block.DetailMesh.normals[index]);
        }
        writer.Write(block.DetailMesh.triangles.Length);
        for (int index = 0; index < block.DetailMesh.triangles.Length; ++index)
            writer.Write(block.DetailMesh.triangles[index]);
        writer.WriteVector3(ref block.DetailPosition);
        writer.WriteVector3(ref block.DetailRotation);
        writer.WriteVector3(ref block.DetailScale);
        writer.Close();
        output.Close();
        Debug.Log((object)"Write Detail");
    }

    public static void ReadVector2(this BinaryReader reader, ref Vector2 v)
    {
        v.x = reader.ReadSingle();
        v.y = reader.ReadSingle();
    }

    public static void WriteVector2(this BinaryWriter writer, ref Vector2 v)
    {
        writer.Write(v.x);
        writer.Write(v.y);
    }

    public static void ReadVector3(this BinaryReader reader, ref Vector3 v)
    {
        v.x = reader.ReadSingle();
        v.y = reader.ReadSingle();
        v.z = reader.ReadSingle();
    }

    public static void WriteVector3(this BinaryWriter writer, ref Vector3 v)
    {
        writer.Write(v.x);
        writer.Write(v.y);
        writer.Write(v.z);
    }

    public static Mesh ReadMesh(this BinaryReader reader)
    {
        Mesh mesh = new Mesh();
        int length1 = reader.ReadInt32();
        Vector3[] vector3Array1 = new Vector3[length1];
        for (int index = 0; index < length1; ++index)
            reader.ReadVector3(ref vector3Array1[index]);
        int length2 = reader.ReadInt32();
        int[] numArray = new int[length2];
        for (int index = 0; index < length2; ++index)
            numArray[index] = reader.ReadInt32();
        Vector3[] vector3Array2 = new Vector3[length2];
        for (int index = 0; index < length2; ++index)
            reader.ReadVector3(ref vector3Array2[index]);
        mesh.Clear();
        mesh.vertices = vector3Array1;
        mesh.triangles = numArray;
        mesh.normals = vector3Array2;
        mesh.UploadMeshData(true);
        return mesh;
    }

    public static void WriteMesh(this BinaryWriter writer, Mesh mesh)
    {
        writer.Write(mesh.vertices.Length);
        for (int index = 0; index < mesh.vertices.Length; ++index)
            writer.WriteVector3(ref mesh.vertices[index]);
        writer.Write(mesh.triangles.Length);
        for (int index = 0; index < mesh.triangles.Length; ++index)
            writer.Write(mesh.triangles[index]);
        writer.Write(mesh.normals.Length);
        for (int index = 0; index < mesh.normals.Length; ++index)
            writer.WriteVector3(ref mesh.normals[index]);
    }

    public static LEGOPrimitiveCollision ReadCollision(this BinaryReader reader)
    {
        LEGOPrimitiveCollision primitiveCollision = new LEGOPrimitiveCollision();
        int length = reader.ReadInt32();
        primitiveCollision.Items = new object[length];
        for (int index = 0; index < primitiveCollision.Items.Length; ++index)
        {
            int num = reader.ReadInt32();
            if (num == 0)
                primitiveCollision.Items[index] = (object)new LEGOPrimitiveCollisionBox()
                {
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    sX = reader.ReadSingle(),
                    sY = reader.ReadSingle(),
                    sZ = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 1)
                primitiveCollision.Items[index] = (object)new LEGOPrimitiveCollisionSphere()
                {
                    radius = reader.ReadSingle(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 2)
                primitiveCollision.Items[index] = (object)new LEGOPrimitiveCollisionTube()
                {
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle(),
                    length = reader.ReadSingle(),
                    innerRadius = reader.ReadSingle(),
                    outerRadius = reader.ReadSingle()
                };
        }
        return primitiveCollision;
    }

    public static void WriteCollision(this BinaryWriter writer, LEGOPrimitiveCollision collision)
    {
        writer.Write(collision.Items.Length);
        foreach (object obj in collision.Items)
        {
            if (obj is LEGOPrimitiveCollisionBox)
            {
                writer.Write(0);
                writer.WriteCollisionBox(obj as LEGOPrimitiveCollisionBox);
            }
            if (obj is LEGOPrimitiveCollisionSphere)
            {
                writer.Write(1);
                writer.WriteCollisionSphere(obj as LEGOPrimitiveCollisionSphere);
            }
            if (obj is LEGOPrimitiveCollisionTube)
            {
                writer.Write(2);
                writer.WriteCollisionTube(obj as LEGOPrimitiveCollisionTube);
            }
        }
    }

    private static void WriteCollisionBox(this BinaryWriter writer, LEGOPrimitiveCollisionBox box)
    {
        writer.Write(box.angle);
        writer.Write(box.ax);
        writer.Write(box.ay);
        writer.Write(box.az);
        writer.Write(box.sX);
        writer.Write(box.sY);
        writer.Write(box.sZ);
        writer.Write(box.tx);
        writer.Write(box.ty);
        writer.Write(box.tz);
    }

    private static void WriteCollisionSphere(this BinaryWriter writer, LEGOPrimitiveCollisionSphere sphere)
    {
        writer.Write(sphere.radius);
        writer.Write(sphere.angle);
        writer.Write(sphere.ax);
        writer.Write(sphere.ay);
        writer.Write(sphere.az);
        writer.Write(sphere.tx);
        writer.Write(sphere.ty);
        writer.Write(sphere.tz);
    }

    private static void WriteCollisionTube(this BinaryWriter writer, LEGOPrimitiveCollisionTube tube)
    {
        writer.Write(tube.angle);
        writer.Write(tube.ax);
        writer.Write(tube.ay);
        writer.Write(tube.az);
        writer.Write(tube.tx);
        writer.Write(tube.ty);
        writer.Write(tube.tz);
        writer.Write(tube.length);
        writer.Write(tube.innerRadius);
        writer.Write(tube.outerRadius);
    }

    public static LEGOPrimitiveConnectivity ReadConnctivity(this BinaryReader reader)
    {
        LEGOPrimitiveConnectivity primitiveConnectivity = new LEGOPrimitiveConnectivity();
        int length = reader.ReadInt32();
        primitiveConnectivity.Items = new object[length];
        for (int index = 0; index < primitiveConnectivity.Items.Length; ++index)
        {
            int num = reader.ReadInt32();
            if (num == 0)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivityAxel()
                {
                    type = reader.ReadInt32(),
                    length = reader.ReadSingle(),
                    grabbing = reader.ReadByte(),
                    startCapped = reader.ReadByte(),
                    endCapped = reader.ReadByte(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 1)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivityBall()
                {
                    type = reader.ReadInt32(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 2)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivityCustom2DField()
                {
                    type = reader.ReadByte(),
                    width = reader.ReadByte(),
                    height = reader.ReadByte(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle(),
                    Value = reader.ReadString()
                };
            if (num == 3)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivityFixed()
                {
                    type = reader.ReadInt32(),
                    axes = reader.ReadByte(),
                    tag = reader.ReadString(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 4)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivityGear()
                {
                    type = reader.ReadInt32(),
                    toothCount = reader.ReadByte(),
                    radius = reader.ReadSingle(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 5)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivityHinge()
                {
                    type = reader.ReadInt32(),
                    oriented = reader.ReadByte(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 6)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivityRail()
                {
                    type = reader.ReadInt32(),
                    length = reader.ReadSingle(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
            if (num == 7)
                primitiveConnectivity.Items[index] = (object)new LEGOPrimitiveConnectivitySlider()
                {
                    type = reader.ReadInt32(),
                    length = reader.ReadSingle(),
                    startCapped = reader.ReadByte(),
                    endCapped = reader.ReadByte(),
                    angle = reader.ReadSingle(),
                    ax = reader.ReadSingle(),
                    ay = reader.ReadSingle(),
                    az = reader.ReadSingle(),
                    tx = reader.ReadSingle(),
                    ty = reader.ReadSingle(),
                    tz = reader.ReadSingle()
                };
        }
        return primitiveConnectivity;
    }

    public static void WriteConnectivity(this BinaryWriter writer, LEGOPrimitiveConnectivity connectivity)
    {
        writer.Write(connectivity.Items.Length);
        foreach (object obj in connectivity.Items)
        {
            if (obj is LEGOPrimitiveConnectivityAxel)
            {
                writer.Write(0);
                writer.WriteConnectivityAxel(obj as LEGOPrimitiveConnectivityAxel);
            }
            if (obj is LEGOPrimitiveConnectivityBall)
            {
                writer.Write(1);
                writer.WriteConnectivityBall(obj as LEGOPrimitiveConnectivityBall);
            }
            if (obj is LEGOPrimitiveConnectivityCustom2DField)
            {
                writer.Write(2);
                writer.WriteConnectivityCustom2DField(obj as LEGOPrimitiveConnectivityCustom2DField);
            }
            if (obj is LEGOPrimitiveConnectivityFixed)
            {
                writer.Write(3);
                writer.WriteConnectivityFixed(obj as LEGOPrimitiveConnectivityFixed);
            }
            if (obj is LEGOPrimitiveConnectivityGear)
            {
                writer.Write(4);
                writer.WriteConnectivityGear(obj as LEGOPrimitiveConnectivityGear);
            }
            if (obj is LEGOPrimitiveConnectivityHinge)
            {
                writer.Write(5);
                writer.WriteConnectivityHinge(obj as LEGOPrimitiveConnectivityHinge);
            }
            if (obj is LEGOPrimitiveConnectivityRail)
            {
                writer.Write(6);
                writer.WriteConnectivityRail(obj as LEGOPrimitiveConnectivityRail);
            }
            if (obj is LEGOPrimitiveConnectivitySlider)
            {
                writer.Write(7);
                writer.WriteConnectivitySlider(obj as LEGOPrimitiveConnectivitySlider);
            }
        }
    }

    private static void WriteConnectivityAxel(this BinaryWriter writer, LEGOPrimitiveConnectivityAxel axel)
    {
        writer.Write(axel.type);
        writer.Write(axel.length);
        writer.Write(axel.grabbing);
        writer.Write(axel.startCapped);
        writer.Write(axel.endCapped);
        writer.Write(axel.angle);
        writer.Write(axel.ax);
        writer.Write(axel.ay);
        writer.Write(axel.az);
        writer.Write(axel.tx);
        writer.Write(axel.ty);
        writer.Write(axel.tz);
    }

    private static void WriteConnectivityBall(this BinaryWriter writer, LEGOPrimitiveConnectivityBall ball)
    {
        writer.Write(ball.type);
        writer.Write(ball.angle);
        writer.Write(ball.ax);
        writer.Write(ball.ay);
        writer.Write(ball.az);
        writer.Write(ball.tx);
        writer.Write(ball.ty);
        writer.Write(ball.tz);
    }

    private static void WriteConnectivityCustom2DField(this BinaryWriter writer, LEGOPrimitiveConnectivityCustom2DField custom2DField)
    {
        writer.Write(custom2DField.type);
        writer.Write(custom2DField.width);
        writer.Write(custom2DField.height);
        writer.Write(custom2DField.angle);
        writer.Write(custom2DField.ax);
        writer.Write(custom2DField.ay);
        writer.Write(custom2DField.az);
        writer.Write(custom2DField.tx);
        writer.Write(custom2DField.ty);
        writer.Write(custom2DField.tz);
        writer.Write(custom2DField.Value);
    }

    private static void WriteConnectivityFixed(this BinaryWriter writer, LEGOPrimitiveConnectivityFixed Fixed)
    {
        if (Fixed.tag == null)
            Fixed.tag = string.Empty;
        writer.Write(Fixed.type);
        writer.Write(Fixed.axes);
        writer.Write(Fixed.tag);
        writer.Write(Fixed.angle);
        writer.Write(Fixed.ax);
        writer.Write(Fixed.ay);
        writer.Write(Fixed.az);
        writer.Write(Fixed.tx);
        writer.Write(Fixed.ty);
        writer.Write(Fixed.tz);
    }

    private static void WriteConnectivityGear(this BinaryWriter writer, LEGOPrimitiveConnectivityGear gear)
    {
        writer.Write(gear.type);
        writer.Write(gear.toothCount);
        writer.Write(gear.radius);
        writer.Write(gear.angle);
        writer.Write(gear.ax);
        writer.Write(gear.ay);
        writer.Write(gear.az);
        writer.Write(gear.tx);
        writer.Write(gear.ty);
        writer.Write(gear.tz);
    }

    private static void WriteConnectivityHinge(this BinaryWriter writer, LEGOPrimitiveConnectivityHinge hinge)
    {
        writer.Write(hinge.type);
        writer.Write(hinge.oriented);
        writer.Write(hinge.angle);
        writer.Write(hinge.ax);
        writer.Write(hinge.ay);
        writer.Write(hinge.az);
        writer.Write(hinge.tx);
        writer.Write(hinge.ty);
        writer.Write(hinge.tz);
    }

    private static void WriteConnectivityRail(this BinaryWriter writer, LEGOPrimitiveConnectivityRail rail)
    {
        writer.Write(rail.type);
        writer.Write(rail.length);
        writer.Write(rail.angle);
        writer.Write(rail.ax);
        writer.Write(rail.ay);
        writer.Write(rail.az);
        writer.Write(rail.tx);
        writer.Write(rail.ty);
        writer.Write(rail.tz);
    }

    private static void WriteConnectivitySlider(this BinaryWriter writer, LEGOPrimitiveConnectivitySlider slider)
    {
        writer.Write(slider.type);
        writer.Write(slider.length);
        writer.Write(slider.startCapped);
        writer.Write(slider.endCapped);
        writer.Write(slider.angle);
        writer.Write(slider.ax);
        writer.Write(slider.ay);
        writer.Write(slider.az);
        writer.Write(slider.tx);
        writer.Write(slider.ty);
        writer.Write(slider.tz);
    }

    public static LEGOPrimitivePhysicsAttributes ReadPhysicsAttributes(this BinaryReader reader)
    {
        return new LEGOPrimitivePhysicsAttributes()
        {
            inertiaTensor = reader.ReadString(),
            centerOfMass = reader.ReadString(),
            mass = reader.ReadSingle(),
            frictionType = reader.ReadByte()
        };
    }

    public static void WritePhysicsAttributes(this BinaryWriter writer, LEGOPrimitivePhysicsAttributes physicsAttributes)
    {
        if (physicsAttributes == null)
        {
            physicsAttributes = new LEGOPrimitivePhysicsAttributes();
            physicsAttributes.centerOfMass = string.Empty;
            physicsAttributes.inertiaTensor = string.Empty;
        }
        writer.Write(physicsAttributes.inertiaTensor);
        writer.Write(physicsAttributes.centerOfMass);
        writer.Write(physicsAttributes.mass);
        writer.Write(physicsAttributes.frictionType);
    }

    public static LEGOPrimitiveBounding ReadBounding(this BinaryReader reader)
    {
        return new LEGOPrimitiveBounding()
        {
            AABB = new LEGOPrimitiveBoundingAABB()
            {
                minX = reader.ReadSingle(),
                minY = reader.ReadSingle(),
                minZ = reader.ReadSingle(),
                maxX = reader.ReadSingle(),
                maxY = reader.ReadSingle(),
                maxZ = reader.ReadSingle()
            }
        };
    }

    public static void WriteBounding(this BinaryWriter writer, LEGOPrimitiveBounding bounding)
    {
        writer.Write(bounding.AABB.minX);
        writer.Write(bounding.AABB.minY);
        writer.Write(bounding.AABB.minZ);
        writer.Write(bounding.AABB.maxX);
        writer.Write(bounding.AABB.maxY);
        writer.Write(bounding.AABB.maxZ);
    }

    public static LEGOPrimitiveGeometryBounding ReadGeometryBounding(this BinaryReader reader)
    {
        return new LEGOPrimitiveGeometryBounding()
        {
            AABB = new LEGOPrimitiveGeometryBoundingAABB()
            {
                minX = reader.ReadSingle(),
                minY = reader.ReadSingle(),
                minZ = reader.ReadSingle(),
                maxX = reader.ReadSingle(),
                maxY = reader.ReadSingle(),
                maxZ = reader.ReadSingle()
            }
        };
    }

    public static void WriteGeometryBounding(this BinaryWriter writer, LEGOPrimitiveGeometryBounding geometryBounding)
    {
        writer.Write(geometryBounding.AABB.minX);
        writer.Write(geometryBounding.AABB.minY);
        writer.Write(geometryBounding.AABB.minZ);
        writer.Write(geometryBounding.AABB.maxX);
        writer.Write(geometryBounding.AABB.maxY);
        writer.Write(geometryBounding.AABB.maxZ);
    }
}
