using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public void Shake()
    {
        StartCoroutine(ShakeEffect(1f, 0.1f));
    }
    IEnumerator ShakeEffect(float duration, float magnitude)
    {
        float totalTime = 0;
        while (totalTime < duration)
        {
            float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            float y = UnityEngine.Random.Range(-1f, 1f) * magnitude;
            this.transform.position = new Vector3(x, y, this.transform.position.z);
            totalTime += Time.deltaTime;
            yield return null;
        }

    }
}
