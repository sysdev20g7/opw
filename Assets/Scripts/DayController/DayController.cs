using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DayController holds and changes what day cycle of day it is, either day time or night time.
/// After half a day it changes the cycle of day and pushes the state to listeners.
/// Also implements a pull method, intended for when listeners are enabled/created, to get current
/// state.
/// Length of night time and day time is equal.
/// 
/// When using this script, ensure that it's executed before any listeners,
/// in the Script Execution Order.
/// </summary>
public class DayController : Singleton<DayController> {

    private List<DayListener> DayListeners;
    private float DayLengthInMinutes = 1;
    private float CycleLengthInSeconds;
    private DayCycle DayCycle;
    private bool running;

    private void Start() {

        DayListeners = new List<DayListener>();
       
        //Finds the number of states of DayCycles
        float NumberOfCycles = Convert.ToSingle(Enum.GetValues(typeof(DayCycle)).Length);
        CycleLengthInSeconds = (DayLengthInMinutes * 60) / NumberOfCycles;

        DayCycle = DayCycle.Dawn;
        running = true;
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
                case DayCycle.Dawn:
                    DayCycle = DayCycle.DayTime;
                    break;
                case DayCycle.DayTime:
                    DayCycle = DayCycle.Dusk;
                    break;
                case DayCycle.Dusk:
                    DayCycle = DayCycle.NightTime;
                    break;
                case DayCycle.NightTime:
                    DayCycle = DayCycle.Dawn;
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
                } else {
                    Debug.Log("Listener is Null, may not be unsubscribed.");
                }
            }
        }
        Debug.Log("Pushing cycle change");
    }

    /// <summary>
    /// Returns the current DayCycle.
    /// </summary>
    /// <returns>DayCycle</returns>
    public DayCycle GetDayCycle() {
        return this.DayCycle;
    }

    /// <summary>
    /// Adds day listener.
    /// </summary>
    /// <param name="dayListener"></param>
    /// <returns>bool</returns>
    public bool addListener(DayListener dayListener) {
        if (dayListener == null && DayListeners.Contains(dayListener)) {
            return false;
        }
        DayListeners.Add(dayListener);
        Debug.Log(dayListener + " added to listeners");

        return true;
    }

    /// <summary>
    /// Removes day listener. 
    /// </summary>
    /// <param name="dayListener"></param>
    /// <returns>bool</returns>
    public bool removeListener(DayListener dayListener) {
        if (!DayListeners.Contains(dayListener)) {
            return false;
        }
        DayListeners.Remove(dayListener);
        Debug.Log(dayListener + " removed from listeners");

        return true;
    }
}
