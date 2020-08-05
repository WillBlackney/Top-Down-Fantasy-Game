using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnEvent
{
    public GameObject enemyPrefab;
    public SpawnLocation spawnLocation;
    public float preSpawnDelayMin;
    public float preSpawnDelayMax;
}
public enum SpawnLocation
{
    Any,
    North,
    South,
    East,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}
