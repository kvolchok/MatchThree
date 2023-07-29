using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    
    [SerializeField]
    private AudioClip _spawnItemsSound;
    [SerializeField]
    private AudioClip _noMatchSound;
    [SerializeField]
    private AudioClip _matchSound;
    [SerializeField]
    private AudioClip _dropItemsSound;

    public void PlaySpawnItemsSound()
    {
        _audioSource.PlayOneShot(_spawnItemsSound);
    }
    
    public void PlayNoMatchSound(float delay)
    {
        _audioSource.clip = _noMatchSound;
        _audioSource.PlayDelayed(delay);
    }
    
    public void PlayMatchSound(float delay)
    {
        _audioSource.clip = _matchSound;
        _audioSource.PlayDelayed(delay);
    }
    
    public void PlayDropItemsSound()
    {
        _audioSource.PlayOneShot(_dropItemsSound);
    }
}