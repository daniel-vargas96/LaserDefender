using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0; //(Sets startingWave to Wave2 Waveconfig i.e. element 0 in Player Spawner gameobject)
    [SerializeField] bool looping = false;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }



    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++) //GetNumberofEnemies [waveConfig.cs method to get number of enemies]
        {
            var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(), //GetEnemyPrefab [waveConfig.cs method to instantiate enemy]
                waveConfig.GetWaypoints()[0].transform.position, //GetWaypoints [waveConfig.cs method to get waypoints], [0] refers to the startingindex number which will be incremented
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns()); //runs the GetTimeBetweenSpawns method which returns the time for WaitForSeconds to use.
        }
    }
}
