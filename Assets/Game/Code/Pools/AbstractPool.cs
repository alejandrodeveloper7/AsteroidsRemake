using System;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class AbstractPool<T> where T : Object
{

    //This class is a old class that I did some time ago to implement diferent configurable pools quickly. 
    //There is an interface and a child from this class that allows pool an item without create a new class     //IPoleableItem and InterfacePool
    // Is a simple class with constructors that creates an array of Gameobjects and can return 
    //an object when it meets the specified conditions or if all are being used expand the pool if the configurations allows it.

    // The abstract Methods allows in each child configure the return conditions and apply diferent logic in some moments of the life cycle of each instance.
    // The children are doing their checks and recycling without using the prefab life cycles, I have commented the abstract methods and
    // the calls to avoid call empty methods that are not needed in this project

    #region Variables

    protected T[] availableInstances;
    private T objectPooled;
    private int scalation;
    private GameObject parent;

    private int poolMaxSize;
    public int PoolMaxSize { get=>poolMaxSize; private set=> poolMaxSize=value; }
    private int poolCurrentSize;
    public int PoolCurrentSize    { get => poolCurrentSize; private set => poolCurrentSize = value; }

    [Header("Memory Allocation")]
    GameObject readyInstance;

    #endregion

    #region Constuctors

    public AbstractPool(T objectCopied, GameObject parent)
        : this(objectCopied, parent, 0, 1, int.MaxValue)
    {
    }
    public AbstractPool(T objectCopied, GameObject parent, int initialSize)
        : this(objectCopied, parent, initialSize, 1, int.MaxValue)
    {
    }
    public AbstractPool(T objectCopied, GameObject parent, int initialSize, int scalation)
        : this(objectCopied, parent, initialSize, scalation, int.MaxValue)
    {
    }
    public AbstractPool(T objectPooled, GameObject parent, int initialSize, int scalation, int poolMaxSize)
    {
        if (scalation <= 0)
        {
            Debug.LogError("The pool need a scalation of atleast 1");
            return;
        }
        if (poolMaxSize <= 0)
        {
            Debug.LogError("The pool need a maximun Size of atleast 1");
            return;
        }
        this.objectPooled = objectPooled;
        this.scalation = scalation;
        PoolMaxSize = poolMaxSize;
        this.parent = parent;
        availableInstances = new T[0];

        if (initialSize > 0)
        {
            ExpandPoolSize(initialSize);
        }
    }

    #endregion

    #region Abstracts

    //protected abstract void NewInstanceCreation(T prefabInstance);
    //protected abstract void InstanceRestartState(T prefabInstance);
    //protected abstract void RecicleInstance(T prefabInstance);
    protected abstract T ObteinReadyInstance();

    #endregion

    #region public Methods

    public T GetInstance()
    {
        T prefabInstance = ObteinReadyInstance();

        if (prefabInstance != null)
        {
            if (parent != null)
            {
                GameObject readyInstance = prefabInstance as GameObject;
                readyInstance.transform.parent = null;
            }
            //InstanceRestartState(prefabInstance);

            return prefabInstance;
        }
        else
        {

            if (scalation == 1 && availableInstances.Length < PoolMaxSize)
            {
                prefabInstance = CreateNewInstance();
                Array.Resize(ref availableInstances, availableInstances.Length + 1);
                availableInstances[availableInstances.Length - 1] = prefabInstance;
                GameObject readyInstance = prefabInstance as GameObject;
                readyInstance.transform.SetParent(null);
                //InstanceRestartState(prefabInstance);
                return prefabInstance;
            }
            else if (availableInstances.Length < PoolMaxSize)
            {
                ExpandPoolSize(scalation);
                prefabInstance = ObteinReadyInstance();
                GameObject readyInstance = prefabInstance as GameObject;
                readyInstance.transform.SetParent(null);
                //InstanceRestartState(prefabInstance);

            }

            if (prefabInstance == null)
            {
                Debug.LogError("Pool at max size and not ready instances");
            }

            return prefabInstance;

        }

    }
    public void RecyclePrefabInstance(T prefab)
    {
        if (prefab == null)
        {
            Debug.Log("You cant recycle null objects");
            return;
        }
        bool isInThePool = false;
        for (int i = 0; i < availableInstances.Length; i++)
        {

            if (availableInstances[i] == prefab)
            {
                isInThePool = true;
            }
        }

        if (!isInThePool)
        {
            Debug.LogError("Prefab not from this pool");
            return;
        }
        if (parent != null)
        {
            readyInstance = prefab as GameObject;
            readyInstance.transform.SetParent(parent.transform);
        }
        //RecicleInstance(prefab);

    }

    #endregion

    #region Internal Logic

    private T CreateNewInstance()
    {

        GameObject gameobjectPooled = objectPooled as GameObject;
        var instance = Object.Instantiate(objectPooled, gameobjectPooled.transform.position, gameobjectPooled.transform.rotation) as T;
        if (parent != null)
        {
            GameObject readyInstance = instance as GameObject;
            readyInstance.transform.SetParent(parent.transform);
        }
        //NewInstanceCreation(instance);
        return instance;
    }
    private void ExpandPoolSize(int count)
    {

        int allocationCount = PoolMaxSize - availableInstances.Length;

        if (allocationCount == 0)
        {
            Debug.LogError("Pool at max size, can´t be expanded more");
            return;
        }
        if (count < allocationCount)
        {
            allocationCount = count;
        }
        else
        {
            Debug.Log("Expanded " + allocationCount + ",pool at max Size. Pooled Prefab: " + objectPooled.name);
        }

        int firstPosition = availableInstances.Length;
        Array.Resize(ref availableInstances, availableInstances.Length + allocationCount);

        for (int i = firstPosition; i < availableInstances.Length; i++)
        {
            availableInstances[i] = CreateNewInstance();
        }
        PoolCurrentSize = availableInstances.Length;

    }

    #endregion

}
