using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().material.SetColor("_BaseColor", Color.black);
    }
}
