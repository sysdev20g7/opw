using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DayController holds and changes what part of the cycle the day is,
/// either Dawn, Day time, Dusk or Night time.
/// Notifies listeners when a change of day cycle.
/// Also implements a pull method, intended for when listeners are enabled/created,
/// to get current state.
/// The length of all parts of a day cycle are equal in length.
/// 
/// When using this script, ensure that it's executed before any listeners,
/// in the Script Execution Order.
/// </summary>
public class DayController : MonoBehaviour {

    [SerializeField] private DayCycle DayCycle;

    [SerializeField] private float DayLengthInMinutes = 1;
    private float CycleLengthInSeconds;

    private bool running;
    private List<DayListener> DayListeners;

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
    /// Tells listeners that the day is entering a new time of day.
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
        Debug.Log("Listner pulling DayCycle");
        return this.DayCycle;
    }

    /// <summary>
    /// Sets a new DayCycle.
    /// </summary>
    /// <param name="dayCycle">The DayCycle to set to.</param>
    public void SetDayCycle(DayCycle dayCycle) {
        Debug.Log("Setting new DayCycle");
        this.DayCycle = dayCycle;
        onCycleChange();
    }

    /// <summary>
    /// Subscribes day listener.
    /// </summary>
    /// <param name="dayListener"></param>
    /// <returns>true, if successfully added.</returns>
    public bool subscribe(DayListener dayListener) {
        if (dayListener == null && DayListeners.Contains(dayListener)) {
            return false;
        }
        DayListeners.Add(dayListener);
        Debug.Log(dayListener + " added to listeners");

        return true;
    }

    /// <summary>
    /// Unsubscribes day listener.
    /// </summary>
    /// <param name="dayListener"></param>
    /// <returns>true if successfully removed.</returns>
    public bool unsubscribe(DayListener dayListener) {
        if (!DayListeners.Contains(dayListener)) {
            return false;
        }
        DayListeners.Remove(dayListener);
        Debug.Log(dayListener + " removed from listeners");

        return true;
    }
}
