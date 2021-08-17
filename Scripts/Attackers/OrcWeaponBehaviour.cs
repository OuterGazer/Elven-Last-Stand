using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWeaponBehaviour : MonoBehaviour
{
    [SerializeField] float damage = 40.0f;
    public float Damage => this.damage;

    private SpriteRenderer unitSprite;

    private Attacker attacker;

    private void Start()
    {
        this.unitSprite = this.gameObject.GetComponent<SpriteRenderer>();
        this.attacker = this.gameObject.GetComponentInParent<Attacker>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //(collision.gameObject.transform.position.y - this.gameObject.transform.parent.position.y <= Mathf.Epsilon)
        if (collision.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder == this.unitSprite.sortingOrder)
        {
            if (collision.gameObject.CompareTag("Player") ||
                collision.gameObject.CompareTag("Resource"))
            {
                Health health = collision.gameObject.GetComponent<Health>();
                this.attacker.PlayHitSFX();

                if (health != null)
                {
                    health.GetImpactPoint(collision.gameObject.transform.position);
                    health.DealDamage(this.damage);
                }
            }
        }
    }
}
