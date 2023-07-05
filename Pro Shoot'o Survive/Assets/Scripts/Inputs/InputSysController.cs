using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSysController
{
    private PlayerInputSys playerInputs;
    private Dictionary<string, InputAction> inputs;

    // Start is called before the first frame update
    public InputSysController()
    {
        playerInputs = new PlayerInputSys();
        inputs = new Dictionary<string, InputAction>();

        InputSystem.settings.maxEventBytesPerUpdate = 0;

        for (int i = 0; i < playerInputs.asset.actionMaps.Count; i++)
        {
            for (int j = 0; j < playerInputs.asset.actionMaps[i].actions.Count; j++)
            {
                inputs[playerInputs.asset.actionMaps[i].actions[j].name] = playerInputs.asset.actionMaps[i].actions[j];
            }
        }

        EnableInputStsyem();
    }

    public void EnableInputStsyem()
    {
        playerInputs.Enable();
    }

    public void DisableInputSystem()
    {
        playerInputs.Disable();
    }

    public bool OnInputTrigger(string inputActionName)
    {
        return inputs[inputActionName].triggered;
    }

    public bool OnInputPressed(string inputActionName)
    {
        return inputs[inputActionName].IsPressed();
    }

    public T GetInputValue<T>(string inputActionName) where T : struct
    {
        return inputs[inputActionName].ReadValue<T>();
    }
}
