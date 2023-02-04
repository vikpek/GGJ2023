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
    private int gamepadId;
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

    public void HandleMove(InputAction.CallbackContext context)
    {
        //Debug.Log($"HandleMove Trigger. context:{context.valueType}, moveInput:{context.ReadValue<Vector2>()}");
        if (!Application.isFocused)
            return;

        Debug.Log($"Player {playerId} OnMove!");
        if(context.performed)
            OnMove(context.ReadValue<Vector2>()[0]);
        if(context.canceled)
            OnMove(0);
    }
    public void HandleAction(InputAction.CallbackContext context)
    {
        //Debug.Log($"HandleMove Trigger. context:{context.valueType}, moveInput:{context.ReadValue<Vector2>()}");
        if (!Application.isFocused)
            return;
    }
    public void HandleMoveStop(InputAction.CallbackContext context)
    {
        OnMove(0);
    }

    public void InitPlayerInput(string currentControlScheme, int playerindex, int gamepadId, PlayerInput input)
    {
        controlScheme = currentControlScheme;
        this.playerId = playerindex;
        this.playerInput = input;
        this.gamepadId = gamepadId;

        //inputMaster = new InputMaster();
        Debug.Log("Player on enable, gamepadId: " + gamepadId);   
        Debug.Log("currentScheme: " + controlScheme + " actionMap:"+playerInput.currentActionMap);
        //playerInput.ActivateInput();
        //inputMaster.Enable();
        
        // switch (controlScheme)
        // {
        //     case "Keyboard2":
        //         Debug.Log("Switching to Keyboard2");
        //         playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);
        //         break;
        //     case "Keyboard&Mouse":                
        //         Debug.Log("Switching to Keyboard&Mouse");
        //         playerInput.SwitchCurrentControlScheme(controlScheme, Keyboard.current);
        //         break;
        //     case "Gamepad":
        //         Debug.Log("Switching to Gamepad");
        //         playerInput.SwitchCurrentControlScheme(controlScheme, Gamepad.all[gamepadId]);
        //         Debug.Log("Gamepad: " +Gamepad.all[gamepadId] + " paired: " + playerInput.user.pairedDevices.Count + " pair: " + playerInput.user.pairedDevices[0]);
                
        //         break;
        // }
        // playerInput.neverAutoSwitchControlSchemes = true;
        
        // inputMaster.Player.Move.performed += HandleMove; //TODO WASD
        // inputMaster.Player.Action.performed += HandleAction;

        // //Keyboard
        // inputMaster.Player.Move.canceled += HandleMoveStop;
        Debug.Log("InitPlayerInput");

    }
}
