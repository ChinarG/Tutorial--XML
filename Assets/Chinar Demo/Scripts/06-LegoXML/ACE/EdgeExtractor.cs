// Decompiled with JetBrains decompiler
// Type: EdgeExtractor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class EdgeExtractor
{
  public static List<Vector3> PositionList = new List<Vector3>();
  public static List<Triangle> Triangles = new List<Triangle>();

  public static Mesh Extract(Vector3[] vertexList, int[] triangleList)
  {
    EdgeExtractor.PositionList.Clear();
    EdgeExtractor.Triangles.Clear();
    List<int> intList = new List<int>();
    for (int index1 = 0; index1 < vertexList.Length; ++index1)
    {
      int num = -1;
      for (int index2 = 0; index2 < EdgeExtractor.PositionList.Count; ++index2)
      {
        if (vertexList[index1] == EdgeExtractor.PositionList[index2])
        {
          num = index2;
          break;
        }
      }
      if (num < 0)
      {
        num = EdgeExtractor.PositionList.Count;
        EdgeExtractor.PositionList.Add(vertexList[index1]);
      }
      if (index1 != num)
      {
        for (int index2 = 0; index2 < triangleList.Length; ++index2)
        {
          if (triangleList[index2] == index1)
            triangleList[index2] = num;
        }
      }
    }
    int index3 = 0;
    while (index3 < triangleList.Length)
    {
      EdgeExtractor.Triangles.Add(new Triangle(triangleList[index3], triangleList[index3 + 1], triangleList[index3 + 2]));
      index3 += 3;
    }
    List<Vector3> inVertices = new List<Vector3>();
    for (int index1 = 0; index1 < EdgeExtractor.Triangles.Count; ++index1)
    {
      Triangle triangle1 = EdgeExtractor.Triangles[index1];
      for (int i = 0; i < 3; ++i)
      {
        Edge edge = triangle1.GetEdge(i);
        for (int index2 = index1 + 1; index2 < EdgeExtractor.Triangles.Count; ++index2)
        {
          Triangle triangle2 = EdgeExtractor.Triangles[index2];
          if (triangle2.HaveEdge(edge) && (double) Mathf.Abs(Vector3.Dot(triangle1.Normal, triangle2.Normal)) < 0.699999988079071)
          {
            inVertices.Add(EdgeExtractor.PositionList[edge.Index1]);
            inVertices.Add(EdgeExtractor.PositionList[edge.Index2]);
          }
        }
      }
    }
    int[] indices = new int[inVertices.Count];
    for (int index1 = 0; index1 < indices.Length; ++index1)
      indices[index1] = index1;
    Mesh mesh = new Mesh();
    mesh.SetVertices(inVertices);
    mesh.SetIndices(indices, MeshTopology.Lines, 0);
    mesh.UploadMeshData(false);
    return mesh;
  }
}
