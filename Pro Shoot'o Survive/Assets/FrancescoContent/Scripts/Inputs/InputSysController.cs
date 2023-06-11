using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputSysController : Singleton<InputSysController>
{
    [SerializeField] private Vector2 mouseDeltaDir;
    [SerializeField] private Vector2 playerMoveDir;

    private PlayerInputSys playerInputs;
    public Dictionary<string, InputAction> Inputs { get; private set; }

    public Vector2 MouseDeltaDir { get { return mouseDeltaDir; } }
    public Vector2 PlayerMoveDir { get { return playerMoveDir; } }

    // Start is called before the first frame update
    void Awake()
    {
        playerInputs = new PlayerInputSys();
        Inputs = new Dictionary<string, InputAction>();

        InputSystem.settings.maxEventBytesPerUpdate = 0;

        for (int i = 0; i < playerInputs.asset.actionMaps.Count; i++)
        {
            for (int j = 0; j < playerInputs.asset.actionMaps[i].actions.Count; j++)
            {
                Inputs[playerInputs.asset.actionMaps[i].actions[j].name] = playerInputs.asset.actionMaps[i].actions[j];
            }
        }

        //print(p.asset.actionMaps[0].actions[0].name);
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        mouseDeltaDir = Inputs["MouseDeltaDir"].ReadValue<Vector2>();
        //playerMoveDir = Inputs["PlayerMoveDir"].ReadValue<Vector2>();
    }
}
