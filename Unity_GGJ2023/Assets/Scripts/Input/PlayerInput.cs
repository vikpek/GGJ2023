using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{

    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float currentSpeed = 0.0f;
    [SerializeField] private AnimationCurve SpeedAcc;

    public Action<float> OnMove = delegate { };

    private InputMaster inputMaster;

    private string name = "";

    // Start is called before the first frame update
    void Start()
    {
    }

    void Awake()
    {
        inputMaster = new InputMaster();
        name = gameObject.GetInstanceID().ToString();
        Debug.Log("Created InputMaster " + name);
    }

    private void OnEnable()
    {
        inputMaster.Enable();
        inputMaster.Player.Move.performed += HandleMove; //TODO WASD
        inputMaster.Player.Action.performed += HandleAction;
        Debug.Log("OnEnable");
    }

    private void OnDisable()
    {
        inputMaster.Disable();
        inputMaster.Player.Move.performed -= HandleMove;
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
}
