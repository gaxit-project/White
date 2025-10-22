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
    [SerializeField] GameObject Lightbg;
    [SerializeField] GameObject LightObj;


    private StageState Stage;
    // private PlayerMove.SwitchState switchState; // ���g�p�̂��߃R�����g�A�E�g�܂��͍폜

    public StageState CurrentStage => Stage;

    public static StageStates Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        Stage = StageState.White;
        Wgenerate();
        SwitchOff();
    }

    // Switch�̏�ԃ`�F�b�N��PlayerMove����̃A�N�V�����Ăяo���ɔC���邽�߁AUpdate����폜
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Reverse();
        }

        // ���R�[�h: PlayerMove.OnSwitch�̏�Ԃ���Ƀ`�F�b�N���Ă���
        /*
        if(PlayerMove.OnSwitch == true)
        {
            SwitchOn();
        }
        else
        {
            SwitchOff();
        }
        */
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

    // �X�C�b�`�̏�Ԃ��g�O���i���]�j������J���\�b�h
    public void ToggleSwitch()
    {
        if (LightObj.activeSelf)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
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
        wGoal.SetActive(false);
        wLift.SetActive(false);
        wNeedle.SetActive(false);
        wSwitch.SetActive(false);
    }

    private void SwitchOn()
    {
        LightObj.SetActive(true);
        Lightbg.SetActive(true);
    }

    private void SwitchOff()
    {
        LightObj.SetActive(false);
        Lightbg.SetActive(false);
    }
}