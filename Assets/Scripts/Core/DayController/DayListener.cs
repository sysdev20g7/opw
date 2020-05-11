
/// <summary>
/// Represents a DayListener. A DayListener is interested
/// in knowing in what part of the DayCycle it is in.
/// </summary>
public interface IDayListener
{
    void OnChangeCycle(DayCycle dayCycle);
}
