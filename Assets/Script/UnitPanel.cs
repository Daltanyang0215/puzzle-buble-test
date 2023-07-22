using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour,IPointerUpHandler
{
    public int panelnum;
    public Unit unit;
    public Image unitIcon;

    public Slider AttackPoint;
    public int attack;
    public Image Attacksilder;
    public Color Attackcolor;

    public GameObject AttackEffect;
    private Transform effecttransform;

    private void Start()
    {
        effecttransform = HexegonBubleManager.instance.effect_transform;
        StartCoroutine(SlideUpdata());
    }


    public void UpdateSlotUi()
    {
        unitIcon.sprite = unit.unit_Icon;
        AttackPoint.maxValue = unit.Attack_Count_MAX;

        switch (unit.unit_Type)
        {
            case Type.Fire:
                Attackcolor = Color.red;
                break;
            case Type.Water:
                Attackcolor = Color.blue;
                break;
            case Type.Eath:
                Attackcolor = Color.green;
                break;
        }

        Attacksilder.color = Attackcolor;
        unitIcon.gameObject.SetActive(true);

        if (unit.AttackEffet != null)
        {
            AttackEffect = unit.AttackEffet;
        }
    }
    public void RemoveSlot()
    {
        unit = null;
        unitIcon.gameObject.SetActive(false);

    }


    public bool Attack()
    {
        bool isAttack = false;
        attack++;


        isAttack = true;
        return isAttack;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public IEnumerator SlideUpdata()
    {
        WaitForFixedUpdate wt = new WaitForFixedUpdate();
        while (true)
        {
            if (attack > AttackPoint.value)
            {
                AttackPoint.value++;
                if (AttackPoint.value >= AttackPoint.maxValue)
                {
                    attack -= (int)AttackPoint.maxValue;
                    AttackPoint.value = 0;
                    HexegonBubleManager.instance.enemy.now_hp -= unit.unit_Attack;
                    Instantiate(AttackEffect, HexegonBubleManager.instance.enemy.transform.position, Quaternion.identity, effecttransform);
                    transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                }
            }
            if (transform.localScale.x > 1)
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.03f, transform.localScale.y - 0.03f, 1f);
            }
            else if(transform.localScale.x < 1)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            yield return wt;
        }
    }
}
