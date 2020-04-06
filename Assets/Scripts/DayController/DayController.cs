using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DayController holds and changes what day cycle of day it is, either day time or night time.
/// After half a day it changes the cycle of day and notifies listeners.
/// Length of night time and day time is equal.
/// 
/// When using this script, ensure that it's executed before any listeners,
/// in the Script Execution Order.
/// </summary>
public class DayController : MonoBehaviour {
    private List<DayListener> DayListeners;
    [SerializeField]
    private float DayLengthInMinutes = 1;
    private float CycleLengthInSeconds;
    private DayCycle DayCycle;
    private bool running;


    private void Start() {

        running = true;
        DayListeners = new List<DayListener>();
        //Remove the next few lines for when I've implemented pull
        //Then the listeners pulls first when they've started and then gets
        //states pushed later. Or just remove on onCycleChange call
        DayCycle = DayCycle.DayTime;
        onCycleChange();
        //Finds the number of states of DayCycles
        float NumberOfCycles = Convert.ToSingle(Enum.GetValues(typeof(DayCycle)).Length);
        CycleLengthInSeconds = (DayLengthInMinutes * 60) / NumberOfCycles;

        //InvokeRepating invokes a method first after 0 seconds, then every X seconds.
        StartCoroutine("changeCycle");
    }

    /// <summary>
    /// Changes the cycle of the day, from day time to night time,
    /// or from nigth time to day time every cycle length in seconds.
    /// </summary>
    private IEnumerator changeCycle() {
        while (running) {
            yield return new WaitForSeconds(CycleLengthInSeconds);
            switch (DayCycle) {
                case DayCycle.DayTime:
                    DayCycle = DayCycle.NightTime;
                    break;
                case DayCycle.NightTime:
                    DayCycle = DayCycle.DayTime;
                    break;
            }
            onCycleChange();
        }
    }

    /// <summary>
    /// Tells listeners that the day is entering a new cycle,
    /// from daytime to nighttime, or from night time to daytime.
    /// </summary>
    private void onCycleChange() {
        if (DayListeners.Count > 0) {
            foreach (DayListener dayListener in DayListeners) {
                if (dayListener != null) {
                    dayListener.onChangeCycle(this.DayCycle);
                }
            }
        }
        Debug.Log("Pushing cycle change");
    }

    /// <summary>
    /// Adds day listeners.
    /// </summary>
    public bool addListener(DayListener dayListener) {
        if (dayListener == null && DayListeners.Contains(dayListener)) {
            return false;
        }
        DayListeners.Add(dayListener);
        Debug.Log(dayListener + " added to listeners");

        return true;
    }
}
