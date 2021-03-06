﻿#region Using Statements
using UnityEngine;
using System.Collections;
#endregion

public enum GameState
{
    Running = 0x1,
    Paused = 0x2
}


public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager _Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            if (_instance == null)
                Debug.LogError("There is no Game Manager on the Scene");

            return _instance;
        }
    }
    #endregion


    public GameState MGameState { private set; get; }
    public float MGameTime { private set; get; }

    [SerializeField]
    private RouletteWheel m_wheel;

    public Weapon[] AllWeapons;
    public Weapon[] AvailableWeapons;


    void Awake()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
    }

    void Start()
    {
        MGameState = GameState.Running;
    }

    void Update()
    {
        MGameTime = MGameState == GameState.Running ? Time.deltaTime : 0f;
    }

    public void PauseGame()
    {
        MGameState = GameState.Paused;
    }

    public void UnPauseGame()
    {
        MGameState = GameState.Running;
    }

    public void ResetGame()
    {
        UnPauseGame();

        m_wheel.Reset();

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach(Enemy e in enemies)
            e.Reset();
    }
}
