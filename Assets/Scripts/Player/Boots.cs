using UnityEngine;

public class Boots : Collectable
{
    private Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getDobleJump()
    {
        animator.SetBool("collected", true);
        StartCoroutine(diseableObject());
    }
}
