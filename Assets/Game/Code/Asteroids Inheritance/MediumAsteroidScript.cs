using UnityEngine;

public class MediumAsteroidScript : AsteroidScript
{
    public delegate void MediumAsteroidDestroyedAction(AsteroidScript asteroidScript);
    public static event MediumAsteroidDestroyedAction OnMediumAsteroidDestroyed;

    public override void AsteroidDestroyed()
    {
        ConfigureDestroyedAsteroid();
        OnMediumAsteroidDestroyed(this);
    }

    internal override void RandomizeSprite()
    {
        randomNumber = Random.Range(0, boardManager.mediumAsteroiSprites.Length);
        ownRenderer.sprite = boardManager.mediumAsteroiSprites[randomNumber];
    }
}
