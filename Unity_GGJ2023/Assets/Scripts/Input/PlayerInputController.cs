using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private AnimationCurve SpeedAcc;
    public Action OnAction = delegate { };
    public Action<float> OnMove = delegate { };

    private float curvePointer = 0.0f;
    private float animationCurveValue = 0.0f;
    private bool inSpeedUp = false;
    private bool inSlowDown = false;
    private bool isRunningLeft = false;
    private float speedUpTimer = 0.0f;

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
        float elapsedTime = curvePointer*Configs.Instance.Get.speedFadeTime;
        float multiplier = isRunningLeft ? -1f : 1f;
        while (elapsedTime < Configs.Instance.Get.speedFadeTime)
        {
            //Debug.Log("AccelerationFade while, curvePointer: " + curvePointer + ", animationCurveValue: " + animationCurveValue);
            elapsedTime += Time.deltaTime; //fixedDelta?
            //Debug.Log("AccelerationFade while, elapsed Time:" + elapsedTime + " curveDuration: " + Configs.Instance.Get.speedFadeTime + " /= " + (elapsedTime / curveDuration));
            curvePointer += (Time.deltaTime / Configs.Instance.Get.speedFadeTime);
            animationCurveValue = SpeedAcc.Evaluate(curvePointer);
            OnMove(animationCurveValue * multiplier);
            if (curvePointer == 1f)
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
        float elapsedTime = Configs.Instance.Get.speedFadeTime - curvePointer*Configs.Instance.Get.speedFadeTime;
        float multiplier = isRunningLeft ? -1f : 1f;
        while (elapsedTime < Configs.Instance.Get.speedFadeTime)
        {
            //Debug.Log("AccelerationFade while, curvePointer: " + curvePointer + ", animationCurveValue: " + animationCurveValue);
            elapsedTime += Time.deltaTime; //fixedDelta?
            //Debug.Log("SlowDownFade while, elapsed Time:" + elapsedTime + " curveDuration: " + Configs.Instance.Get.speedFadeTime + " /= " + (elapsedTime / Configs.Instance.Get.speedFadeTime));
            curvePointer -= (Time.deltaTime / Configs.Instance.Get.speedFadeTime);
            animationCurveValue = SpeedAcc.Evaluate(curvePointer);
            OnMove(animationCurveValue * multiplier);
            if (curvePointer == 0f)
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