using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    public Animator animator;
    public float comboResetTime = 1.0f;

    private bool isAttacking = false;
    private float lastAttackTime;
    private int attackCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!isAttacking)
            {
                attackCount = 1;
                lastAttackTime = Time.time;
                Attack();
            }
            else
            {
                if (Time.time - lastAttackTime <= comboResetTime)
                {
                    attackCount++;
                }
                else
                {
                    attackCount = 1;
                    lastAttackTime = Time.time;
                    Attack();
                }
            }
        }

        if (isAttacking && Time.time - lastAttackTime > comboResetTime)
        {
            attackCount = 0;
            isAttacking = false;
        }
    }

    void Attack()
    {
        if (animator == null)
        {
            return;
        }

        if (attackCount == 1)
        {
            animator.SetTrigger("PlayerAttackStart");
            StartCoroutine(PlayAttackSequence(new string[] { "PlayerAttackEnd" }));
        }
        else if (attackCount > 1)
        {
            animator.SetTrigger("PlayerAttackStart");
            StartCoroutine(PlayAttackSequence(new string[] { "PlayerAttackLoop", "PlayerAttackEnd" }));
        }

        isAttacking = true;
    }

    private System.Collections.IEnumerator PlayAttackSequence(string[] animations)
    {
        foreach (string anim in animations)
        {
            animator.SetTrigger(anim);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(stateInfo.length);
        }

        isAttacking = false;
        attackCount = 0;
    }
}
