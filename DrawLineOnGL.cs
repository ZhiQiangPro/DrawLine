/************************************************************
  Copyright (C), 2007-2017,BJ Rainier Tech. Co., Ltd.
  FileName : DrawLineOnGL.cs
  Author :      Version : 2.0          Date : 2017.1.18
  Description : 用于GL画线，可选择画一条线段、多条连续线段、多条不连续线段
************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLineOnGL : MonoBehaviour {

    /*******************Declare Variables**************************/
    public enum LineStyle
    {
        SINGLE = 1,//一条线段
        CONTINUOUS = 2,//多条连续线段
        DISCONTINUOUS = 4,//多条不连续线段
    }
    private static LineStyle lineStyle;
    public static LineStyle Style
    {
        get { return lineStyle; }
        set { lineStyle = value; }
    }    

    private static List<Vector2> linePoints;//线段端点
    public static List<Vector2> LinePoints
    {
        get { return linePoints; }
        set { linePoints = value; }
    }

    public Material mat;
    
    private static bool canDraw;    
    
	//**************************************
	//初始化函数，所挂载的gameobjec被激活时调用，且只调用一次，先于Start函数
	//**************************************
    void Awake()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        linePoints = new List<Vector2>();
    }

    //**************************************
    //初始化函数，在Update函数第一次被调用前调用，且只调用一次
    //**************************************
    void Start () 
    {

    }
  
    //**************************************
    //更新函数，且每帧调用一次
    //**************************************
    void Update ()
    {
        
    }

    public static void Draw(LineStyle style)
    {
        
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

    /// <summary>
    /// 清空线段端点集合
    /// </summary>
    public static void ClearLinePoints()
    {
        linePoints.Clear();
    }
    
    void OnPostRender()
    {
        if (canDraw)
        {
            //保存当前Matirx
            GL.PushMatrix();
            //刷新当前材质
            mat.SetPass(0);
            //设置pixelMatrix
            GL.LoadPixelMatrix();
            GL.Color(Color.white);

            switch (lineStyle)
            {
                case LineStyle.SINGLE://一条线段
                    GL.Begin(GL.LINES);
                    GL.Vertex3(linePoints[0].x, linePoints[0].y, 0);
                    GL.Vertex3(linePoints[1].x, linePoints[1].y, 0);
                    GL.End();
                    break;
                case LineStyle.CONTINUOUS://多条连续线段
                    for (int i = 0; i < linePoints.Count - 1; i++)
                    {
                        GL.Begin(GL.LINES);
                        GL.Vertex3(linePoints[i].x, linePoints[i].y, 0);
                        GL.Vertex3(linePoints[i + 1].x, linePoints[i + 1].y, 0);                        
                        GL.End();
                    }
                    break;
                case LineStyle.DISCONTINUOUS://多条不连续线段
                    for (int i = 0; i < linePoints.Count - 1; i += 2)
                    {
                        GL.Begin(GL.LINES);
                        GL.Vertex3(linePoints[i].x, linePoints[i].y, 0);
                        GL.Vertex3(linePoints[i + 1].x, linePoints[i + 1].y, 0);
                        GL.End();
                    }
                    break;
            }

            //读取之前的Matrix   
            GL.PopMatrix();
        }
    }
}