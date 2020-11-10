using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimScript : MonoBehaviour
{
    public bool activeOnAwake;
    public bool loop;
    public Animation anim;
    public AnimationClip animClip;
    public Animator animator;
    private void Start()
    {
        if (loop)
        {
            animator.SetBool("loop", true);
        }
        if (activeOnAwake)
        {
            animator.SetBool("onAwake", true);
        }
        else
        {
            animator.SetBool("onAwake", false);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            StartCoroutine(PlayAnimationOnce());
        }
    }
    public void PlayAnimation()
    {
        animator.SetTrigger("active");
    }
    public IEnumerator PlayAnimationOnce()
    {
        animator.SetTrigger("active");
        yield return new WaitForSeconds(0.5f);
        animator.ResetTrigger("active");
    }
}
