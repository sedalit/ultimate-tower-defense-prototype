using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletoneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singletone")]
    [SerializeField] private bool m_DontDestroyOnLoad;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("MonoSingleton: object of type already exist, instance wiil be destroyed = " + typeof(T).Name);
            Destroy(this);
            return;
        }

        Instance = this as T;

        if(m_DontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
    
}
