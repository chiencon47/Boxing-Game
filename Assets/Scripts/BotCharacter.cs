using UnityEngine;
using UnityEngine.AI;

public class BotCharacter : CharacterBase
{
    private NavMeshAgent agent;
    private CharacterBase target;

    [Header("Combat Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float disengageDistance = 10f;
    [SerializeField] private float strafeSpeed = 1.5f;
    [SerializeField] private float observeDelay = 0.6f;
    [SerializeField] private float moveUpdateRate = 0.3f;
    [SerializeField] private float targetReevalInterval = 3f;
    [SerializeField] private float flankingProbability = 0.3f;

    private float actionCooldown = 2f;
    private float actionTimer;
    private float observeTimer = 0f;
    private float reevalTimer = 0f;
    private float moveUpdateTimer = 0f;
    private float strafeTimer = 0f;
    private float nextStrafeDelay = 0f;
    private int strafeDirection = 1;

    private Vector3 flankingPosition;
    private bool isFlanking = false;
    private Vector3 lastTargetPosition;

    protected override void Init()
    {
        base.Init();
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Missing NavMeshAgent on " + name);
            return;
        }

        agent.speed = stats.baseMoveSpeed;
        agent.stoppingDistance = attackRange * 0.8f;
        agent.angularSpeed = 360f;
        agent.acceleration = 8f;
        agent.updateRotation = false;

        actionTimer = Random.Range(0f, actionCooldown);
    }

    protected override void HandleInput()
    {
        if (isDead || agent == null) return;

        reevalTimer -= Time.deltaTime;
        if (reevalTimer <= 0f || target == null || target.IsDead)
        {
            FindTarget();
            reevalTimer = targetReevalInterval;
            if (target == null) return;
        }

        FaceTarget();

        observeTimer -= Time.deltaTime;
        if (observeTimer > 0f)
        {
            agent.isStopped = true;
            isMove = false;
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > disengageDistance)
        {
            StopMovement();
            return;
        }

        if (distance > attackRange * 1.2f)
        {
            HandleApproach();
        }
        else
        {
            HandleCombat();
        }

        actionTimer -= Time.deltaTime;
    }

    private void FaceTarget()
    {
        if (target == null) return;

        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 15f);
        }
    }

    private void HandleApproach()
    {
        moveUpdateTimer -= Time.deltaTime;

        if (!isFlanking && Random.value < flankingProbability)
        {
            isFlanking = true;
            flankingPosition = CalculateFlankingPosition();
        }

        Vector3 targetPos = isFlanking ? flankingPosition : target.transform.position;

        if (moveUpdateTimer <= 0f || Vector3.Distance(agent.destination, targetPos) > 0.5f)
        {
            agent.isStopped = false;
            agent.SetDestination(targetPos);
            moveUpdateTimer = moveUpdateRate;
        }

        isMove = true;
    }

    private void HandleCombat()
    {
        agent.isStopped = true;
        isMove = false;

        HandleStrafing();

        if (actionTimer <= 0f)
        {
            Attack();
            actionTimer = actionCooldown * Random.Range(0.8f, 1.2f);
        }
    }

    private void HandleStrafing()
    {
        if (strafeTimer > 0f)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            Vector3 strafeDir = Vector3.Cross(Vector3.up, dirToTarget);
            Vector3 move = strafeDir * strafeDirection * strafeSpeed * Time.deltaTime;

            agent.Move(move);
            strafeTimer -= Time.deltaTime;
        }
        else
        {
            nextStrafeDelay -= Time.deltaTime;
            if (nextStrafeDelay <= 0f)
            {
                strafeDirection = Random.value > 0.5f ? 1 : -1;
                strafeTimer = Random.Range(0.3f, 0.6f);
                nextStrafeDelay = Random.Range(1f, 2.5f);
            }
        }
    }

    private Vector3 CalculateFlankingPosition()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        Vector3 flankDir = Vector3.Cross(Vector3.up, toTarget.normalized) * (Random.value > 0.5f ? 1 : -1);
        Vector3 flankingPos = target.transform.position + (toTarget.normalized + flankDir) * attackRange * 1.5f;

        if (NavMesh.SamplePosition(flankingPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return target.transform.position + toTarget.normalized * attackRange;
    }

    private void FindTarget()
    {
        target = CharacterManager.Instance.GetClosestEnemy(transform.position, team);

        if (target != null)
        {
            lastTargetPosition = target.transform.position;
            observeTimer = observeDelay;
            isFlanking = false;
        }
    }

    private void StopMovement()
    {
        if (!agent.isStopped)
            agent.SetDestination(transform.position);

        isMove = false;
        isFlanking = false;
    }
}
