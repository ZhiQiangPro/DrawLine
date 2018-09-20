﻿/************************************************************
  Copyright (C), 2007-2017,BJ Rainier Tech. Co., Ltd.
  FileName: DragUI.cs
  Author:李婷      Version :1.0          Date: 2017-8-10
  Description:UGUI的拖拽面板功能
************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragUI : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    // 当前面板  
    public RectTransform panelRectTransform;
    // 父节点,这个最好是UI父节点，因为它的矩形大小刚好是屏幕大小  
    public RectTransform parentRectTransform;
    // 鼠标起点  
    private Vector2 originalLocalPointerPosition;
    // 面板起点  
    private Vector3 originalPanelLocalPosition;
   

    private static int siblingIndex = 0;
    void Awake()
    {
       
    }

    void Start()
    {
       
    }

    /// <summary>
    /// 鼠标按下 
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(PointerEventData data)
    {
        siblingIndex++;
        panelRectTransform.transform.SetSiblingIndex(siblingIndex);
        // 记录当前面板起点  
        originalPanelLocalPosition = panelRectTransform.localPosition;
        // 通过屏幕中的鼠标点，获取在父节点中的鼠标点  
        // parentRectTransform:父节点  
        // data.position:当前鼠标位置  
        // data.pressEventCamera:当前事件的摄像机  
        // originalLocalPointerPosition:获取当前鼠标起点  
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
    }
  
    /// <summary>
    ///   // 拖动 
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        if (panelRectTransform == null || parentRectTransform == null)
            return;
        Vector2 localPointerPosition;
        // 获取本地鼠标位置  
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, data.position, data.pressEventCamera, out localPointerPosition))
        {
            // 移动位置 = 本地鼠标当前位置 - 本地鼠标起点位置  
            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
            // 当前面板位置 = 面板起点 + 移动位置  
            panelRectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
        }
        ClampToWindow();
    }

    
    /// <summary>
    /// // 限制当前面板在父节点中的区域位置  
    /// </summary>
    void ClampToWindow()
    {
        // 面板位置  
        Vector3 pos = panelRectTransform.localPosition;
        // 如果是UI父节点，设置面板大小为0，那么最大最小位置为正负屏幕的一半  
        Vector3 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
        Vector3 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;
        //限制面板位置
        pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);
        panelRectTransform.localPosition = pos;
    }
}