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
    private Sprite spriteDawn;
    [SerializeField]
    private Sprite spriteDay;
    [SerializeField]
    private Sprite spriteDusk;
    [SerializeField]
    private Sprite spriteNight;
    private DayCycle dayCycle;


    public void Start() {

        GameObject temp = GameObject.FindGameObjectWithTag("DayController");
        if (temp != null) dayController = temp.GetComponent<DayController>();
        else Debug.Log("Can't Find DayController");
        if (dayController != null) { 
            dayController.subscribe(this);
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
            case DayCycle.Dawn:
                if (image.sprite != spriteDawn) {
                    image.sprite = spriteDawn;
                    Debug.Log("Changing Sprite to Dawn");
                }
                break;
            case DayCycle.DayTime:
                if (image.sprite != spriteDay) {
                    image.sprite = spriteDay;
                    Debug.Log("Changing Sprite to DayTime");
                }
                break;
            case DayCycle.Dusk:
                if (image.sprite != spriteDusk) {
                    image.sprite = spriteDusk;
                    Debug.Log("Changing Sprite to Dusk");
                }

                break;
            case DayCycle.NightTime:
                if (image.sprite != spriteNight) {
                    image.sprite = spriteNight;
                    Debug.Log("Changing Sprite to NightTime");
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Unsubscribes this listener from DayController.
    /// </summary>
    private void OnDisable() {
        if (dayController != null)
        dayController.unsubscribe(this);
    }
}
