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
    public void PlayAnimation()
    {
        StartCoroutine(PlayAnimationOnce());
    }
    public void Update()
    {
        if (GetComponent<NewUnitAttack>().playAnim)
        {
            PlayAnimation();
        }
    }
    public IEnumerator PlayAnimationOnce()
    {
        animator.SetTrigger("active");
        yield return new WaitForSeconds(0.5f);
        animator.ResetTrigger("active");
    }
}
