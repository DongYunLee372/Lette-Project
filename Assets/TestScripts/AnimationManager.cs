using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField]
    AnimationClip[] ani;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        ani = animator.runtimeAnimatorController.animationClips;
    }

    public void play(string name)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
