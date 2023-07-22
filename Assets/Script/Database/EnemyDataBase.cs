using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataBase : MonoBehaviour
{
    #region Singleton
    public static EnemyDataBase instance;
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

    public List<Enemy> EnemyDB = new List<Enemy>();
}
