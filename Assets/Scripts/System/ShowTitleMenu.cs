using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowTitleMenu : MonoBehaviour
{
    public static ShowTitleMenu TitleInstance;

    [SerializeField] private Button Startbutton;
    [SerializeField] private Button Option;
    [SerializeField] private Button Quit;
    [SerializeField] private TextMeshProUGUI Push;

    private void Awake()
    {
        TitleInstance = this;
        Startbutton.gameObject.SetActive(false);
        Option.gameObject.SetActive(false);
        Quit.gameObject.SetActive(false);
        Push.gameObject.SetActive(true);
    }
    public void ShowTitle()
    {
        bool active = !Startbutton.gameObject.activeSelf;
        Startbutton.gameObject.SetActive(active);
        Option.gameObject.SetActive(active);
        Quit.gameObject.SetActive(active);
        Push.gameObject.SetActive(!active);
        Debug.Log("showtitle");
    }
}
