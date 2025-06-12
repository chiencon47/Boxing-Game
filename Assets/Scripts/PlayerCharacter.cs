using UnityEngine;

public class PlayerCharacter : CharacterBase
{
    private Joystick joystick;

    private void Start()
    {
        joystick = GameController.Instance.joystick;
    }

    protected override void HandleInput()
    {
        Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
        isMove = direction.magnitude > 0f;
        if (isMove)
        {
            MoveCharacter(direction.normalized);
        }
    }
}

