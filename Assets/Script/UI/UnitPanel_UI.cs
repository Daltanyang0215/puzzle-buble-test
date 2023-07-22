using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitPanel_UI : MonoBehaviour , IPointerUpHandler,IPointerDownHandler,IPointerExitHandler
{

    public int slotnum;
    public int unit_index;
    public Unit unit;
    public Image unitIcon;
    public bool selected;
    public GameObject selectIcon;

    private float time;
    private bool touch;

    private void Update()
    {
        if (touch)
        {
            time += Time.deltaTime;
            if(time > 1f)
            {
                UserData.instance.SelectUnit(slotnum);
                touch = false;
                time = 0;
            }
        }
    }

    public void UpdateSlotUi()
    {
        unitIcon.sprite = unit.unit_Icon;
       if(unit.unit_Index!=0) unitIcon.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        unit = null;
        unitIcon.gameObject.SetActive(false);

    }




    public void OnPointerUp(PointerEventData eventData)
    {
        if (unit == null || UserData.instance.unit_Ui.activeSelf)
        {
            time = 0;
            touch = false;
            return;
        }
        else if (selected && TeamPanel.instance.temppanel == null)
        {
            TeamPanel.instance.temppanel = this;
            selectIcon.SetActive(true);
        }
        else if (TeamPanel.instance.temppanel != null)
        {
            TeamPanel.instance.Swapteam(TeamPanel.instance.temppanel, this);

        }
        time = 0;
        touch = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        touch = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        time = 0;
        touch = false;
    }
}
