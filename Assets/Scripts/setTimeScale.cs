using UnityEngine;

public class setTimeScale : MonoBehaviour
{
    void LateUpdate()
    {
        Time.timeScale = 1.0f;
    }

    void FixedUpdate()
    {
        Time.timeScale = 1.0f;
    }

}
