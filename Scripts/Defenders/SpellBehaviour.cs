using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviour : MonoBehaviour
{
    [SerializeField] float xRange = 12.0f; //limit spells to max 2 squares after unit placing limit to avoid abusing wizards at the very front
    [SerializeField] float damage = 40.0f;
    [SerializeField] float spellSpeed = 15.0f;
    [SerializeField] float spellRotation = 360.0f;
    private Vector2 spellMovement;
    [SerializeField] GameObject spellSplashVFX;

    private Rigidbody2D spellRB;
    private DefenderWizard wizard;
    private float yOrigin; //to check that projectiles exclusively hit enemies on their lane

    // Start is called before the first frame update
    void Start()
    {
        this.spellRB = this.gameObject.GetComponent<Rigidbody2D>();
        this.spellMovement = new Vector2(this.spellSpeed, 0.0f);
    }

    
    void Update()
    {
        if (this.gameObject.transform.position.x >= this.xRange - 0.50f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        this.spellRB.MoveRotation(this.spellRB.rotation + this.spellRotation * Time.fixedDeltaTime);
        this.spellRB.MovePosition(this.spellRB.position + this.spellMovement * Time.fixedDeltaTime);
    }

    public void SetProjectileOriginAndCaster(float yOrigin, DefenderWizard inWizard)
    {
        this.yOrigin = yOrigin;
        this.wizard = inWizard;
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
                    this.wizard.ActivateSpellEffect(collision.GetComponent<Attacker>());

                    health.GetImpactPoint(collision.gameObject.transform.position);
                    health.DealDamage(this.damage);

                    GameObject.Destroy(this.gameObject);
                    Instantiate<GameObject>(this.spellSplashVFX, this.gameObject.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
