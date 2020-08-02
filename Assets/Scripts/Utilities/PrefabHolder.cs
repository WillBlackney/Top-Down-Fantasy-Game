using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabHolder : MonoBehaviour
{
    [Header("Projectile Prefabs")]
    public GameObject arrowPrefab;
    public GameObject fireBallPrefab;

    // Singleton Pattern
    #region
    public static PrefabHolder Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    
}
