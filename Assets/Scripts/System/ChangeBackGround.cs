using UnityEngine;

public class ChangeBackGround : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        var currentStage = StageStates.Instance.CurrentStage;

        if (currentStage == StageStates.StageState.White)
        {
            cam.backgroundColor = Color.white;
        }
        else
        {
            cam.backgroundColor = Color.black;
        }
    }
}
