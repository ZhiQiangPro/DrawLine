/************************************************************
  Copyright (C), 2007-2017,BJ Rainier Tech. Co., Ltd.
  FileName: MyDrawGLine.cs
  Author: 崔志强      Version :1.0          Date: 
  Description:
************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyDrawGLine : MonoBehaviour
{
    public Material mat;
    public static bool canDraw = false;
    private static List<Vector2> pointList;

    public static List<Vector2> PointList
    {
        get { return pointList; }
        set { pointList = value; }
    }

    private static List<int> breakpointList;

    public static List<int> BreakpointList
    {
        get { return breakpointList; }
        set { breakpointList = value; }
    }


    private void Awake()
    {
        pointList = new List<Vector2>();
        breakpointList = new List<int>();
    }

    /// <summary>
    /// 开始画线
    /// </summary>
    public static void Draw()
    {
        canDraw = true;
    }

    /// <summary>
    /// 停止画线
    /// </summary>
    public static void DontDraw()
    {
        canDraw = false;
    }

    void OnPostRender()
    {
        if (canDraw)
        {
            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadPixelMatrix();
            GL.Color(Color.white);

            for (int i = 0; i < pointList.Count - 1; i++)

            {
                for (int j = 0; j < breakpointList.Count; j++)

                {
                    if (i == (int) (breakpointList[j]))

                    {
                        i++;
                    }
                }

                GL.Begin(GL.LINES);
                GL.Vertex3((pointList[i]).x, pointList[i].y, 0);
                if (i < pointList.Count - 1)
                {
                    GL.Vertex3((pointList[i + 1]).x, pointList[i + 1].y, 0);

                }
                GL.End();
            }

            GL.PopMatrix();
        }
    }
}