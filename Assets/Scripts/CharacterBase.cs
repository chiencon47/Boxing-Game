using UnityEngine;
using UnityEngine.AI;
public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected int team;
    protected CharacterStats stats;
    protected Animator animator;
    protected Vector2 moveInput;
    protected bool isDead = false;
    protected bool isAttacking = false;
    protected bool isMove = false;
    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        stats = new CharacterStats();
        animator = GetComponent<Animator>();
        stats.InitStats();
        CharacterManager.Instance.Register(this);
    }
    protected virtual void Update()
    {
        if (isDead) return;

        HandleInput();
        HandleAnimation();
    }

    protected abstract void HandleInput();

    protected void MoveCharacter(Vector2 direction)
    {
        Vector3 move = new Vector3(direction.x, 0, direction.y) * stats.baseMoveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        if (move != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), 0.2f);
        }
    }

    protected virtual void HandleAnimation()
    {
        animator.SetBool("IsWalking", isMove);
    }

    public virtual void Attack()
    {
        if (isDead || isAttacking) return;
        animator.SetTrigger("Punch");
        isAttacking = true;
        Invoke(nameof(ResetAttack), 0.5f);
    }

    public virtual void Jump()
    {
        if (isDead) return;
        animator.SetTrigger("Jump");
    }

    public virtual void TakeDamage(float idAtaack, int damage)
    {
        if (isDead) return;
        animator.SetFloat("AnimID", idAtaack);
        animator.SetTrigger("Hit");
        stats.currentHP -= damage;
        if (stats.currentHP <= 0)
        {
            Knockout();
        }
    }

    public virtual void Knockout()
    {
        if (isDead) return;
        animator.SetTrigger("Knockout");
        isDead = true;
        CharacterManager.Instance.Unregister(this);
    }

    public virtual void Celebrate()
    {
        animator.SetTrigger("Victory");
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public void SetLevel(int level)
    {
        stats.level = level;
        stats.InitStats();
    }

    public int GetTeam()
    {
        return team;
    }

    public void SetTeam(int team)
    {
        this.team = team;
    }

    public bool IsDead => isDead;
}
