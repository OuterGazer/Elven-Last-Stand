using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderArcher : MonoBehaviour
{
    //[SerializeField] GameObject arrow;
    [SerializeField] float arrowImpulse = 10.0f;
    private Vector2 arrowOffset;

    [SerializeField] float unitRange;
    [SerializeField] float attackSpeed = 1.0f;
    private WaitForSeconds attackRate;
    private bool canAttack = true;

    private Animator unitAnimator;
    private Defender defender;
    private DefenderSpawner defenderSpawner;

    // Start is called before the first frame update
    void Start()
    {
        this.attackRate = new WaitForSeconds(this.attackSpeed);

        this.defender = this.gameObject.GetComponent<Defender>();
        this.defenderSpawner = GameObject.FindObjectOfType<DefenderSpawner>();
        this.unitAnimator = this.gameObject.GetComponent<Animator>();

        this.arrowOffset = this.gameObject.transform.position + new Vector3(0.761f, -0.043f);
    }

    // FixedUpdate is called a fixed amount of times per second
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

    public void SpawnArrow(GameObject inArrow)
    {
        GameObject arrow = Instantiate<GameObject>(inArrow, this.arrowOffset, Quaternion.Euler(0.0f, 0.0f, 19.429f));

        arrow.transform.SetParent(this.defenderSpawner.ProjectileParent.transform);
        arrow.GetComponent<SpriteRenderer>().sortingOrder = this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder;

        arrow.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0.0f, 0.0f, 15.258f) * Vector2.right * this.arrowImpulse, ForceMode2D.Impulse);
        arrow.GetComponent<ArrowBehaviour>().SetProjectileYOriginAndArcher(this.gameObject.transform.position.y, this.defender);

        this.defender.PlayAttackSFX();
    }
}
