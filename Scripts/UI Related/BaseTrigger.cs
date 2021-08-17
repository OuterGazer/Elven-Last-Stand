using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    private BaseIntegrityDisplay baseHealth;

    // Start is called before the first frame update
    void Start()
    {
        this.baseHealth = GameObject.FindObjectOfType<BaseIntegrityDisplay>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Attacker>().PlayBaseHitSFX();
            OrcWeaponBehaviour orcDamage = collision.gameObject.GetComponentInChildren<OrcWeaponBehaviour>();
            this.baseHealth.DamageBase((int)orcDamage.Damage);
            GameObject.Destroy(collision.gameObject, 3.0f);
        }
    }
}
