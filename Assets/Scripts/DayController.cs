using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayController : MonoBehaviour
{
    private const float DayLengthInSeconds = 600.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("changeCycle", 0.0f, DayLengthInSeconds);
    }

    private void changeCycle() {
            Debug.Log("SKIFTER DAG");
    }

}
