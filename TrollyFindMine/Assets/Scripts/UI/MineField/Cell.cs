using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    protected Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void HighLightCell()
    {
        outline.enabled = true;
    } 
    public void DeHighLightCell()
    {
        outline.enabled = false;
    } 
}