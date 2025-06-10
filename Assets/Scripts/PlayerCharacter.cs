using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    protected override void HandleInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.J)) Attack();
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.H)) TakeDamage();
        if (Input.GetKeyDown(KeyCode.K)) Knockout();
        if (Input.GetKeyDown(KeyCode.V)) Celebrate();
    }
}
