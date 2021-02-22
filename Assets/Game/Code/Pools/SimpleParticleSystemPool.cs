using UnityEngine;

public class SimpleParticleSystemPool : AbstractPool<GameObject>
{

    //Simple abstractPool child that returns a instance by checking the ParticleSystem in the object

    [Header("MemoryAllocation")]
    ParticleSystem currentSystem;




    public SimpleParticleSystemPool(GameObject objectCopied, GameObject parent) : base(objectCopied, parent)
    {
    }
    public SimpleParticleSystemPool(GameObject objectCopied, GameObject parent, int initialSize) : base(objectCopied, parent, initialSize)
    {
    }
    public SimpleParticleSystemPool(GameObject objectCopied, GameObject parent, int initialSize, int scalation) : base(objectCopied, parent, initialSize, scalation)
    {
    }
    public SimpleParticleSystemPool(GameObject objectPooled, GameObject parent, int initialSize, int scalation, int poolMaxSize) : base(objectPooled, parent, initialSize, scalation, poolMaxSize)
    {
    }




    //This pool class Checks if the particle System int he Gameobject is playing or not. Its a very simple check that allow reuse this pool type
    //in diferent particle systems if they dont use the loop option
    protected override GameObject ObteinReadyInstance()
    {
        for (int i = 0; i < availableInstances.Length; i++)
        {
            currentSystem = availableInstances[i].GetComponent<ParticleSystem>();
            if (!currentSystem.isPlaying)
            {
                return availableInstances[i];
            }
        }
        return null;
    }

}
