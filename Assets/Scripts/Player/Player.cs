using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : StateGerenciator
{
    private Rigidbody2D rig;
    public float Speed;
    private bool isJumping = false;
    public float JumpForce;
    private Animator anim;
    private bool PlayerDie = false;
    private SpriteRenderer Sprite;
    private bool inDoor = false;
    private GameObject door;
    private bool canJump;
    private bool collectBoots = false;
    [SerializeField]
    private bool canDobleJump;
    [SerializeField]
    private Transform groundCheck;
    public GameObject keyboard_E;
    public GameObject keyboard_F;
    private bool isNearCandle = false;
    private bool inChest = false;

    public AudioClip jumpAudio, walkAudio;

    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        Sprite = this.gameObject.GetComponent<SpriteRenderer>();
        collectBoots = dobleJump;
    }

    void Update()
    {
        move();
        jump();
        interactDoor();
        congelamento();
        interactChest();
    }

    void congelamento()
    {

        // Lógica de congelamento
        if (isNearCandle)
        {
            GameController.Instance.ReduzirCongelamento();
        }
        else
        {
            GameController.Instance.AumentarCongelamento(); // Supondo que existe um método para aumentar congelamento
        }
    }

    void interactChest()
    {
        float berrys = GameController.Instance.berrys;
        float firewoods = GameController.Instance.firewoods;
        if (inChest && Input.GetKeyUp(KeyCode.F) && (berrys >= 15 && firewoods >= 15))
        {
            GameController.Instance.finalizingGame();
        }
    }

    void move()
    {
        float movement = Input.GetAxis("Horizontal");
        anim.SetBool("walk", movement != 0);
        rig.linearVelocity = new Vector2(movement * Speed, rig.linearVelocity.y);

        if (movement > 0f)
        {
            Sprite.flipX = false;
            if (!isJumping)
            {
                if (!SoundManager.audioSrc.isPlaying)
                {
                    SoundManager.PlaySound(walkAudio);
                }
            }
        }
        else if (movement < 0f)
        {
            Sprite.flipX = true;
            if (!isJumping)
            {
                if (!SoundManager.audioSrc.isPlaying)
                {
                    SoundManager.PlaySound(walkAudio);
                }
            }
        }
    }

    void interactDoor()
    {
        if (Input.GetKeyUp(KeyCode.E) && inDoor)
        {
            switch (door.tag)
            {
                case "outsideDoor":
                case "insideDoor":
                    door.GetComponent<NextFase>().GoToNextFase(collectBoots);
                    inDoor = false;
                    break;
            }
        }
    }

    void jump()
    {
        canJump = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetButtonDown("Jump") && (!isJumping && !PlayerDie && canJump))
        {
            rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            isJumping = true;
            SoundManager.PlaySound(jumpAudio);
        }
        else if (Input.GetButtonDown("Jump") && !canJump && (canDobleJump && collectBoots))
        {
            rig.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            isJumping = true;
            canDobleJump = false;
            SoundManager.PlaySound(jumpAudio);
        }
        isJumping = !canJump;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 6:
                GameController.Instance.AdicionarFrio(0.2f);
                break;
            case 8:
                canDobleJump = true;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "berry":
                collision.gameObject.GetComponent<Berry>().collectBerry();
                break;
            case "firewood":
                collision.gameObject.GetComponent<Firewood>().collectFirewood();
                break;
            case "bag":
                collision.gameObject.GetComponent<Bag>().retrieveItens();
                break;
            case "insideDoor":
            case "outsideDoor":
                keyboard_E.SetActive(true);
                door = collision.gameObject;
                inDoor = true;
                break;
            case "boots":
                dobleJump = true;
                collectBoots = true;
                collision.gameObject.GetComponent<Boots>().getDobleJump();
                break;
            case "nextFase":
                collision.gameObject.GetComponent<NextFase>().GoToNextFase(collectBoots);
                break;
            case "candle":
                isNearCandle = true;
                break;
            case "chest":
                inChest = true;
                keyboard_F.SetActive(true);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "insideDoor":
            case "outsideDoor":
                keyboard_E.SetActive(false);
                door = null;
                inDoor = false;
                break;
            case "candle":
                isNearCandle = false;
                break;
            case "chest":
                inChest = false;
                keyboard_F.SetActive(false);
                break;
        }
    }
}