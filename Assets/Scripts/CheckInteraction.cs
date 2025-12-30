using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactSprite;
    private bool _playerInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerInRange = true;
            interactSprite.SetActive(true);
            Debug.Log("Interactuable");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerInRange = false;
            interactSprite.SetActive(false);
            Debug.Log("No Interactuable");
        }
    }
}
