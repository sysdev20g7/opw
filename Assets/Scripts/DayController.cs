using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    DayController holds and changes what day cycle of day it is, either day time or night time.
    After X seconds it changes the cycle of day and notifies listeners.
    In this implementation, the length of night time and day time is equal.
 */
public class DayController : MonoBehaviour
{
    private List<DayListener> dayListeners;
    [SerializeField] private const float DayCycleLengthInSeconds = 600.0f;
    [SerializeField] private DayCycle dayCycle;

    // Start is called before the first frame update
    void Start() {
        //InvokeRepating invokes a method first after 0 seconds, then every X seconds.
        dayListeners = new List<DayListener>();
        dayCycle = DayCycle.DayTime;
        InvokeRepeating("changeCycle", 0.0f, DayCycleLengthInSeconds);
    }

    //
    private void changeCycle() {
        if (dayCycle == DayCycle.DayTime) {
            dayCycle = DayCycle.NightTime;
        }
        else {
            dayCycle = DayCycle.DayTime;
        }
        onCycleChange();
    }

    // Tells listeners that the day is entering a new cycle,
    // from daytime to nighttime, or from nighttime to daytime.
    private void onCycleChange() {
        foreach (DayListener dayListener in dayListeners)
        {
            dayListener.onChangeCycle(dayCycle);
        }
    }

    //Adds day listeners.
    public bool addListener(DayListener dayListener) {
        if (dayListener == null) { 
            return false;
        }
        dayListeners.Add(dayListener);
        
        return true;
    }
}
