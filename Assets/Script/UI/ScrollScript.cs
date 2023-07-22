using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollScript : ScrollRect
{
    bool forParent;

    PrantScrollManager pm;
    ScrollRect parentScrollRect;
    protected override void Start()
    {
        pm = GameObject.FindWithTag("MainScrollView").GetComponent<PrantScrollManager>();
        parentScrollRect = GameObject.FindWithTag("MainScrollView").GetComponent<ScrollRect>();
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {

        forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if (forParent)
        {
            pm.OnBeginDrag(eventData);
            parentScrollRect.OnBeginDrag(eventData);
        }
        else base.OnBeginDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            pm.OnDrag(eventData);
            parentScrollRect.OnDrag(eventData);
        }
        else base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            pm.OnEndDrag(eventData);
            parentScrollRect.OnEndDrag(eventData);
        }
        else base.OnEndDrag(eventData);
    }

}
