using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFader : MonoBehaviour
{
    // Define variables
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    Material material;
    public bool DoFade = false;



    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalOpacity = material.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (DoFade)
        {
            FadeNow();
        }
        else
        {
            ResetFade();
        }
    }


    void FadeNow()
    {
        Color currentColor = material.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed * Time.deltaTime));
        material.color = smoothColor;
    }

    void ResetFade()
    {
        Color currentColor = material.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed * Time.deltaTime));
        material.color = smoothColor;
    }
}
