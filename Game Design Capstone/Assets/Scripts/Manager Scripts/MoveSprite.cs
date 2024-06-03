using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSprite : MonoBehaviour
{
    public Vector2 startPoint = new Vector2(-35, 5);   // Start position in local space
    public Vector2 endPoint = new Vector2(2000, 5);      // End position in local space
    public float duration = 10.0f;                       // Duration of the movement

    private RectTransform rectTransform;
    private float timeElapsed = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
            rectTransform.anchoredPosition = startPoint;
        else
            Debug.LogError("RectTransform component not found!");
    }

    void Update()
    {
        if (timeElapsed < duration && rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPoint, endPoint, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
        }
        else if (rectTransform != null)
        {
            rectTransform.anchoredPosition = endPoint;
        }
    }
}

