using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySerializable<T>
{
    public EnemySerializable(List<T> _target) => target = _target;
    public List<T> target;
}
[Serializable]
public class Enemy
{
    public int unit_Index;
    public Type enemy_Type;
    public string enemy_name;
    public Sprite enemy_Image;

    public int enemy_HP;
    public int enemy_Attack;
    public int enemy_Recovery;
    public int Attack_Count_MAX;
}
