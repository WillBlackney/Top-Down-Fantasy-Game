using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Wave Data", menuName = "EnemyWaveData", order = 53)]
public class EnemyWaveData : ScriptableObject
{
    public List<SpawnEvent> spawnEvents;
}
