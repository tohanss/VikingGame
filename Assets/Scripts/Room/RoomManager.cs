using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class RoomManager : MonoBehaviour
{

    // Enemy related
    private int NumberOfEnemies;
    [Tooltip("The parent to all enemies that are in the room from the start")]
    [SerializeField] private GameObject listOfEnemies;

    // Event related
    public event EventHandler OnStartingEnemiesDead;
    public event EventHandler OnBattleStarted;
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
        NumberOfEnemies = listOfEnemies.transform.childCount;
    }

    private void RoomManager_OnStartingEnemiesDead(object sender, System.EventArgs e)
    {
            startWaveBattle();
            OnStartingEnemiesDead -= OnStartingEnemiesDead;
    }

    // Update is called once per frame
    void Update()
    {
        if (NumberOfEnemies == 0 && state == State.Normal)
        {
            OnStartingEnemiesDead?.Invoke(this, EventArgs.Empty);
        }
    }
    private void startWaveBattle()
    {
        Debug.Log("Starting Waves");
        state = State.WaveActive;
        OnBattleStarted?.Invoke(this, EventArgs.Empty);
    }
}
