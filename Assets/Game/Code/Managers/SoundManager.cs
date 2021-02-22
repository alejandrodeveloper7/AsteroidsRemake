using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //This class manages the audioSources

    #region Variables

    [Header("References")]
    public AudioSource shootSource;
    public AudioSource playerDeadSource;
    public AudioSource asteroidDestroyedSource;

    #endregion

    #region Event subscriptions
    
    private void OnEnable()
    {
        SmallAsteroidScript.OnSmallAsteroidDestroyed += PlayAsteroidDestroyedSound;
        MediumAsteroidScript.OnMediumAsteroidDestroyed += PlayAsteroidDestroyedSound;
        BigAsteroidScript.OnBigAsteroidDestroyed += PlayAsteroidDestroyedSound;
        PlayerPhysicScript.OnPlayerDead += PlayPlayerDeadSound;
    }
    private void OnDisable()
    {
        SmallAsteroidScript.OnSmallAsteroidDestroyed -= PlayAsteroidDestroyedSound;
        MediumAsteroidScript.OnMediumAsteroidDestroyed -= PlayAsteroidDestroyedSound;
        BigAsteroidScript.OnBigAsteroidDestroyed -= PlayAsteroidDestroyedSound;
        PlayerPhysicScript.OnPlayerDead -= PlayPlayerDeadSound;
    }
    
    #endregion

    #region Sound reproduction methods

    public void PlayAsteroidDestroyedSound(AsteroidScript asteroidScript)
    {
        asteroidDestroyedSource.Play();
    }

    public void PlayPlayerDeadSound(Vector2 position)
    {
        playerDeadSource.Play();
    }

    public void PlayShootSound()
    {
        shootSource.Play();
    }
    
    #endregion

}
