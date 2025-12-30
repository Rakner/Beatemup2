using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public UIController uiController;
    public List<EnemyBeatController> enemies;

    public static GameManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
        enemies = new List<EnemyBeatController>();
    }

    void Start()
    {
        uiController.SetPlayerFace(player.GetComponent<CharacterBeatView>().faceSprite);
        uiController.SetPlayerName(player.GetComponent<CharacterBeatView>().name);
        uiController.SetPlayerHealth(100f);
        GameManager.Instance.uiController.SetEnableEnemyElements(false);
    }

    void Update()
    {
        
    }

    public void PlayerHitted(float normalizedHealth)
    {
        uiController.SetPlayerHealth(normalizedHealth);
    }

    public void EnemyHitted(Sprite face, string enemyName,  float normalizedHealth)
    {
        uiController.SetEnemyFace(face);
        uiController.SetEnemyName(enemyName);
        uiController.SetEnemyHealth(normalizedHealth);
    }

    private void FinishStage()
    {
        Debug.Log("Finish Stage");
    }

    public void GameOver()
    {
        uiController.GameOver();
    }

    public void AddEnemy(EnemyBeatController enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemies(EnemyBeatController enemy)
    {
        enemies.Remove(enemy);
    }
    
}
