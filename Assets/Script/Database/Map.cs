using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapSerializable<T>
{
    public MapSerializable(List<T> _target) => target = _target;
    public List<T> target;
}
[Serializable]
public class Map
{
    public string map_name;
    public int map_flower;

    public List<int> Enemy_index = new List<int>();
    //[HideInInspector]
    //public List<Enemy> map_enemy = new List<Enemy>();

    
}
