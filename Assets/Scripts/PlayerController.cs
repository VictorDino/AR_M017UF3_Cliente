using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [Header("Stats")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 22f;

    [Header("Enemy")]
    [SerializeField] private float enemySmooth = 500;

    private Rigidbody2D rb;

    [Header("References")]
    [SerializeField] Transform firePoint;

    private string playerName = "";
    [SerializeField] TMP_Text nickName;
    private string enemyName = "";

    private bool isGrounded;
    private bool isMoveing = false;
    [SerializeField] private GameObject groundCheckPoint;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private bool WPressed = false;
    private bool APressed = false;
    private bool DPressed = false;

    private PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;
    private Quaternion enemyRotation = Quaternion.identity;

    private Animator animator;

    private int max_hp = 100;
    private int current_hp = 100;
    private int lifes = 3;
    private int dmg = 20;

    private float shootCD = 0.5f;
    private float counter = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;
    }

    private void Start()
    {
        max_hp = 100;
        current_hp = max_hp;
        lifes = 3;

        playerName = PhotonNetwork.NickName;
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            CheckInput();
            nickName.text = playerName;
            nickName.color = Color.black;
            nickName.gameObject.transform.rotation = Quaternion.identity;
        }
        else
        {
            SmoothReplicate();
        }

        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.transform.position, groundCheckRadius, groundLayer);

        if (APressed)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
            APressed = false;
        }
        else if (DPressed)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            DPressed = false;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (WPressed && isGrounded)
        {
            rb.velocity = new Vector2(0, jumpForce);
            WPressed = false;
        }
        else { WPressed = false; }
    }

    private void CheckInput()
    {
        if (Input.GetKey(KeyCode.W)) WPressed = true;
        if (Input.GetKey(KeyCode.A)) APressed = true;
        if (Input.GetKey(KeyCode.D)) DPressed = true;
        
        if (counter < shootCD) { counter += Time.deltaTime; }

        if (Input.GetKey(KeyCode.Space) && counter > shootCD)
        {
            Shoot();
            counter = 0;
        }

        if (APressed || DPressed || WPressed) { isMoveing = true; }
        else { isMoveing = false; }
    }

    private void UpdateAnimations()
    {
        if (isMoveing) { animator.SetBool("moveing", true); }
        else { animator.SetBool("moveing", false); }

        if (isGrounded) { animator.SetBool("grounded", true); }
        else { animator.SetBool("grounded", false); }
    }

    private void SmoothReplicate()
    {
        transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * enemySmooth);
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * enemySmooth);
        nickName.text = enemyName;
        nickName.gameObject.transform.rotation = Quaternion.identity;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(isMoveing);
            stream.SendNext(isGrounded);
            stream.SendNext(playerName);
        }
        else if (stream.IsReading)
        {
            enemyPosition = (Vector3)stream.ReceiveNext();
            enemyRotation = (Quaternion)stream.ReceiveNext();
            isMoveing = (bool)stream.ReceiveNext();
            isGrounded = (bool)stream.ReceiveNext();
            enemyName = (string)stream.ReceiveNext();
        }
    }

    private void Shoot()
    {
        if (firePoint.transform.position.x > this.transform.position.x)
        {
            PhotonNetwork.Instantiate("Bullet_Right", firePoint.position, Quaternion.identity).GetComponent<BulletController>().setDamage(dmg);
        }
        else
        {
            PhotonNetwork.Instantiate("Bullet_Left", firePoint.position, Quaternion.identity).GetComponent<BulletController_L>().setDamage(dmg);
        }
    }

    public void Damage(int damage)
    {
        pv.RPC("NetworkDamage", RpcTarget.All, damage);
    }

    public int getDamage()
    {
        return dmg;
    }

    [PunRPC]
    private void NetworkDamage(int damage)
    {
        current_hp -= damage;

        if (current_hp <= 0)
        {
            lifes--;
            if (lifes <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                GameManager._GAME_MANAGER.Respawn(this.gameObject);
                current_hp = max_hp;
            }
        }
    }
}
