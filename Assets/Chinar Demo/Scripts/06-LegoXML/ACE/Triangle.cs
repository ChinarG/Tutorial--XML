// Decompiled with JetBrains decompiler
// Type: Triangle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
  private static List<int> temp = new List<int>();
  public int A;
  public int B;
  public int C;

  public Triangle(int a, int b, int c)
  {
    Triangle.temp.Clear();
    Triangle.temp.Add(a);
    Triangle.temp.Add(b);
    Triangle.temp.Add(c);
    Triangle.temp.Sort();
    this.A = Triangle.temp[0];
    this.B = Triangle.temp[1];
    this.C = Triangle.temp[2];
  }

  public Edge GetEdge(int i)
  {
    switch (i)
    {
      case 0:
        return new Edge(this.A, this.B);
      case 1:
        return new Edge(this.B, this.C);
      case 2:
        return new Edge(this.A, this.C);
      default:
        return (Edge) null;
    }
  }

  public bool HaveEdge(Edge e)
  {
    for (int i = 0; i < 3; ++i)
    {
      if (Edge.Equal(this.GetEdge(i), e))
        return true;
    }
    return false;
  }

  public Vector3 Normal
  {
    get
    {
      return Vector3.Cross(EdgeExtractor.PositionList[this.A] - EdgeExtractor.PositionList[this.B], EdgeExtractor.PositionList[this.C] - EdgeExtractor.PositionList[this.B]).normalized;
    }
  }
}
