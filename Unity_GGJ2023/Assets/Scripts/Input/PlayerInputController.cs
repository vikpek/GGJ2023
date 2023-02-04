using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputController : MonoBehaviour
{

    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float currentSpeed = 0.0f;
    [SerializeField] private AnimationCurve SpeedAcc;

    private int playerId = 0;

    public Action<float> OnMove = delegate { };

    private InputMaster inputMaster;
    private PlayerInput playerInput;

    string controlScheme;

    private string name = "";

    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
        //inputMaster = new InputMaster();
        name = gameObject.GetInstanceID().ToString();
        Debug.Log("Created InputMaster " + name);
        //inputMaster = playerInput.; 

    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        Debug.Log("Player on disable");
        inputMaster.Disable();
        inputMaster.Player.Move.performed -= HandleMove;
        inputMaster.Player.Action.performed -= HandleAction;

        //Keyboard
        inputMaster.Player.Move.canceled -= HandleMoveStop;
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        if (!Application.isFocused)
            return;
        //Debug.Log($"HandleMove Trigger. context:{context.valueType}, moveInput:{context.ReadValue<Vector2>()}");

        OnMove(context.ReadValue<Vector2>()[0]);

    }
    private void HandleAction(InputAction.CallbackContext context)
    {
        //Debug.Log($"HandleMove Trigger. context:{context.valueType}, moveInput:{context.ReadValue<Vector2>()}");
        if (!Application.isFocused)
            return;
    }
    private void HandleMoveStop(InputAction.CallbackContext context)
    {
        OnMove(0);
    }

    public void InitPlayerInput(string currentControlScheme, int playerId, PlayerInput input)
    {
        controlScheme = currentControlScheme;
        this.playerId = playerId;
        this.playerInput = input;

        inputMaster = new InputMaster();
        Debug.Log("Player on enable");
        //playerInput = GetComponent<PlayerInput>();     
        Debug.Log("currentScheme: " + controlScheme + " playerInput: " + playerInput);
        playerInput.ActivateInput();
        inputMaster.Enable();
        switch (controlScheme)
        {
            case "Keyboard2":
                playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);
                break;
            case "Keyboard&Mouse":

                playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);
                break;
            case "Gamepad":
                playerInput.SwitchCurrentControlScheme(controlScheme, Gamepad.all[playerId]);
                break;
        }
        inputMaster.Player.Move.performed += HandleMove; //TODO WASD
        inputMaster.Player.Action.performed += HandleAction;

        //Keyboard
        inputMaster.Player.Move.canceled += HandleMoveStop;
        Debug.Log("InitPlayerInput");

    }
}
