using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField] private int health;
    
    public override void HitByPlayer(float damage, CharacterBeatController player)
    {
        base.HitByPlayer(damage, player);
        
        player.AddHealth(health);
        player.HealthUpdate();
    }
    
    public override void ExecuteAction()
    {
        base.ExecuteAction();
    }
    
    public override void ExitAction()
    {
        base.ExitAction();
        //Destroy(gameObject);
    }
}
