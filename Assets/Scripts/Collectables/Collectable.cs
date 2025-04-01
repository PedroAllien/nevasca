using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectable : StateGerenciator
{
    public AudioClip collected;
    public AudioSource audioSource;
    public IEnumerator diseableObject()
    {
        audioSource.clip = collected;
        audioSource.Play();
        yield return new WaitForSeconds(0.29f);
        this.gameObject.SetActive(false);
        this.isActive = this.gameObject.activeInHierarchy;
        yield return null;
    }
}
