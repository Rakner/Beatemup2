using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBeatController : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] protected int maxHealth;
    [SerializeField] protected int currentHealth;
    [SerializeField, Range(0f, 100f)] protected float damagePerHit;
    [SerializeField, Range(0f, 100f)] protected float jumpHeight;

    [SerializeField] protected Transform bottomAnchor;
    [SerializeField] protected Transform hitAnchor;
    [SerializeField] protected Transform topLimit;
    [SerializeField] protected Transform bottomLimit;
    [SerializeField] protected Transform leftLimit;
    [SerializeField] protected Transform rightLimit;

    [SerializeField] protected Vector2 hitSize;

    protected void Awake()
    {
        currentHealth = maxHealth;
    }

    public void AddHealth(int health)
    {
        currentHealth += health;
        currentHealth = Mathf.Clamp(currentHealth, 0, 100);
        Debug.Log(currentHealth);
    }

    public void HealthUpdate()
    {
        float normalizedHealth = currentHealth * 1f / maxHealth * 1f;
        GameManager.Instance.PlayerHitted(normalizedHealth);
        
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(hitAnchor.position, hitSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(bottomAnchor.position, new Vector2(0.1f,0.1f));
    }
}
