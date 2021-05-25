using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RoomManager : MonoBehaviour
{

    /*
     * Represents a single Enemy Wave
     */
    [System.Serializable]
    public class Wave
    {
        public string name;
        public int numberOfBoars; //number of boars to spawn
        public int numberOfTrolls; //number of trolls to spawn
        public int numberOfElves; //number of elves to spawn
    }

    // Wave and enemy related
    private int numberOfEnemies;
    private int waveIndex;
    private int numberOfWaves;

    // Enemy references
    [Tooltip("The parent to all enemies that are in the room from the start")]
    [SerializeField] private GameObject listOfEnemies;
    public Transform boar;
    public Transform troll;
    public Transform elf;
    public Transform[] enemyPortals;
    public Wave[] waves;

    // Event related
    public event EventHandler OnStartingEnemiesDead;
    //public event EventHandler OnBattleStarted;
    public event EventHandler OnBattleOver;

    // State related
    private State state;

    //Different states the room can be in
    private enum State
    {
        Normal,
        WaveActive,
        BattleOver,
    }

    private void Awake()
    {
        state = State.Normal;
    }
    // Start is called before the first frame update
    void Start()
    {
        OnStartingEnemiesDead += RoomManager_OnStartingEnemiesDead;
        numberOfEnemies = listOfEnemies.transform.childCount;
        waveIndex = 0;
        numberOfWaves = waves.Length;

    }

    //start wavebattle event
    private void RoomManager_OnStartingEnemiesDead(object sender, System.EventArgs e)
    {
            startWaveBattle();
            OnStartingEnemiesDead -= OnStartingEnemiesDead;
    }
    private void startWaveBattle()
    {
        state = State.WaveActive;
        //OnBattleStarted?.Invoke(this, EventArgs.Empty); //close entry portal
    }

    // Update is called once per frame
    void Update()
    {   
        //starts wavebattle if all starting enemies is dead
        if (numberOfEnemies == 0 && state == State.Normal)
        {
            OnStartingEnemiesDead?.Invoke(this, EventArgs.Empty); 
        }

        //start spawning waves from list and also check if wave is over to start next
        if (state == State.WaveActive && isWaveOver())      
        {
            if (!isBattleOver()) //check if we have reached our number of waves for the room
            {
                StartCoroutine(spawnWave(waves[waveIndex]));
                waveIndex++;
            }
        }
        // Victory, wavebattle is over. Open exit portal
        if(state == State.BattleOver)
        {
            OnBattleOver?.Invoke(this, EventArgs.Empty); 
        }

        // FOR DEMO PURPOSE TURN ON ROOM TRANSFER in newExitPortal
        if (Input.GetKeyDown("p") && transform.GetChild(2).CompareTag("ExitPortal"))
        {
            transform.GetChild(2).transform.GetChild(1).gameObject.SetActive(true);
        }
    }
   
    //handles check if battle is over
    private bool isBattleOver()
    {
        if(waveIndex < numberOfWaves)
        {
            return false;
        }
        state = State.BattleOver;
        return true;
    }

    //handles check if wave is over
    private bool isWaveOver()
    {
        if (numberOfEnemies > 0)
        {
            return false;
        }
        numberOfEnemies = 0;
        return true;

    }
    //handles spawning of one enemy. Spawns the enemy in parent gameObject
    private void spawnEnemy(Transform _enemy, GameObject parent)
    {
        Transform spawnPoint = enemyPortals[UnityEngine.Random.Range(0, enemyPortals.Length)];
        var enemy = Instantiate(_enemy, spawnPoint.position + (Vector3)UnityEngine.Random.insideUnitCircle*1.5f, Quaternion.identity);
        enemy.transform.parent = parent.transform;
    }

    //handles the wave spawning. Sets the numberOfEnemies for the current wave and also creates wave parent gameObject for enemies to spawn in.
    private IEnumerator spawnWave(Wave _wave)
    {
        numberOfEnemies = _wave.numberOfBoars + _wave.numberOfTrolls + _wave.numberOfElves;
        var waveGO = new GameObject();
        waveGO.transform.parent = gameObject.transform;
        waveGO.name = _wave.name;

        for (int i = 0; i < _wave.numberOfBoars; i++)
        {
            spawnEnemy(boar, waveGO);
            yield return new WaitForSeconds(0.2f);
        }

        for (int i = 0; i < _wave.numberOfTrolls; i++)
        {
            spawnEnemy(troll, waveGO);
            yield return new WaitForSeconds(0.2f);
        }

        for (int i = 0; i < _wave.numberOfElves; i++)
        {
            spawnEnemy(elf, waveGO);
            yield return new WaitForSeconds(0.2f);
        }

        yield break;
    }

    // This function is called from Enemy-script when an enemy dies
    public void decrementNumberOfEnemies()
    {
        numberOfEnemies--;
    }
}
