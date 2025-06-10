using UnityEngine;

public class BotCharacter : CharacterBase
{
    private float actionTimer;

    protected override void HandleInput()
    {
        // Tự di chuyển
        moveInput = new Vector2(Mathf.Sin(Time.time), Mathf.Cos(Time.time));

        // Hành động ngẫu nhiên
        actionTimer -= Time.deltaTime;
        if (actionTimer <= 0)
        {
            int rand = Random.Range(0, 4);
            if (rand == 0) Attack();
            else if (rand == 1) Jump();
            else if (rand == 2) TakeDamage();
            else if (rand == 3) Knockout();

            actionTimer = Random.Range(1f, 3f);
        }
    }
}
