/************************************************************
  Copyright (C), 2007-2017,BJ Rainier Tech. Co., Ltd.
  FileName: DrawTest.cs
  Author:       Version :1.0          Date: 
  Description:
************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class DrawTest : MonoBehaviour
{
    private Transform beiJing;
    private Button beiJing1; //背景1
    private Camera uIcamera;
    [SerializeField] private List<Vector2> pointList = new List<Vector2>();
    [SerializeField] private List<int> breakpointList = new List<int>();
    private List<Button> btnList = new List<Button>();
    private int nameIndex=0;
    private void Awake()
    {
        beiJing = transform.Find("背景");
        beiJing1 = transform.Find("背景1").GetComponent<Button>();
        uIcamera = transform.Find("Camera").GetComponent<Camera>();
    }

    private void Start()
    {
        for (int i = 0; i < beiJing.childCount; i++)
        {
            var i1 = i;
            beiJing.GetChild(i).GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
            {
                if (!MyDrawGLine.canDraw)
                {
                    pointList.Add(uIcamera.WorldToScreenPoint(beiJing.GetChild(i1).position));
                }

                MyDrawGLine.canDraw = true;
                pointList.Add(uIcamera.WorldToScreenPoint(beiJing.GetChild(i1).position));

                MyDrawGLine.PointList = pointList;
            });
        }

        beiJing1.OnClickAsObservable().Subscribe(_ =>
        {
            GameObject button = LoadObject("btn1", beiJing1.transform);
            button.transform.localScale = new Vector3(1, 1, 1);
            button.transform.position = uIcamera.ScreenToWorldPoint(Input.mousePosition);
            nameIndex++;
            button.name = nameIndex.ToString();
            if (!MyDrawGLine.canDraw)
            {
                pointList.Add(uIcamera.WorldToScreenPoint(button.transform.position));
            }
            pointList.Add(uIcamera.WorldToScreenPoint(button.transform.position));
            MyDrawGLine.PointList = pointList;
            MyDrawGLine.canDraw = true;

        });


        this.UpdateAsObservable().Select(_ => Input.GetMouseButtonDown(1))
            .Where(_ => _)
            .Subscribe(_ =>
            {
                breakpointList.Add(pointList.Count - 1);
                MyDrawGLine.BreakpointList = breakpointList;
                
            });
    }

    private GameObject LoadObject(string newPath, Transform parentsObject)
    {
        Object bpartGameObjectPreb = Resources.Load(newPath, typeof(GameObject));
        GameObject bpartGameObject = Instantiate(bpartGameObjectPreb) as GameObject;
        Debug.Assert(bpartGameObject != null, "bpartGameObject != null");
        bpartGameObject.transform.parent = parentsObject;
        return bpartGameObject;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 75, 30), "确定"))
        {
            breakpointList.Add(pointList.Count - 1);
            MyDrawGLine.BreakpointList = breakpointList;
        }

        if (GUI.Button(new Rect(0, 40, 75, 30), "自由"))
        {
            transform.Find("背景").gameObject.SetActive(false);
            transform.Find("背景1").gameObject.SetActive(true);

            MyDrawGLine.BreakpointList.Clear();
            MyDrawGLine.PointList.Clear();
        }

        if (GUI.Button(new Rect(0, 80, 75, 30), "固定"))
        {
            transform.Find("背景").gameObject.SetActive(true);
            transform.Find("背景1").gameObject.SetActive(false);
        }

        if (GUI.Button(new Rect(0, 120, 75, 30), "回退"))
        {
            if (pointList.Count >= 1)
            {
                if (beiJing1.transform.childCount >=1)
                {
                    Destroy(beiJing1.transform.GetChild(beiJing1.transform.childCount - 1).gameObject);
                    nameIndex = beiJing1.transform.childCount - 1;
                }

                pointList.RemoveAt(pointList.Count - 1);
                MyDrawGLine.PointList = pointList;
            }
        }
    }
}