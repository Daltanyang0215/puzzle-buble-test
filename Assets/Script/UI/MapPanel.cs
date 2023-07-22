using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour, IPointerUpHandler
{
    public Map map;
    public int map_index;

    public void OnPointerUp(PointerEventData eventData)
    {
        SendData.instance.maplevel = map_index;
        SceneManager.LoadScene(1);

        for (int i = 0; i < 6; i++)
        {
            SendData.instance.units[i] = TeamPanel.instance.slots[i].unit;
        }
        UserData.instance.Save();

    }

    private void Start()
    {
        map = MapDataBase.instance.MapDB[map_index];
        gameObject.transform.GetChild(0).GetComponent<Text>().text = map.map_name;
    }

}
