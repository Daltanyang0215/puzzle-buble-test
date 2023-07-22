using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendData : MonoBehaviour
{
    #region Singleton
    public static SendData instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(instance);
    }
    #endregion

    public int maplevel;
    public List<Unit> units;

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            if (UserData.instance.user_Units[i] != 0)
            {
                units[i] =UnitDataBase.instance.UnitDB[UserData.instance.user_Units[i]];
            }
        }
        
    }

}
