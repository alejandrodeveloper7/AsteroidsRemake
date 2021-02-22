using UnityEngine;

public class SmallAsteroidScript : AsteroidScript
{
    public delegate void SmallAsteroidDestroyedAction(AsteroidScript asteroidScript);
    public static event SmallAsteroidDestroyedAction OnSmallAsteroidDestroyed;

    public override void AsteroidDestroyed()
    {
        ConfigureDestroyedAsteroid();
        OnSmallAsteroidDestroyed(this);
    }

    internal override void RandomizeSprite()
    {
        randomNumber = Random.Range(0, boardManager.smallAsteroiSprites.Length);
        ownRenderer.sprite = boardManager.smallAsteroiSprites[randomNumber];
    }
}
