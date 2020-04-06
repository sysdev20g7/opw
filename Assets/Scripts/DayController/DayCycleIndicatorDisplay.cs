using UnityEngine;
using UnityEngine.UI;

/* Updates a UI Image which is used for indicating in what cycle
   of the day the game is in.
   Must be used with a Canvas and a child Image.

   When using this script, ensure that it's executed after any publishers,
   in the Script Execution Order.
*/
public class DayCycleIndicatorDisplay : MonoBehaviour, DayListener {
    public DayController dayController;
    public Image Image;
    private DayCycle dayCycle;
    public Sprite spriteNight;
    public Sprite spriteDay;

    //Adds the CycleIndicatorDisplay as a listener to DayController.
    public void Start() {
        dayController.addListener(this);
    }

    //Update is called every frame.
    void Update() {
        updateIndicator();

    }

    //Changes the cycle of the day.
    public void onChangeCycle(DayCycle dayCycle) {
        this.dayCycle = dayCycle;
        Debug.Log("Test Listener: Cycle changed to " + dayCycle);
    }

    //Changes the sprite of the UI Image.
    private void updateIndicator() {
        switch (dayCycle) {
            case DayCycle.DayTime:
                if (Image.sprite != spriteDay) {
                    Image.sprite = spriteDay;
                    Debug.Log("Changing Sprite DayTime");
                }
                break;
            case DayCycle.NightTime:
                if (Image.sprite != spriteNight) {
                    Image.sprite = spriteNight;
                    Debug.Log("Changing Sprite NightTime");
                }
                break;
            default:
                break;
        }
    }
}
