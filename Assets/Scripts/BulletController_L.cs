using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController_L : MonoBehaviour
{
    [SerializeField] private float speed = -10f;

    private Rigidbody2D rb;

    private PhotonView pv;

    private int dmg = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        rb.velocity = new Vector2(speed, 0);

        Destroy(this.gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController playerScript))
        {
            playerScript.Damage(dmg);
            pv.RPC("NetworkDestroy", RpcTarget.All);
        }
    }

    public void setDamage(int damage)
    {
        dmg = damage;
    }

    [PunRPC]
    public void NetworkDestroy()
    {
        Destroy(this.gameObject);
    }
}
