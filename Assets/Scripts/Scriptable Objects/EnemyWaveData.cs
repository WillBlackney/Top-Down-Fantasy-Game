using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Wave Data", menuName = "EnemyWaveData", order = 53)]
public class EnemyWaveData : ScriptableObject
{
    public int waveLevel;
    public WaveType waveType;
    public List<SpawnEvent> spawnEvents;
}
public enum WaveType
{
    Basic,
    Elite,
    Boss
}
