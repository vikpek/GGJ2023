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
    [SerializeField] private float curveSteps = 0.01f;
    [SerializeField] private float curveDuration = 10f;
    public Action OnAction = delegate { };
    public Action<float> OnMove = delegate { };

    private float curvePointer = 0.0f;
    private float animationCurveValue = 0.0f;
    private bool inSpeedUp = false;
    private bool inSlowDown = false;
    private bool isRunningLeft = false;

    private int playerId = 0;

    private int gamepadId;

    private InputMaster inputMaster;
    private PlayerInput playerInput;

    private Coroutine speedUpCoroutine;
    private Coroutine slowDownCoroutine;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void HandleMove(InputAction.CallbackContext context)
    {
        if (!Application.isFocused)
            return;

        if (context.performed)
        {
            isRunningLeft = context.ReadValue<Vector2>()[0] < 0f;
            if (!inSpeedUp && context.ReadValue<Vector2>()[0] != 0f)
            {
                Debug.Log("Starting Acceleration with " + curvePointer + " curvePointer");
                speedUpCoroutine = StartCoroutine("AccelerationFade");
                return;
            }

            if (!inSlowDown && context.ReadValue<Vector2>()[0] == 0f)
            {
                slowDownCoroutine = StartCoroutine("SlowDownFade");
            }
        }
        if (context.canceled)
        {
            if (!inSlowDown && context.ReadValue<Vector2>()[0] == 0f)
            {
                slowDownCoroutine = StartCoroutine("SlowDownFade");
            }
        }
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

    IEnumerator AccelerationFade()
    {     
        Debug.Log("AccelerationFade entry, curvePointer: " + curvePointer);
        if (slowDownCoroutine != null)
        {
            StopCoroutine(slowDownCoroutine);
            inSlowDown = false;
        }

        if (inSpeedUp)
        {
            Debug.Log("AccelerationFade break");
            yield break;
        }
        inSpeedUp = true;
        float elapsedTime = 0f;
        float multiplier = isRunningLeft ? -1f : 1f;
        while (elapsedTime < curveDuration)
        {
            //Debug.Log("AccelerationFade while, curvePointer: " + curvePointer + ", animationCurveValue: " + animationCurveValue);

            elapsedTime += Time.deltaTime; //fixedDelta?
            Debug.Log("AccelerationFade while, elapsed Time:" + elapsedTime + " curveDuration: " + curveDuration + " /= " + (elapsedTime / curveDuration));
            curvePointer += (Time.deltaTime / curveDuration);
            animationCurveValue = SpeedAcc.Evaluate(curvePointer);
            OnMove(animationCurveValue * multiplier);
            if (curvePointer == 1)
            {
                inSpeedUp = false;
                yield break;
            }
            yield return null;
        }
        curvePointer = 1f;
        inSpeedUp = false;
    }

    IEnumerator SlowDownFade()
    {
        
        if (speedUpCoroutine != null)
        {
            StopCoroutine(speedUpCoroutine);
            inSpeedUp = false;
        }

        if (inSlowDown)
        {
            Debug.Log("SlowDownFade break");
            yield break;
        }
        inSlowDown = true;
        float elapsedTime = 0f;
        float multiplier = isRunningLeft ? -1f : 1f;
        while (elapsedTime < curveDuration)
        {
            //Debug.Log("AccelerationFade while, curvePointer: " + curvePointer + ", animationCurveValue: " + animationCurveValue);

            elapsedTime += Time.deltaTime; //fixedDelta?
            Debug.Log("SlowDownFade while, elapsed Time:" + elapsedTime + " curveDuration: " + curveDuration + " /= " + (elapsedTime / curveDuration));
            curvePointer -= (Time.deltaTime / curveDuration);
            animationCurveValue = SpeedAcc.Evaluate(curvePointer);
            OnMove(animationCurveValue * multiplier);
            if (curvePointer == 0)
            {
                inSlowDown = false;
                yield break;
            }
            yield return null;
        }
        curvePointer = 0f;
        inSlowDown = false;
    }
}