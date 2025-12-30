using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class CharacterBeatView : MonoBehaviour
{
    public Sprite faceSprite;
    public string characterName;
    
    public Animator animator;

    void Awake()
    {
        animator =  GetComponent<Animator>();
    }

    public void ChangeAnimatorState(string estado, int i)
    {
        animator.SetInteger(estado, i);
    }

    public void ChangeAnimatorState(string estado, bool i)
    {
        animator.SetBool(estado, i);
    }
}
