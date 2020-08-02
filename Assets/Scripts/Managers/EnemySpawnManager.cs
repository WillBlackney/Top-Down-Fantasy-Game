using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemySpawnManager : Singleton<EnemySpawnManager>
{
    // Properties + Component References
    #region
    [Header("General Properties")]
    public int currentStage = 1;
    public bool activelySpawning;
    public EnemyWaveData testingWave;
    public List<EnemyWaveData> allWaveData;
    public List<SpawnEvent> currentWaveSpawnEvents;
    public List<Enemy> allEnemies;

    [Header("Spawn Location Properties")]
    public Transform[] spawnLocations;
    public Transform northSpawn;
    public Transform southSpawn;
    public Transform eastSpawn;
    public Transform westSpawn;
    public Transform northEastSpawn;
    public Transform northWestSpawn;
    public Transform southWestSpawn;
    public Transform southEastSpawn;
    #endregion
       
    // Get Data
    #region
    public Transform GetRandomSpawnLocation()
    {
        return spawnLocations[Random.Range(0, spawnLocations.Length)];
    }
    public EnemyWaveData GetNextValidEnemyWave()
    {
        List<EnemyWaveData> validWaves = new List<EnemyWaveData>();

        foreach (EnemyWaveData wave in allWaveData)
        {
            if (wave.waveLevel == currentStage)
            {
                validWaves.Add(wave);
            }
        }
        return validWaves[Random.Range(0, validWaves.Count)];
    }
    public Transform GetSpawnLocationFromSpawnEvent(SpawnEvent se)
    {
        Transform locationReturned = null;

        if (se.spawnLocation == SpawnLocation.Any)
        {
            locationReturned = GetRandomSpawnLocation();
        }
        else if (se.spawnLocation == SpawnLocation.North)
        {
            locationReturned = northSpawn;
        }
        else if (se.spawnLocation == SpawnLocation.South)
        {
            locationReturned = southSpawn;
        }
        else if (se.spawnLocation == SpawnLocation.East)
        {
            locationReturned = eastSpawn;
        }
        else if (se.spawnLocation == SpawnLocation.West)
        {
            locationReturned = westSpawn;
        }
        else if (se.spawnLocation == SpawnLocation.NorthEast)
        {
            locationReturned = northEastSpawn;
        }
        else if (se.spawnLocation == SpawnLocation.NorthWest)
        {
            locationReturned = northWestSpawn;
        }
        else if (se.spawnLocation == SpawnLocation.SouthEast)
        {
            locationReturned = southEastSpawn;
        }
        else if (se.spawnLocation == SpawnLocation.SouthWest)
        {
            locationReturned = southWestSpawn;
        }

        return locationReturned;
    }
    #endregion

    // Spawn Enemy Waves Logic
    #region
    public void SpawnEnemy(GameObject enemyPrefab, Vector3 location)
    {
        Vector3 spawnLocation = location;
        Enemy enemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity).GetComponent<Enemy>();
        enemy.InitializeSetup();
        AddEnemyToAllEnemiesList(enemy);
    }      
    public void SpawnEnemyWave(EnemyWaveData wave)
    {
        StartCoroutine(SpawnEnemyWaveCoroutine(wave));
    }
    private IEnumerator SpawnEnemyWaveCoroutine(EnemyWaveData wave)
    {
        currentWaveSpawnEvents.Clear();
        currentWaveSpawnEvents.AddRange(wave.spawnEvents);
        activelySpawning = true;

        foreach(SpawnEvent se in currentWaveSpawnEvents)
        {
            float spawnDelay = Random.Range(se.preSpawnDelayMin, se.preSpawnDelayMax);
            yield return new WaitForSeconds(spawnDelay);
            Transform spawnLocation = GetSpawnLocationFromSpawnEvent(se);
            SpawnEnemy(se.enemyPrefab, spawnLocation.position);

            // is this the last enemy to spawn in the wave?
            if(currentWaveSpawnEvents.Last() == se)
            {
                activelySpawning = false;
            }
        }
    }
    #endregion

    // Misc Logic
    #region
    public void AddEnemyToAllEnemiesList(Enemy enemy)
    {
        allEnemies.Add(enemy);
    }
    public void RemoveEnemyFromAllEnemiesList(Enemy enemy)
    {
        allEnemies.Remove(enemy);
    }
    public void ModifyCurrentStage(int stageGainedOrLost)
    {
        currentStage += stageGainedOrLost;

        // TO DO: update text+gui related to current wave
    }
    public void SetCurrentStage(int newStage)
    {
        currentStage = newStage;

        // TO DO: update text+gui related to current wave
    }
    public void DestroyAllEnemies()
    {
        if(allEnemies.Count > 1)
        {
            // Reverse for loop to safely iterate over all enemies while destroying them
            for (int i = allEnemies.Count - 1; i >= 0; i--)
            {
                Destroy(allEnemies[i]);
            }
        }
        else if(allEnemies.Count == 1)
        {
            Destroy(allEnemies[0]);
        }      

        // Flush list
        allEnemies.Clear();
    }
    public void ResetToStartSettings()
    {
        DestroyAllEnemies();
        SetCurrentStage(1);
    }
    #endregion

}
