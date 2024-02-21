using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFXquality : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
        QualitySettings.antiAliasing = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
