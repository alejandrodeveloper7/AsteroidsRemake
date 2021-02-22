public interface IPooleableItem
{
    //Any Object that use this interface could be pooled with the generic pool InterfacePool, 
    //only need manage a bool that defines if ready to use or not

    bool ReadyToUse
    {
        get;
    }
}
