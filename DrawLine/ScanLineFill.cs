/// <summary>
/// FileName: ScanLineFill.cs
/// Author: Jiang Xiaolong
/// Created Time: 2015.05.29
/// Version: 1.0
/// Company: Sunnytech
/// Function: 扫描线填充算法，没有作特殊处理，TODO：有些地方未优化；
///
/// Changed By:
/// Modification Time:
/// Discription:
/// </summary>
using System.Collections.Generic;
using System;


public class ScanLineFill
{

	public ScanLineFill()
	{ 
		
	}

}

/// <summary>
/// 定义的多边形，含有变数据；
/// </summary>
public class PlanePolygon
{
	//边数据表，采用最小y值索引
	private Dictionary<int, List<PlaneEdge>> edgeTable = new Dictionary<int, List<PlaneEdge>>();

	public int YMax = 0;
	public int YMin = 0;

	public PlanePolygon()
	{

	}

	/// <summary>
	/// 增加边；
	/// </summary>
	/// <param name="p1x">点1x值；</param>
	/// <param name="p1y">点1y值；</param>
	/// <param name="p2x">点2x值；</param>
	/// <param name="p2y">点2y值；</param>
	public void AddEdge(int p1x, int p1y, int p2x, int p2y)
	{
		PlaneEdge edge = new PlaneEdge(p1x, p1y, p2x, p2y);

		if (edgeTable.Count == 0)
		{
			YMin = Math.Min(p1y, p2y);
			YMax = Math.Max(p1y, p2y);
		}
		else
		{
			YMin = Math.Min(YMin, Math.Min(p1y, p2y));
			YMax = Math.Max(YMax, Math.Max(p1y, p2y));
		}

		if (edgeTable.ContainsKey(edge.ymin))
		{
			edgeTable[edge.ymin].Add(edge);
		}
		else
		{
			edgeTable.Add(edge.ymin, new List<PlaneEdge> { edge });
		}
	}

	/// <summary>
	/// 返回当前活性边；
	/// </summary>
	/// <param name="yValue">当前y值；</param>
	/// <returns></returns>
	public List<PlaneEdge> GetAEL(int yValue)
	{
		List<PlaneEdge> rList = new List<PlaneEdge>();
		if (edgeTable.ContainsKey(yValue))
		{
			foreach (PlaneEdge e in edgeTable[yValue])
			{
				rList.Add(e);
			}
			edgeTable.Remove(yValue);
		}
		return rList;
	}

	/// <summary>
	/// 返回水平直线；
	/// </summary>
	/// <returns></returns>
	public List<PlaneEdge> GetHorizontalLine()
	{
		List<PlaneEdge> rList = new List<PlaneEdge>();
		Dictionary<int, List<PlaneEdge>>.KeyCollection keys = edgeTable.Keys;
		foreach (int key in keys)
		{
			for (int i = 0; i < edgeTable[key].Count; i++)
			{
				if (edgeTable[key][i].IsHorizonEdge())
				{
					rList.Add(edgeTable[key][i]);
					edgeTable[key].RemoveAt(i);
					i--;
				}
			}
		}
		return rList;
	}

}


/// <summary>
/// 边结构，存储边数据；
/// </summary>
public struct PlaneEdge
{
	public int ymin;
	public int ymax;
	private float dx;
	public int x0;
	public int x1;

	public PlaneEdge(int p1x, int p1y, int p2x, int p2y)
	{
		ymin = Math.Min(p1y, p2y);
		ymax = Math.Max(p1y, p2y);
		if (ymin == p1y)
		{
			x0 = p1x;
			x1 = p2x;
		}
		else
		{
			x0 = p2x;
			x1 = p1x;
		}
		if (ymax == ymin)
		{
			dx = 0f;
		}
		else
		{
			dx = (float)(p2x - p1x) / (float)(p2y - p1y);
		}
	}

	/// <summary>
	/// 是否为水平边；
	/// </summary>
	/// <returns></returns>
	public bool IsHorizonEdge()
	{
		return ymin == ymax ? true : false;
	}

	/// <summary>
	/// 根据当前Y值返回X值，简化的交点计算方法；
	/// </summary>
	/// <param name="y">当前y值；</param>
	/// <returns>当前x值；</returns>
	public float XValue(int y)
	{
		return x0 + (y - ymin) * dx;
	}
}


