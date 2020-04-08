using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Updates a UI Image which is used for indicating in what cycle
/// of the day the game is in.
/// Must be used with a Canvas and a child Image.
///
/// When using this script, ensure that it's executed after any publishers,
/// in the Script Execution Order.
/// </summary>
public class DayCycleIndicatorDisplay : MonoBehaviour, DayListener {
    [SerializeField]
    private DayController dayController;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite spriteNight;
    [SerializeField]
    private Sprite spriteDay;
    private DayCycle dayCycle;

    public void Start() {
        dayController = DayController.Instance;
        if (!dayController) { 
            dayController.addListener(this);
            dayCycle = dayController.GetDayCycle();
            updateIndicator();
        }
        else {
            Debug.Log("DayController not referenced.");
        }
    }

    /// <summary>
    /// Changes the this listeners day cycle to observers.
    /// </summary>
    /// <param name="dayCycle"></param>
    public void onChangeCycle(DayCycle dayCycle) {
        this.dayCycle = dayCycle;
        updateIndicator();
        Debug.Log(this + "-Listener: Cycle changed to " + dayCycle);
    }

    /// <summary>
    /// Updates a UI Image with sprite corresponding
    /// to the cycle of the day.
    /// </summary>
    private void updateIndicator() {
        switch (dayCycle) {
            case DayCycle.DayTime:
                if (image.sprite != spriteDay) {
                    image.sprite = spriteDay;
                    Debug.Log("Changing Sprite DayTime");
                }
                break;
            case DayCycle.NightTime:
                if (image.sprite != spriteNight) {
                    image.sprite = spriteNight;
                    Debug.Log("Changing Sprite NightTime");
                }
                break;
            default:
                break;
        }
    }

    //Unsubsribes this listener when disabled.
    private void OnDisable() {
        if (!dayController)
        dayController.removeListener(this);
    }
}
