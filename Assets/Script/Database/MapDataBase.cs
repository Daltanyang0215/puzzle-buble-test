using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDataBase : MonoBehaviour
{
    #region Singleton
    public static MapDataBase instance;
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

    public List<Map> MapDB = new List<Map>();
}
