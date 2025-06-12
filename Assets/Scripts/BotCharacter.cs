using UnityEngine;
using UnityEngine.AI;

public class BotCharacter : CharacterBase
{
    private NavMeshAgent agent;
    private CharacterBase target;
    private float actionCooldown = 2f;
    private float actionTimer;

    [SerializeField] private float attackRange = 2f;

    protected override void Init()
    {
        base.Init();

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = stats.baseMoveSpeed;
            agent.stoppingDistance = attackRange * 0.9f;
        }
        else
        {
            Debug.LogError($"BotCharacter on {name} is missing NavMeshAgent!");
        }
    }

    protected override void HandleInput()
    {
        if (isDead || agent == null) return;

        if (target == null || target.IsDead)
        {
            FindTarget();
            if (target == null) return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        agent.SetDestination(target.transform.position);
        isMove = agent.velocity.magnitude > 0.1f;

        if (distance <= attackRange)
        {
            agent.isStopped = true;

            if (actionTimer <= 0f)
            {
                Attack();
                actionTimer = actionCooldown;
            }
        }
        else
        {
            agent.isStopped = false;
        }

        // Quay mặt về phía mục tiêu
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        }

        actionTimer -= Time.deltaTime;
    }

    private void FindTarget()
    {
        target = CharacterManager.Instance.GetClosestEnemy(transform.position, team);
    }

}
