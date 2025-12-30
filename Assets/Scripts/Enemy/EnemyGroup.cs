using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup : MonoBehaviour, ITriggerObject
{
    private bool _enableGroup = false;
    private int _currentEnemy = 0;
    
    [SerializeField] private List<EnemyInScene> _enemies;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var enemy in _enemies)
        {
            enemy.SetMyGroup(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_enableGroup)
        {
            foreach (var enemy in _enemies)
            {
                enemy.AddTime(Time.deltaTime);
            }
        }
    }

    public void InitEnemy(EnemyBeatController enemy)
    {
        enemy.gameObject.SetActive(true);
        _currentEnemy++;
    }

    public void TriggerByPlayer(CharacterBeatController player)
    {
        _enableGroup = true;
    }
}
