using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UnitSerializable<T>
{
    public UnitSerializable(List<T> _target) => target = _target;
    public List<T> target;
}

[Serializable]
public class Unit
{
    public int unit_Index;
    public Type unit_Type;
    public string unit_name;
    public Sprite unit_Image;
    public Sprite unit_Icon;

    public GameObject AttackEffet;

    public int unit_HP;
    public int unit_Attack;
    public int unit_Recovery;
    public int Attack_Count_MAX;


}
