// Decompiled with JetBrains decompiler
// Type: Edge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C4620B93-89DB-4575-91D4-00F0A36DE06A
// Assembly location: E:\Work Space\eca-decompilation\Assets\Plugins\Assembly-CSharp.dll

public class Edge
{
  public int Index1;
  public int Index2;

  public Edge(int a, int b)
  {
    this.Index1 = a;
    this.Index2 = b;
  }

  public static bool Equal(Edge x, Edge y)
  {
    return x.Index1 == y.Index1 && x.Index2 == y.Index2;
  }
}
