using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderWarrior : MonoBehaviour
{    
    [SerializeField] float unitRange;
    [SerializeField] float attackSpeed = 1.0f;
    private WaitForSeconds attackRate;
    private bool canAttack = true;

    private Animator unitAnimator;

    private Defender defender;

    // Start is called before the first frame update
    void Start()
    {
        this.attackRate = new WaitForSeconds(this.attackSpeed);
        this.unitAnimator = this.gameObject.GetComponent<Animator>();

        this.defender = this.gameObject.GetComponent<Defender>();
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
        this.defender.PlayAttackSFX();

        yield return this.attackRate;

        this.canAttack = true;
    }

    public void SetIsEnemyInRangeFalse()
    {
        this.unitAnimator.SetBool("isEnemyInRange", false);
    }
}
