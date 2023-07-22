using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    #region Singleton
    public static UserData instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(instance);
    }
    #endregion

    public string user_Name;
    public int user_Level;
    public int map_Level;

    //public int[] user_Team = new int[6];
    public List<int> user_Units = new List<int>();

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    private int slotcnt;

    public GameObject unitpanel;

    public GameObject get_Ui;
    public Image get_icon;

    public GameObject unit_Ui;
    public GameObject unit_Ui_advencd;
    public Image unit_icon;
    int saveint;

    string filePath;

    [SerializeField]
    public class Data
    {
        public string username;
        public int user_Level;
        public int map_Level;
        public int slotcnt;
        public List<int> user_Units;
    }
        
    public int Slotcnt
    {
        get => slotcnt;
        set
        {
            slotcnt = value;
            if(slotcnt > TeamPanel.instance.slots.Count)
            {
                for (int i = TeamPanel.instance.slots.Count; i < slotcnt; i++)
                {
                    GameObject addslot = Instantiate(unitpanel, TeamPanel.instance.transform);
                    TeamPanel.instance.slots.Add(addslot.GetComponent<UnitPanel_UI>());
                }
            }
            onSlotCountChange.Invoke(slotcnt);
        }
    }

    public void Start()
    {
        filePath = Application.persistentDataPath + "/MyUnitsText.txt";
        Load();
        onChangeItem.Invoke();
    }
    public bool AddUnit(Unit _unit)
    {
        if (user_Units.Count < Slotcnt)
        {
            user_Units.Add(_unit.unit_Index);
            if (onChangeItem != null)
            {
                onChangeItem.Invoke();
            }
            return true;
        }
        return false;
    }
    public void RemoveUnit(int _index)
    {
        user_Units.RemoveAt(_index);
        onChangeItem.Invoke();
    }

    public void RemoveData()
    {
        File.Delete(filePath);
        Load();
        onChangeItem.Invoke();
    }

    public void ShopUnit()
    {
        

        int ran = Random.Range(1, UnitDataBase.instance.UnitDB.Count);
        if(!AddUnit(UnitDataBase.instance.UnitDB[ran]))
        {
            Debug.Log("보관함이 모자랍니다");
            get_Ui.SetActive(false);
            return;
        }
        get_icon.sprite = UnitDataBase.instance.UnitDB[ran].unit_Icon;
        onChangeItem.Invoke();
    }

    public void SelectUnit(int index)
    {
        saveint = index;
        unit_Ui.SetActive(true);
        unit_icon.sprite = TeamPanel.instance.slots[index].unit.unit_Icon;
        unit_Ui_advencd.transform.GetChild(0).GetComponent<Text>().text = TeamPanel.instance.slots[index].unit.unit_name.ToString();
        unit_Ui_advencd.transform.GetChild(1).GetComponent<Text>().text = TeamPanel.instance.slots[index].unit.unit_Index.ToString();
        unit_Ui_advencd.transform.GetChild(2).GetComponent<Text>().text = TeamPanel.instance.slots[index].unit.unit_Type.ToString();
        unit_Ui_advencd.transform.GetChild(3).GetComponent<Image>().sprite = TeamPanel.instance.slots[index].unit.unit_Icon;
        unit_Ui_advencd.transform.GetChild(4).GetComponent<Text>().text = TeamPanel.instance.slots[index].unit.unit_HP.ToString();
        unit_Ui_advencd.transform.GetChild(5).GetComponent<Text>().text = TeamPanel.instance.slots[index].unit.unit_Attack.ToString();
        unit_Ui_advencd.transform.GetChild(6).GetComponent<Text>().text = TeamPanel.instance.slots[index].unit.unit_Recovery.ToString();
        unit_Ui_advencd.transform.GetChild(7).GetComponent<Text>().text = TeamPanel.instance.slots[index].unit.Attack_Count_MAX.ToString();
    }
    public void SelectRemoveUnit()
    {
        user_Units.RemoveAt(saveint);
        saveint = 0;
        onChangeItem.Invoke();
    }

    public void Save()
    {
        Data savedata = new Data();
        savedata.username = user_Name;
        savedata.user_Level = user_Level;
        savedata.map_Level = map_Level;
        savedata.user_Units = user_Units;
        savedata.slotcnt = Slotcnt;
        string savesata = JsonUtility.ToJson(savedata);
        File.WriteAllText(filePath, savesata);

        Debug.Log("save");
    }
    public void Load()
    {
        if (!File.Exists(filePath))
        {

            Slotcnt = 26;
            return;
        }

        string loaddata = File.ReadAllText(filePath);

        Data load_data = new Data();
        load_data = JsonUtility.FromJson<Data>(loaddata);
        user_Name = load_data.username;
        user_Level = load_data.user_Level;
        map_Level = load_data.map_Level;
        user_Units = load_data.user_Units;
        Slotcnt = load_data.slotcnt;
        onChangeItem.Invoke();
        Debug.Log("load");
    }

    public void Quit()
    {
        Save();
        Application.Quit();
    }
}
