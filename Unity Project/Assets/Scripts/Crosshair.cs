using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] Texture2D image;
    [SerializeField] int size;
    [SerializeField] float maxangle;
    [SerializeField] float minangle;
    float lookHeight;
    public void LookHeight(float value){
        lookHeight += value;
        if (lookHeight > maxangle || lookHeight < minangle)
            lookHeight -= value;

    }

    void OnGUI()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition.y = Screen.height - screenPosition.y;
        GUI.DrawTexture(new Rect(screenPosition.x, screenPosition.y - lookHeight, size, size), image);
    }
}
