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
    private string animName;

    private void Start()
    {
        animName = anim.clip.ToString();
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
    public void PlayAnimation(string Name)
    {
        animator.SetBool("active", true);
        animator.Play(Name, 0);
    }
    public IEnumerator PlayAnimationOnce()
    {
        animator.SetBool("active", true);
        animator.Play("idle", 0);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("active", false);
    }
}
