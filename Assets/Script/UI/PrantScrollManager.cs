using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrantScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTR;

    public Slider tapslider;
    public RectTransform[] btnRect; 

    const int SIZE = 4;
    float[] pos = new float[SIZE];
    float distance,curPos, targetPos;
    int targetIndex;
    bool isDrag;
    void Start()
    {
        distance = 1f / (SIZE - 1);
        for (int i = 0; i < SIZE; i++)
        { 
            pos[i] = distance * i;
        }

    }

    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();

    public void OnDrag(PointerEventData eventData) => isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        if(curPos == targetPos)
        {
            if(eventData.delta.x>18&& curPos -distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }
            else if (eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }

        for (int i = 0; i < SIZE; i++)
        {
            if (contentTR.GetChild(i).GetComponent<ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
                contentTR.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1; 
        }

    }

    float SetPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        }
        return 0;
    }

    void Update()
    {
        tapslider.value = scrollbar.value;

        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);
            for (int i = 0; i < SIZE; i++) btnRect[i].sizeDelta = new Vector2(i == targetIndex ? 360 : 180, btnRect[i].sizeDelta.y);
        }
    }

    public void TabClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }

}
