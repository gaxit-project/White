using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageStates : MonoBehaviour
{
    public enum StageState
    {
        White,
        Black
    };

    [SerializeField] GameObject wGround;
    [SerializeField] GameObject bGround;
    [SerializeField] GameObject wLadder;
    [SerializeField] GameObject bLadder;
    [SerializeField] GameObject wFloat;
    [SerializeField] GameObject bFloat;
    [SerializeField] GameObject wGoal;
    [SerializeField] GameObject bGoal;
    [SerializeField] GameObject wLift;
    [SerializeField] GameObject bLift;
    [SerializeField] GameObject wNeedle;
    [SerializeField] GameObject bNeedle;
    [SerializeField] GameObject wSwitch;
    [SerializeField] GameObject bSwitch;
    [SerializeField] GameObject LightObj;


    private StageState Stage;
    private PlayerMove.SwitchState switchState;

    public StageState CurrentStage => Stage;

    public static StageStates Instance;
    private void Awake()
    {
        Instance = this;
        LightObj.SetActive(false);
    }

    private void Start()
    {
        
        Stage = StageState.White;
        Wgenerate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Reverse();
        }

        if(switchState == PlayerMove.SwitchState.On)
        {
            SwitchOn();
        }
        else
        {
            SwitchOff();
        }
    }

    public void Reverse()
    {
        if (Stage == StageState.White)
        {
            Stage = StageState.Black;
            Bgenerate();
        }
        else
        {
            Stage = StageState.White;
            Wgenerate();
        }
    }

    private void Wgenerate()
    {
        wGround.SetActive(true);
        wLadder.SetActive(true);
        wFloat.SetActive(true);
        wGoal.SetActive(true);
        wLift.SetActive(true);
        wNeedle.SetActive(true);
        wSwitch.SetActive(true);
        bGround.SetActive(false);
        bLadder.SetActive(false);
        bFloat.SetActive(false);
        bGoal.SetActive(false);
        bLift.SetActive(false);
        bNeedle.SetActive(false);
        bSwitch.SetActive(false);

    }

    private void Bgenerate()
    {
        bGround.SetActive(true);
        bLadder.SetActive(true);
        bFloat.SetActive(true);
        bGoal.SetActive(true);
        bLift.SetActive(true);
        bNeedle.SetActive(true);
        bSwitch.SetActive(true);
        wGround.SetActive(false);
        wLadder.SetActive(false);
        wFloat.SetActive(false);
        wGoal.SetActive (false);
        wLift.SetActive (false);
        wNeedle.SetActive (false);
        wSwitch.SetActive (false);
    }

    private void SwitchOn()
    {
        LightObj.SetActive(true);
    }

    private void SwitchOff()
    {
        LightObj.SetActive(false);
    }
}
