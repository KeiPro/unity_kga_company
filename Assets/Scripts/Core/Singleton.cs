using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];

                if (objs.Length > 0)
                {
                    m_instance = objs[0];
                }
                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + "in the scene.");
                }
                if (m_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = string.Format("_{0}", typeof(T).Name);
                    m_instance = obj.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            if (ShouldDontDestroyOnLoad())
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (m_instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    protected virtual bool ShouldDontDestroyOnLoad()
    {
        return false;
    }
}