using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EnemySpawner : MonoBehaviour
{
  //References
  [SerializeField] private GameObject[] enemyPrefabs;
  
  //Events
  public static UnityEvent onEnemyDestroy = new UnityEvent();
   
   
  //Enemy Atttibutes
  [SerializeField] private int baseEnemy = 4;
  [SerializeField] private float enemiesPerSec = 0.5f;
  [SerializeField] private float timeBtwnWaves = 5f;
  [SerializeField] private float difficultyScaling = 0.75f;
  
  private int currentWave = 1;
  private float timeBtwnSpawn;
  private int aliveEnemies;
  private int enemiesToSpawn;
  private bool isSpawning = false;

  private void Awake()
  {
      onEnemyDestroy.AddListener(OnEnemyDestroyed);
  }


  private void Start()
  {
      StartCoroutine(StartWave());
  }

  private void Update()
  {
      if (!isSpawning) return;
      
      timeBtwnSpawn += Time.deltaTime;

      if (timeBtwnSpawn >= (1f / enemiesPerSec) && enemiesToSpawn > 0)
      {
          SpawnEnemy();
          enemiesToSpawn--;
          aliveEnemies++;
          timeBtwnSpawn = 0f;
      }

      if (aliveEnemies == 0 && enemiesToSpawn == 0)
      {
          EndWave();
      }
  }


  private IEnumerator StartWave()
  {
      yield return new WaitForSeconds(timeBtwnWaves);
      
      isSpawning = true;  
    enemiesToSpawn = EnemiesPerWave();
  }

  private void SpawnEnemy()
  {
      GameObject prefabToSpawn = enemyPrefabs[0];
      Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
  }
  private int EnemiesPerWave()
  { 
      return Mathf.RoundToInt(baseEnemy * Mathf.Pow(currentWave,difficultyScaling));
  }


  private void EndWave()
  {
      isSpawning = false;
      timeBtwnSpawn = 0f;
      StartCoroutine(StartWave());
  }
  private void OnEnemyDestroyed()
  {
      aliveEnemies--;
  }

}
