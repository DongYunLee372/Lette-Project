using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public AnimationController animator;
    public RuntimeAnimatorController stateanimations;

    void Start()
    {
        animator = GetComponent<AnimationController>();
    }

    bool flag = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            
            animator.Play(stateanimations);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {

            flag = !flag;
            animator.SetBool("Run", flag);

        }
        

    }
}
