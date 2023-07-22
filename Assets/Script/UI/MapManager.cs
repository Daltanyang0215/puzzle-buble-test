using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject map_panel;


    private void Start()
    {
        for (int i = MapDataBase.instance.MapDB.Count; i >=0 ; i--)
        {
          GameObject creatmap = Instantiate(map_panel,gameObject.transform);

            creatmap.GetComponent<MapPanel>().map_index = i;

            if (UserData.instance.map_Level < SendData.instance.maplevel)
            {
                UserData.instance.map_Level = SendData.instance.maplevel;
                UserData.instance.map_Level = Mathf.Clamp(UserData.instance.map_Level, 0, MapDataBase.instance.MapDB.Count - 1);
            }


            if (i <= UserData.instance.map_Level) creatmap.SetActive(true);
        }
    }

}
