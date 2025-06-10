using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public float moveSpeed = 3f;

    protected Vector2 moveInput;
    protected bool isDead = false;
    protected bool isAttacking = false;

    protected virtual void Update()
    {
        if (isDead) return;

        HandleInput();
        HandleMovement();
        HandleAnimation();
    }

    protected abstract void HandleInput(); // Player/Bot xử lý riêng

    protected virtual void HandleMovement()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    protected virtual void HandleAnimation()
    {
        animator.SetBool("IsWalking", moveInput.magnitude > 0.1f);
    }

    // === Các hành động chính ===
    public virtual void Attack()
    {
        if (isDead || isAttacking) return;
        animator.SetTrigger("Punch");
        isAttacking = true;
        Invoke(nameof(ResetAttack), 0.5f); // reset sau khi anim kết thúc
    }

    public virtual void Jump()
    {
        if (isDead) return;
        animator.SetTrigger("Jump");
    }

    public virtual void TakeDamage()
    {
        if (isDead) return;
        animator.SetTrigger("Hit");
    }

    public virtual void Knockout()
    {
        if (isDead) return;
        animator.SetTrigger("Knockout");
        isDead = true;
    }

    public virtual void Celebrate()
    {
        animator.SetTrigger("Victory");
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}
