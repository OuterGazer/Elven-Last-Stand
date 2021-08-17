using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100.0f;
    private float maxHealth;
    [SerializeField] GameObject blood;
    [SerializeField] GameObject lifeBarParent;

    private Animator animator;
    private Attacker attacker;
    private Defender defender;

    private Vector2 impactPoint;

    // Start is called before the first frame update
    void Start()
    {
        this.maxHealth = this.health;

        this.animator = this.gameObject.GetComponent<Animator>();

        if(this.gameObject.CompareTag("Enemy"))
            this.attacker = this.gameObject.GetComponent<Attacker>();

        if (this.gameObject.CompareTag("Player"))
            this.defender = this.gameObject.GetComponent<Defender>();
    }

    public void DealDamage(float inDamage)
    {
        this.health -= inDamage;

        this.DecreaseHealthBar();

        this.animator.SetBool("isHit", true);
        if (this.attacker != null)
            this.attacker.PlayHurtSFX();
        else if (this.defender != null)
            this.defender.PlayHurtSFX();

        GameObject blood = Instantiate<GameObject>(this.blood, this.impactPoint, Quaternion.identity);
        blood.GetComponent<Renderer>().sortingOrder = this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;

        if (this.gameObject.name.Equals("Orc Thug(Clone)"))
            this.gameObject.GetComponent<OrcThugBehaviour>().YouCantRun();

        if (this.health <= 0)
        {
            if(this.attacker != null)
                this.attacker.SetSpeed(0.0f);

            if (this.attacker != null)
                this.attacker.PlayDeathSFX();
            else if(this.defender != null)
                this.defender.PlayDeathSFX();

            this.animator.SetBool("isDead", true);


            if (this.gameObject.CompareTag("Resource"))
                this.DestroyDeadBody();
        }
    }

    private void DecreaseHealthBar()
    {
        float scaleFactor = this.health / this.maxHealth;

        if(this.health > 0)
        {
            this.lifeBarParent.transform.localScale = new Vector3(scaleFactor, 0.075f, 1.0f);
        }
        else
        {
            this.lifeBarParent.transform.localScale = Vector3.zero;
        }

        
    }

    public void GetImpactPoint(Vector2 inImpactPoint)
    {
        this.impactPoint = inImpactPoint;
    }
    public void SetIsHitToFalse()
    {
        this.animator.SetBool("isHit", false);
    }

    public void DestroyDeadBody()
    {
        GameObject.Destroy(this.gameObject, 0.50f);
        this.animator.enabled = false;
    }
}
