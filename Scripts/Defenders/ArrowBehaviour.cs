using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] float damage = 40.0f;

    private Rigidbody2D arrowRB;
    private float yOrigin; //to check that projectiles exclusively hit enemies on their lane

    private Defender archer;

    // Start is called before the first frame update
    void Start()
    {
        this.arrowRB = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.arrowRB.rotation = Vector2.SignedAngle(Vector2.right, this.arrowRB.velocity);

        if (this.gameObject.transform.position.y <= this.yOrigin - 0.50f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void SetProjectileYOriginAndArcher(float yOrigin, Defender inArcher)
    {
        this.yOrigin = yOrigin;
        this.archer = inArcher;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.position.y == this.yOrigin)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Health health = collision.gameObject.GetComponent<Health>();

                if (health != null)
                {
                    health.GetImpactPoint(collision.gameObject.transform.position);
                    health.DealDamage(this.damage);

                    this.archer.PlayHitSFX();

                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }
}
