using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffButton : MonoBehaviour
{
    // Start is called before the first frame update
    private void PressedStartButton()
    {
        gameObject.SetActive(false);
    }
}
