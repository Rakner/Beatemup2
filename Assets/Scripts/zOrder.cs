using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zOrder : MonoBehaviour
{
    [SerializeField] private Transform anchor;
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anchor == null)
        {
            _spriteRenderer.sortingOrder = (int)(transform.position.y * -10);
        }
        else
        {
            _spriteRenderer.sortingOrder = (int)(anchor.position.y * -10);
        }
    }
}
