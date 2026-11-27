using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    private int count = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string path = Path.Combine(Application.persistentDataPath, count.ToString()+".jpeg");
            ScreenCapture.CaptureScreenshot(path);
            count++;
        }
    }
}
