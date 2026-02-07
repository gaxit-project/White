using UnityEngine;
using System.Collections;

public class FinishScalePulse : MonoBehaviour
{
    private Vector3 normalScale = new Vector3(0.15f, 0.2f, 1f);
    private Vector3 pulseScale = new Vector3(0.15f, 0.03f, 1f);

    private Coroutine pulseCoroutine;

    private void OnEnable()
    {
        PlayerMove.OnPlayerFinished += StartPulse;
    }

    private void OnDisable()
    {
        PlayerMove.OnPlayerFinished -= StartPulse;
    }

    private void StartPulse()
    {
        if (pulseCoroutine == null)
            pulseCoroutine = StartCoroutine(PulseLoop());
    }

    private IEnumerator PulseLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            transform.localScale = pulseScale;
            yield return new WaitForSeconds(0.2f);
            transform.localScale = normalScale;
        }
    }
}
