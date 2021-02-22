using UnityEngine;

public class InterfacePool : AbstractPool<GameObject>
{

    //A generic AbstractPool Child that checks the variable of the interface
    public InterfacePool(GameObject objectCopied, GameObject parent) : base(objectCopied, parent)
    {
    }

    public InterfacePool(GameObject objectCopied, GameObject parent, int initialSize) : base(objectCopied, parent, initialSize)
    {
    }

    public InterfacePool(GameObject objectCopied, GameObject parent, int initialSize, int scalation) : base(objectCopied, parent, initialSize, scalation)
    {
    }

    public InterfacePool(GameObject objectPooled, GameObject parent, int initialSize, int scalation, int poolMaxSize) : base(objectPooled, parent, initialSize, scalation, poolMaxSize)
    {
    }





    //Checks the bool of the interface. All objects with that interface can be pooled using this class
    protected override GameObject ObteinReadyInstance()
    {
        for (int i = 0; i < availableInstances.Length; i++)
        {
            if (availableInstances[i].GetComponent<IPooleableItem>().ReadyToUse)
            {
                return availableInstances[i];
            }
        }
        return null;
    }
}
