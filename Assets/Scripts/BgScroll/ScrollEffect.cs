using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollEffect : MonoBehaviour
{
    public float scrollValue;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position -= new Vector3(0, scrollValue, 0);

        if (transform.position.y <= -spriteRenderer.bounds.size.y)
        {
            this.transform.position = new Vector3(0, 2*spriteRenderer.bounds.size.y, 0);
        }
    }
}
