using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEyes : MonoBehaviour
{
    private Vector3 normalScale = new Vector3(0.15f, 0.2f, 1f);
    private Vector3 pulseScale = new Vector3(0.15f, 0.03f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PulseLoop());
    }

    // Update is called once per frame
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
