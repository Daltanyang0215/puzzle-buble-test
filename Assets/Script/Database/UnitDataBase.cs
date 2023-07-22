using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDataBase : MonoBehaviour
{
    #region Singleton
    public static UnitDataBase instance;
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

    public List<Unit> UnitDB = new List<Unit>();
    

}
