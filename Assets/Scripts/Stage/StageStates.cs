using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private Button button;
    [SerializeField] private Color whiteStageColor = Color.black;
    [SerializeField] private Color blackStageColor = Color.white;


    private StageState Stage;

    public StageState CurrentStage => Stage;

    public static StageStates Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Stage = StageState.White;
        clearTimeText.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        Wgenerate();
        SwitchOff();
    }

    // Switchの状態チェックはPlayerMoveからのアクション呼び出しに任せるため、Updateから削除
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Reverse();
        }

        // 旧コード: PlayerMove.OnSwitchの状態を常にチェックしていた
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

    // スイッチの状態をトグル（反転）する公開メソッド
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

    public void ShowClearUI(int clearTime)
    {
        if (clearTimeText == null)
        {
            Debug.LogError("Clear Time Text UIがStageStatesに設定されていません。");
            return;
        }

        // 1. テキストの色を設定
        if (Stage == StageState.White)
        {
            clearTimeText.color = whiteStageColor;
        }
        else // StageState.Black
        {
            clearTimeText.color = blackStageColor;
        }

        // 2. テキストの内容を設定
        clearTimeText.text = "Stage Clear!";

        // 3. UIを表示
        clearTimeText.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
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