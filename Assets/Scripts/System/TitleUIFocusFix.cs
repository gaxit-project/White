using UnityEngine;
using UnityEngine.EventSystems;

public class TitleUIFocusFix : MonoBehaviour
{
    public GameObject firstSelectButton;
    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectButton);
        }
    }
}
