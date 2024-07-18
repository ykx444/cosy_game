using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBackground : MonoBehaviour
{
    public RawImage backgroundImage;
    public float scrollSpeed = 0.5f;

    private void Update()
    {
        // Calculate the new UV Rect
        Rect uvRect = backgroundImage.uvRect;
        uvRect.x += scrollSpeed * Time.deltaTime;

        // Apply the new UV Rect to the RawImage
        backgroundImage.uvRect = uvRect;
    }
}
