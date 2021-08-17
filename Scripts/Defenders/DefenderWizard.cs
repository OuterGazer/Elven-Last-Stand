using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderWizard : MonoBehaviour
{
    private Vector2 spellOffset;

    [SerializeField] float unitRange;
    [SerializeField] float attackSpeed = 1.0f;
    private WaitForSeconds attackRate;
    [SerializeField] float freezingTime = 4.0f;
    private WaitForSeconds freezeRate;
    [SerializeField] float slowDownFactor = 0.50f;
    private bool canAttack = true;

    private Animator unitAnimator;
    private Defender defender;
    private DefenderSpawner defenderSpawner;

    private Color32 frozenColor = new Color32(3, 74, 166, 255);

    // Start is called before the first frame update
    void Start()
    {
        this.attackRate = new WaitForSeconds(this.attackSpeed);
        this.freezeRate = new WaitForSeconds(this.freezingTime);

        this.defender = this.gameObject.GetComponent<Defender>();
        this.defenderSpawner = GameObject.FindObjectOfType<DefenderSpawner>();
        this.unitAnimator = this.gameObject.GetComponent<Animator>();

        this.spellOffset = this.gameObject.transform.position + new Vector3(1.0371f, 0.1229f);
    }

    // FixedUpdate is called 50 times per second as standard
    void FixedUpdate()
    {
        int layerMask = 1 << 9; //layer 9 is the enemies, ignore everything except the enemies
        RaycastHit2D[] inRange = Physics2D.RaycastAll(this.gameObject.transform.position, Vector2.right, this.unitRange, layerMask); 

        if (inRange.Length > 0)
        {
            if (canAttack)
            {
                this.StartCoroutine(this.Attack());
            }
        }
        else
        {
            this.unitAnimator.SetBool("isEnemyInRange", false);
        }
    }

    private IEnumerator Attack()
    {
        this.canAttack = false;
        this.unitAnimator.SetBool("isEnemyInRange", true);

        yield return this.attackRate;

        this.canAttack = true;
    }

    public void SetIsEnemyInRangeFalse()
    {
        this.unitAnimator.SetBool("isEnemyInRange", false);
    }

    public void SpawnSpell(GameObject inSpell)
    {
        GameObject spell = Instantiate<GameObject>(inSpell, this.spellOffset, Quaternion.identity);

        spell.GetComponent<SpriteRenderer>().sortingOrder = this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder;
        spell.transform.SetParent(this.defenderSpawner.ProjectileParent.transform);

        spell.GetComponent<SpellBehaviour>().SetProjectileOriginAndCaster(this.gameObject.transform.position.y,
                                                                          this);
        this.defender.PlayAttackSFX();
    }

    public void ActivateSpellEffect(Attacker inEnemy)
    {
        this.defender.PlayHitSFX();
        this.StartCoroutine(this.SlowDownEnemy(inEnemy));
    }

    private Attacker currentAffectedEnemy;

    private IEnumerator SlowDownEnemy(Attacker inEnemy)
    {
        this.currentAffectedEnemy = inEnemy;

        this.currentAffectedEnemy.SetSpriteColor(this.frozenColor);
        this.currentAffectedEnemy.SetUnitAnimationSpeed(true);
        this.currentAffectedEnemy.SetSpeedFactor(this.slowDownFactor);

        yield return this.freezeRate;

        this.ReturnEnemyToNormal(this.currentAffectedEnemy);
    }

    private void ReturnEnemyToNormal(Attacker inEnemy)
    {
        if (inEnemy != null)
        {
            inEnemy.SetSpriteColor(Color.white);
            inEnemy.SetUnitAnimationSpeed(false);
            inEnemy.SetSpeedFactor(1.0f);
        }
    }

    private void OnDestroy()
    {
        this.ReturnEnemyToNormal(this.currentAffectedEnemy);
    }
}
