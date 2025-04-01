using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Bag : Singleton<Bag>
{
    private float Berrys;
    private float Firewood;
    private int actualScene = 0;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D capsuleCollider;
    public AudioClip collected;
    public AudioSource audioSource;
    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
        Berrys = GameController.Instance.berrys;
        Firewood = GameController.Instance.firewoods;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        capsuleCollider= GetComponent<CircleCollider2D>();
        actualScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        isActive();
    }

    private void isActive()
    {
        if (SceneManager.GetActiveScene().buildIndex == actualScene)
        {
            spriteRenderer.enabled = true;
            capsuleCollider.enabled = true; 
        }
        else
        {
            spriteRenderer.enabled = false;
            capsuleCollider.enabled = false;
        }
    }

    public void retrieveItens()
    {
        audioSource.clip = collected;
        audioSource.Play();
        GameController.Instance.GetBerrys(Berrys);
        GameController.Instance.GetFirewood(Firewood);
        animator.SetBool("collected", true);
        Destroy(this.gameObject, 0.29f);
    }
}
