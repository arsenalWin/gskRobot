using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class GUILine {
	//将图片初始化为纯白色
	public static void TextureInitial(Texture2D _argb32){
		for (int i = 0; i < _argb32.width; i++)
		{
			for (int j = 0; j < _argb32.height; j++)
			{
				Color color = new Color(1, 1, 1, 0.0f);
				_argb32.SetPixel(i, j, color);
			}
		}
		_argb32.Apply();
	}
	
	public static void DrawSegment(Texture2D draw_tex, int start_x, int start_y, int end_x, int end_y, Color line_color, int thickness)
	{//Debug.Log (start_x);Debug.Log (start_y);Debug.Log (end_x);Debug.Log (end_y);
		int R = thickness; //端点半径
		
		//DDA算法
		//YX轴步进
		int xDis = end_x - start_x;
		int yDis = end_y - start_y;
		bool isXStep = true;
		int maxStep = 0;
		//X、Y轴步进选择
		if (Math.Abs(xDis) >= Math.Abs(yDis))
		{
			isXStep = true;
			maxStep = Math.Abs(xDis);
		}
		else
		{
			isXStep = false;
			maxStep = Math.Abs(yDis);
		}
		float widthMakeUp = 0f;
		//斜线宽度补偿
		if (start_x != end_x && start_y != end_y)
		{
			if (isXStep)
			{
				widthMakeUp = (float)(R / Math.Cos(Math.Atan(Math.Abs((double)(end_y - start_y) / (double)(end_x - start_x)))));
			}
			else
			{
				widthMakeUp = (float)(R / Math.Sin(Math.Atan(Math.Abs((double)(end_y - start_y) / (double)(end_x - start_x)))));
			}
		}
		else
		{
			widthMakeUp = thickness;
		}

		//如果是多边形，求四边

		//四条边上的已知点
		float aX = 0;
		float aY = 0;
		float bX = 0;
		float bY = 0;
		float cX = 0;
		float cY = 0;
		float dX = 0;
		float dY = 0;

		if (thickness > 1)
		{
			if (isXStep)
			{
				aX = start_x;
				aY = start_y + widthMakeUp;
				bX = end_x;
				bY = end_y + widthMakeUp;
				cX = start_x;
				cY = start_y - widthMakeUp;
				dX = end_x;
				dY = end_y - widthMakeUp;
			}
			else
			{
				aX = start_x - widthMakeUp;
				aY = start_y;
				bX = end_x - widthMakeUp;
				bY = end_y;
				cX = start_x + widthMakeUp;
				cY = start_y;
				dX = end_x + widthMakeUp;
				dY = end_y;
			}
			//普通斜线段情况，重新计算4个交点
			if (start_x != end_x && start_y != end_y)
			{
				float k = (float)(start_x - end_x) / (float)(end_y - start_y);
				float b1 = start_y - k * start_x;
				float b2 = end_y - k * end_x;
				//线段AB
				float bAB = aY + (1f / k) * aX;
				//线段CD
				float bCD = cY + (1f / k) * cX;
				aX = (bAB - b1) / (k + 1f / k);
				aY = k * aX + b1;
				cX = (bCD - b1) / (k + 1f / k);
				cY = k * cX + b1;
				bX = (bAB - b2) / (k + 1f / k);
				bY = k * bX + b2;
				dX = (bCD - b2) / (k + 1f / k);
				dY = k * dX + b2;
			}

			//创建线多边形，赋值四条边
			PlanePolygon linePolygon = new PlanePolygon();
			linePolygon.AddEdge(Convert.ToInt32(aX), Convert.ToInt32(aY), Convert.ToInt32(bX), Convert.ToInt32(bY));
			linePolygon.AddEdge(Convert.ToInt32(aX), Convert.ToInt32(aY), Convert.ToInt32(cX), Convert.ToInt32(cY));
			linePolygon.AddEdge(Convert.ToInt32(bX), Convert.ToInt32(bY), Convert.ToInt32(dX), Convert.ToInt32(dY));
			linePolygon.AddEdge(Convert.ToInt32(cX), Convert.ToInt32(cY), Convert.ToInt32(dX), Convert.ToInt32(dY));

			//扫描范围
			int yMin = linePolygon.YMin;
			int yMax = linePolygon.YMax;

			//先画好水平线
			List<PlaneEdge> horizonLine = linePolygon.GetHorizontalLine();
			for (int i = 0; i < horizonLine.Count; i++)
			{
				if (horizonLine[i].x0 < horizonLine[i].x1)
				{
					for (int xValue = horizonLine[i].x0; xValue <= horizonLine[i].x1; xValue++)
					{
						SetPixelColor(draw_tex, xValue, horizonLine[i].ymin, line_color);
					}
				}
				else
				{
					for (int xValue = horizonLine[i].x1; xValue <= horizonLine[i].x0; xValue++)
					{
						SetPixelColor(draw_tex, xValue, horizonLine[i].ymin, line_color);
					}
				}
			}

			//初始化活性边表
			List<PlaneEdge> AELTable = new List<PlaneEdge>();
			for (int activeY = yMin; activeY <= yMax; activeY++)
			{
				//加入到活性边表
				List<PlaneEdge> tempAEL = linePolygon.GetAEL(activeY);
				if (tempAEL.Count > 0)
				{
					AELTable.AddRange(tempAEL);
				}

				//遍历活性边表，求交点
				List<int> insectP = new List<int>();
				for (int i = 0; i < AELTable.Count; i++)
				{
					insectP.Add(Convert.ToInt32(AELTable[i].XValue(activeY)));
					//删除求解完成的边
					if (AELTable[i].ymax - 1 == activeY)
					{
						AELTable.RemoveAt(i);
						i--;
					}
				}

				//交点排序
				insectP.Sort();
				//填充
				if (insectP.Count % 2 != 0)
				{
					Debug.LogWarning("交点个数不为偶数，自动删除最后一个点，图形应该有误！");
					insectP.RemoveAt(insectP.Count - 1);
				}
				for (int i = 0; i < insectP.Count; i = i + 2)
				{
					for (int xValue = insectP[i]; xValue <= insectP[i + 1]; xValue++)
					{
						SetPixelColor(draw_tex, xValue, activeY, line_color);
					}
				}
			}

			//端点处理，以终点和起点为圆心，画圆
			//起点
			for (int i = start_x - R; i <= start_x + R; i++)
			{
				for (int j = start_y - R; j <= start_y + R; j++)
				{
					//判断点是否在圆内
					if ((i - start_x) * (i - start_x) + (j - start_y) * (j - start_y) <= R * R)
					{
						SetPixelColor(draw_tex, i, j, line_color);
					}
				}
			}
			//终点
			for (int i = end_x - R; i <= end_x + R; i++)
			{
				for (int j = end_y - R; j <= end_y + R; j++)
				{
					//判断点是否在圆内
					if ((i - end_x) * (i - end_x) + (j - end_y) * (j - end_y) <= R * R)
					{
						SetPixelColor(draw_tex, i, j, line_color);
					}
				}
			}
		}
		else  //最细线段，因为Unity显示的问题，采用了Wu反走样算法 
		{
			float fxUnit = (float)xDis / (float)maxStep;
			float fyUnit = (float)yDis / (float)maxStep;
			//设置起点终点颜色
			SetPixelColor(draw_tex, start_x, start_y, line_color);
			SetPixelColor(draw_tex, end_x, end_y, line_color);
			float x = (float)start_x;
			float y = (float)start_y;
			int xInt = 0;
			int yInt = 0;
			for (int i = 1; i <= maxStep; i++)
			{
				x += fxUnit;
				y += fyUnit;
				xInt = (int)x;
				yInt = (int)y;

				//点对反走样
				if (isXStep)
				{
					//一条线段用一对点来实现反走样
					if (yInt < Convert.ToInt32(y))
					{
						SetPixelColor(draw_tex, Convert.ToInt32(x), yInt, line_color);
					}
					else
					{
						SetPixelColor(draw_tex, Convert.ToInt32(x), yInt + 1, line_color);
					}

					//1的情况，用填充算法不是很好，采用Wu反走样去填补；
					if (thickness != 0)
					{
						thickness = Convert.ToInt32(widthMakeUp) * 2;

						if (yInt < Convert.ToInt32(y))
						{
							for (int yValue = 0; yValue <= widthMakeUp; yValue++)
							{
								if (yValue % 2 == 0)
								{
									SetPixelColor(draw_tex, Convert.ToInt32(x), yInt + yValue / 2, line_color);
								}
								else
								{
									SetPixelColor(draw_tex, Convert.ToInt32(x), yInt - 1 - (yValue / 2), line_color);
								}
							}
						}
						else
						{
							for (int yValue = 0; yValue <= widthMakeUp; yValue++)
							{
								if (yValue % 2 == 0)
								{
									SetPixelColor(draw_tex, Convert.ToInt32(x), yInt - yValue / 2 - 1, line_color);
								}
								else
								{
									SetPixelColor(draw_tex, Convert.ToInt32(x), yInt + (yValue / 2) , line_color);
								}
							}
						}
					}
				}
				else
				{
					//一条线段用一对点来实现反走样
					if (xInt < Convert.ToInt32(x))
					{
						SetPixelColor(draw_tex, xInt, Convert.ToInt32(y), line_color);
					}
					else
					{
						SetPixelColor(draw_tex, xInt + 1, Convert.ToInt32(y), line_color);
					}

					//1的情况，用填充算法不是很好，采用Wu反走样去填补；
					if (thickness != 0)
					{
						thickness = Convert.ToInt32(widthMakeUp) * 2;

						if (xInt < Convert.ToInt32(x))
						{
							for (int xValue = 0; xValue <= widthMakeUp; xValue++)
							{
								if (xValue % 2 == 0)
								{
									SetPixelColor(draw_tex, xInt + xValue / 2, Convert.ToInt32(y), line_color);
								}
								else
								{
									SetPixelColor(draw_tex, xInt - 1 - (xValue / 2), Convert.ToInt32(y), line_color);
								}
							}
						}
						else
						{
							for (int xValue = 0; xValue <= widthMakeUp; xValue++)
							{
								if (xValue % 2 == 0)
								{
									SetPixelColor(draw_tex, xInt - xValue / 2 - 1, Convert.ToInt32(y), line_color);
								}
								else
								{
									SetPixelColor(draw_tex, xInt + (xValue / 2), Convert.ToInt32(y), line_color);
								}
							}
						}
					}
				}
				SetPixelColor(draw_tex, Convert.ToInt32(x), Convert.ToInt32(y), line_color);
			}
		}

		
		draw_tex.Apply();
	}

	private static void SetPixelColor(Texture2D draw_tex, int x, int y, Color line_color)
	{
		if (x >= 0 && x <= draw_tex.width && y >= 0 && y <= draw_tex.height)
		{
			draw_tex.SetPixel(x, y, line_color);
		}
	}

	public static void DrawPolygon(Texture2D draw_tex, List<int> point_list1, List<int> point_list2, Color area_color)
	{
		if (point_list1.Count != point_list2.Count || point_list1.Count < 3)
		{
			Debug.LogError("多边形点集合错误，个数不对！");
			return;
		}
		//创建线多边形，赋值四条边
		PlanePolygon linePolygon = new PlanePolygon();
		for (int i = 0; i < point_list1.Count - 1; i++)
		{
			linePolygon.AddEdge(point_list1[i], point_list2[i], point_list1[i + 1], point_list2[i + 1]);
		}
		linePolygon.AddEdge(point_list1[point_list1.Count - 1], point_list2[point_list1.Count - 1], point_list1[0], point_list2[0]);

		//扫描范围
		int yMin = linePolygon.YMin;
		int yMax = linePolygon.YMax;

		//先画好水平线
		List<PlaneEdge> horizonLine = linePolygon.GetHorizontalLine();
		for (int i = 0; i < horizonLine.Count; i++)
		{
			if (horizonLine[i].x0 < horizonLine[i].x1)
			{
				for (int xValue = horizonLine[i].x0; xValue <= horizonLine[i].x1; xValue++)
				{
					SetPixelColor(draw_tex, xValue, horizonLine[i].ymin, area_color);
				}
			}
			else
			{
				for (int xValue = horizonLine[i].x1; xValue <= horizonLine[i].x0; xValue++)
				{
					SetPixelColor(draw_tex, xValue, horizonLine[i].ymin, area_color);
				}
			}
		}
		//初始化活性边表
		List<PlaneEdge> AELTable = new List<PlaneEdge>();
		for (int activeY = yMin; activeY <= yMax; activeY++)
		{
			//加入到活性边表
			List<PlaneEdge> tempAEL = linePolygon.GetAEL(activeY);
			if (tempAEL.Count > 0)
			{
				AELTable.AddRange(tempAEL);
			}
			

			//遍历活性边表，求交点
			List<int> insectP = new List<int>();
			for (int i = 0; i < AELTable.Count; i++)
			{
				insectP.Add(Convert.ToInt32(AELTable[i].XValue(activeY)));
				//删除求解完成的边
				if (AELTable[i].ymax - 1 == activeY)
				{
					AELTable.RemoveAt(i);
					i--;
				}
			}

			//交点排序
			insectP.Sort();
			//填充
			if (insectP.Count % 2 != 0)
			{
				Debug.LogWarning("交点个数不为偶数，自动删除最后一个点，图形应该有误！");
				insectP.RemoveAt(insectP.Count - 1);
			}
			for (int i = 0; i < insectP.Count; i = i + 2)
			{
				for (int xValue = insectP[i]; xValue <= insectP[i + 1]; xValue++)
				{
					SetPixelColor(draw_tex, xValue, activeY, area_color);
				}
			}
		}
		draw_tex.Apply();
	}

}
