using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{
    [SerializeField] float damage = 40.0f;

    private SpriteRenderer unitSprite;
    private void Start()
    {
        this.unitSprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.gameObject.transform.position.y - this.gameObject.transform.parent.position.y <= Mathf.Epsilon
        if (collision.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder == this.unitSprite.sortingOrder)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Health health = collision.gameObject.GetComponent<Health>();

                if (health != null)
                {
                    health.GetImpactPoint(collision.gameObject.transform.position);
                    health.DealDamage(this.damage);
                }
            }
        }
    }
}
