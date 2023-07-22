using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeamPanel : MonoBehaviour
{
    #region Singleton
    public static TeamPanel instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    UserData userData;

//    public UnitPanel_UI[] slots;
    public List<UnitPanel_UI> slots;

    public Transform slotholder;

    public UnitPanel_UI[] seletslots;
    public Transform seletslotholder;
    //[HideInInspector]
    public UnitPanel_UI temppanel;

    public Text total_hp;
    public Text total_fire;
    public Text total_water;
    public Text total_eath;
    public Text total_recevly;


    public void Start()
    {
        userData = UserData.instance;
        slots.Clear();
        slots.AddRange(seletslotholder.GetComponentsInChildren<UnitPanel_UI>());
        slots.AddRange(slotholder.GetComponentsInChildren<UnitPanel_UI>().ToList<UnitPanel_UI>());
        userData.onSlotCountChange += SlotChange;
        userData.onChangeItem += RedrawSlotUI;

        TextUpdata();
    }


    public void Swapteam(UnitPanel_UI first, UnitPanel_UI second)
    {
        Unit tempunit;
        int tempint;

        userData.user_Units[second.slotnum] = first.unit.unit_Index;
        userData.user_Units[first.slotnum] = second.unit.unit_Index;

        tempunit = second.unit;
        second.unit = first.unit;
        first.unit = tempunit;

        tempint = second.unit_index;
        second.unit_index = first.unit_index;
        first.unit_index = tempint;

        first.selectIcon.SetActive(false);
        second.selectIcon.SetActive(false);
        first.UpdateSlotUi();
        second.UpdateSlotUi();
        temppanel = null;
        selectclear();
        TextUpdata();

        userData.onChangeItem.Invoke();
    }

    public void selectclear()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].selectIcon.SetActive(false);
        }
        TextUpdata();
    }


    private void SlotChange(int val)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].slotnum = i;

            if (i < userData.Slotcnt)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
        TextUpdata();
    }


    public void slotadd()
    {
        userData.Slotcnt+=5;
        TextUpdata();
        RedrawSlotUI();
    }
    void RedrawSlotUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].RemoveSlot();
        }
        for (int i = 0; i < userData.user_Units.Count; i++)
        {
            slots[i].unit = UnitDataBase.instance.UnitDB[UserData.instance.user_Units[i]];
            //seletslots[i].unit = UnitDataBase.instance.UnitDB[UserData.instance.user_Team[i]];
            slots[i].UpdateSlotUi();
        }
        TextUpdata();
    }

    public void TextUpdata()
    {
        int hp = 0, fire = 0, water = 0, eath = 0, recovery = 0;

        for (int i = 0; i < 6;  i++)
        {
            hp += slots[i].unit.unit_HP;
            recovery += slots[i].unit.unit_Recovery;

            switch (slots[i].unit.unit_Type)
            {
                case Type.Fire:
                    fire += slots[i].unit.unit_Attack;
                    break;
                case Type.Water:
                    water += slots[i].unit.unit_Attack;
                    break;
                case Type.Eath:
                    eath += slots[i].unit.unit_Attack;
                    break;
                default:
                    break;
            }
        }
        total_hp.text       = hp.ToString();
        total_fire.text     = fire.ToString();
        total_water.text    = water.ToString();
        total_eath.text     = eath.ToString();
        total_recevly.text  = recovery.ToString();
    }

}
