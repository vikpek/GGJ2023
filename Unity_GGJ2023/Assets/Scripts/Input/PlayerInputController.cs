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
    private float isRunningLeftMultiplier = 1;
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

        //isRunningLeftMultiplier = (context.ReadValue<Vector2>()[0] < 0f) ? -1 : 1; //not working because of edgecase 0
        if(context.ReadValue<Vector2>()[0] > 0f){
            isRunningLeftMultiplier = 1;
        }else if(context.ReadValue<Vector2>()[0] < 0f){
            isRunningLeftMultiplier = -1;
        }

        if (context.performed || context.started)
        {
            if (!inSpeedUp && context.ReadValue<Vector2>()[0] != 0f)
            {
                speedUpCoroutine = StartCoroutine("AccelerationFade");
                return;
            }

            if (!inSlowDown && context.ReadValue<Vector2>()[0] == 0f)
            {
                slowDownCoroutine = StartCoroutine("SlowDownFade");
                return;
            }
            if(animationCurveValue == 1)
                OnMove(isRunningLeftMultiplier);
            
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
        if(elapsedTime >= Configs.Instance.Get.speedFadeTime)
            OnMove(isRunningLeftMultiplier);
          
        while (elapsedTime < Configs.Instance.Get.speedFadeTime)
        {
            elapsedTime += Time.deltaTime; //fixedDelta?
            curvePointer += (Time.deltaTime / Configs.Instance.Get.speedFadeTime);
            animationCurveValue = SpeedAcc.Evaluate(curvePointer);
            OnMove(animationCurveValue * isRunningLeftMultiplier);
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
        if(elapsedTime >= Configs.Instance.Get.speedFadeTime)
            OnMove(isRunningLeftMultiplier);

        while (elapsedTime < Configs.Instance.Get.speedFadeTime)
        {
            elapsedTime += Time.deltaTime; //fixedDelta?
            curvePointer -= (Time.deltaTime / Configs.Instance.Get.speedFadeTime);
            animationCurveValue = SpeedAcc.Evaluate(curvePointer);
            OnMove(animationCurveValue * isRunningLeftMultiplier);
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