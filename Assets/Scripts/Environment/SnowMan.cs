using System.Collections;
using UnityEngine;

public class SnowMan : MonoBehaviour
{
    [SerializeField]
    private Vector2 raycastOffset;
    [SerializeField]
    private float rangeDetectar;
    [SerializeField]
    private bool ModoZumbi;



    private Rigidbody2D rb;
    public float Speed;
    public Transform col;
    public LayerMask layer;
    public Player player;
    public float ShootRange;
    public float AttackRange;
    public float Firehate = 1f;
    private float nextFireTime;
    public GameObject Bullet;
    public GameObject BulletParent;
    private float OldSpeed = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        DetectarBeira();
        DetectarJogador();
    }

    private void attack()
    {
        Speed = 0;
        Instantiate(Bullet, BulletParent.transform.position, Quaternion.identity);
        nextFireTime = Time.time + Firehate;
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(Speed, rb.linearVelocity.y);
        var origemX = transform.position.x + raycastOffset.x;
        var origemY = transform.position.y + raycastOffset.y;
        var raycastParedeDireita = Physics2D.Raycast(new Vector2(origemX, origemY), Vector2.right, 0.5f, layer);
        Debug.DrawRay(new Vector2(transform.position.x, origemY), Vector2.right, Color.cyan);
        if (raycastParedeDireita.collider != null)
        {
            VirarTras();
        }
        var raycastParedeEsquerda = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, origemY), Vector2.left, 0.5f, layer);
        Debug.DrawRay(new Vector2(transform.position.x, origemY), Vector2.left, Color.cyan);
        if (raycastParedeEsquerda.collider != null)
        {
            VirarFrente();
        }

    }

    void Virar()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1f, transform.localScale.y);
        Speed *= -1f;
        OldSpeed = Speed;
    }
    void VirarTras()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.eulerAngles = new Vector3(0f, 180f, 0f);
        Speed = -3f;
        OldSpeed = Speed;
    }
    void VirarFrente()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        Speed = 3f;
        OldSpeed = Speed;
    }
    private void DetectarJogador()
    {
        var distanciajogador = Vector2.Distance(player.transform.position, transform.position);
        var diferencajogador = player.transform.position.x - transform.position.x;
        if (distanciajogador < ShootRange && distanciajogador > AttackRange && nextFireTime < Time.time)
        {
            attack();
        } else if(distanciajogador > ShootRange || distanciajogador < AttackRange)
        {
            StartCoroutine(backSpeed());
        }
    }

    IEnumerator backSpeed()
    {
        yield return new WaitForSeconds(0.5f);
        Speed = OldSpeed;
    }

    private void DetectarBeira()
    {
        var raycastDireita = Physics2D.Raycast(new Vector2(col.transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, 1f, layer);
        Debug.DrawRay(new Vector2(col.transform.position.x + raycastOffset.x, transform.position.y), Vector2.down, Color.red);
        if (raycastDireita.collider == null)
        {
            Virar();
        }
        var raycastEsquerda = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, 1f, layer);
        Debug.DrawRay(new Vector2(transform.position.x - raycastOffset.x, transform.position.y), Vector2.down, Color.red);
        if (raycastEsquerda.collider == null)
        {
            Virar();
        }
    }
}
