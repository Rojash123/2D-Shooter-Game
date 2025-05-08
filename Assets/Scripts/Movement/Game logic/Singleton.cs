using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> :MonoBehaviour where T:MonoBehaviour
{
    private static T instance;
    [SerializeField] private bool donotDestroyOnLoad;
    public static T Instance
    {
        get
        {
            if (instance == null)
                SetInstance();
            return instance;
        }
        private set
        {
            Instance = instance;
        }
    }
    public virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        if (!donotDestroyOnLoad)
        {
            return;
        }

        DontDestroyOnLoad(gameObject);

    }
    public void DestroySingleton()
    {
        Destroy(gameObject);
        instance = null;
    }
    private static void SetInstance()
    {
        if (instance != null)
        {
            return;
        }
        instance = FindObjectOfType<T>();

    }
}
