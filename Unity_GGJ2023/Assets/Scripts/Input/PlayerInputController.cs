using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float currentSpeed = 0.0f;
    [SerializeField] private AnimationCurve SpeedAcc;

    private int playerId = 0;

    public Action<float> OnMove = delegate { };
    private int gamepadId;
    public Action OnAction = delegate { };

    private InputMaster inputMaster;
    private PlayerInput playerInput;

    public void HandleMove(InputAction.CallbackContext context)
    {
        if (!Application.isFocused)
            return;

        if (context.performed)
            OnMove(context.ReadValue<Vector2>()[0]);
        if (context.canceled)
            OnMove(0);
    }
    public void HandleAction(InputAction.CallbackContext context)
    {
        //Debug.Log($"HandleMove Trigger. context:{context.valueType}, moveInput:{context.ReadValue<Vector2>()}");
        if (!Application.isFocused)
            return;

        OnAction();
    }

    public void InitPlayerInput(int playerindex, int gamepadId)
    {
        this.playerId = playerindex;
        this.gamepadId = gamepadId;
    }
}