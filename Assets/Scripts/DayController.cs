using System.Collections.Generic;
using UnityEngine;

/*
    DayController holds and changes what day cycle of day it is, either day time or night time.
    After half a day it changes the cycle of day and notifies listeners.
    Length of night time and day time is equal.
    
    When using this script, ensure that it's executed before any listeners,
    in the Script Execution Order.
 */
public class DayController : MonoBehaviour
{
    private List<DayListener> dayListeners;
    public float DayLengthInMinutes = 10;
    private float CycleLengthInSeconds;
    [SerializeField] private DayCycle dayCycle;


    // Sets the DayController in the correct state.
    private void Start() {

        dayListeners = new List<DayListener>();
        dayCycle = DayCycle.NightTime;
        onCycleChange();
        CycleLengthInSeconds = (DayLengthInMinutes * 60) / 2;

        //InvokeRepating invokes a method first after 0 seconds, then every X seconds.
        InvokeRepeating("changeCycle", 0.0f, CycleLengthInSeconds);
    }

    //Changes the cycle of the day, from day time to night time,
    //or from nigth time to day time.
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
    // from daytime to nighttime, or from night time to daytime.
    private void onCycleChange() {
        if (dayListeners.Count > 0) { 
        foreach (DayListener dayListener in dayListeners) {
                dayListener.onChangeCycle(this.dayCycle);
            }
        }
        Debug.Log("Pushing cycle change");
    }

    //Adds day listeners.
    public bool addListener(DayListener dayListener) {
        if (dayListener == null && dayListeners.Contains(dayListener)) { 
            return false;
        }
        dayListeners.Add(dayListener);
        Debug.Log(dayListener + " added to listeners");
        
        return true;
    }
}
