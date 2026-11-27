using UnityEngine;

public class rectsizereader : MonoBehaviour
{
    void Start()
    {
        Debug.Log(GetComponent<RectTransform>().rect.size);
        Debug.Log(GetComponent<RectTransform>().anchoredPosition);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
