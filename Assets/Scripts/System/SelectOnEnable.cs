using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnEnable : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;

    private void OnEnable()
    {
        // 一旦クリア
        EventSystem.current.SetSelectedGameObject(null);

        // 表示された1フレーム後に選択
        StartCoroutine(SelectNextFrame());
    }

    private System.Collections.IEnumerator SelectNextFrame()
    {
        yield return null; // 1フレーム待つ
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
