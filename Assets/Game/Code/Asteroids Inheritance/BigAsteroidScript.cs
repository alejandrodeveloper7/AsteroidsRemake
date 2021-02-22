using UnityEngine;

public class BigAsteroidScript : AsteroidScript
{
    public delegate void BigAsteroidDestroyedAction(AsteroidScript asteroidScript);
    public static event BigAsteroidDestroyedAction OnBigAsteroidDestroyed;

    public override void AsteroidDestroyed()
    {
        ConfigureDestroyedAsteroid();
        OnBigAsteroidDestroyed(this);
    }

    internal override void RandomizeSprite()
    {
        randomNumber = Random.Range(0, boardManager.bigAsteroiSprites.Length);    
        ownRenderer.sprite =boardManager.bigAsteroiSprites[randomNumber];
    }
}
