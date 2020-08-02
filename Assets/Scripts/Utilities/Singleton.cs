using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    // Classes that inherit from this will implement a 'Lazy Singleton Pattern'

    public static T Instance;

    protected virtual void Awake()
    {
        if (!Instance)
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }


}
