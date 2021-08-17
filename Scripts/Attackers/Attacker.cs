using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{ 
    [Range(0.0f, 20.0f)] //for walkSpeed
    [SerializeField] float movementSpeed = 1.0f;
    private float speedFactor = 1.0f; //to account for slowdown spell effect in movement and attackspeed
    
    public void SetSpeed (float inSpeed)
    {
        this.movementSpeed = inSpeed;
    }

    [SerializeField] float attackSpeed = 1.0f;
    private WaitForSeconds attackRate;
    private bool canAttack = true;

    private SpriteRenderer unitSprite;
    private Animator unitAnimator;
    [SerializeField] float unitRange = 1.0f;

    private LevelManager levelManager;

    private AudioSource audioSource;
    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip weaponSFX;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip dieSFX;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip baseHitSFX;

    private void Awake()
    {
        this.levelManager = GameObject.FindObjectOfType<LevelManager>();

        this.levelManager.AddAttacker(this);

        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefsController.GetMasterSFX();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.unitSprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();
        this.attackRate = new WaitForSeconds(this.attackSpeed);
        this.unitAnimator = this.gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        int layerMask = (1 << 8); //layer 8 is the defenders, 9 is attackers. Attack defenders and wait behind existing attackers
        RaycastHit2D[] inRange = Physics2D.RaycastAll(this.gameObject.transform.position, Vector2.left, this.unitRange, layerMask);

        if (inRange.Length > 0)
        {
            //must put 1 because at index 0 will be always the unit's own collider as I'm checking for enemies and attackers at the same time
            /*if (inRange[1].collider.CompareTag("Enemy")) //wait behind existing attackers
            {
                this.unitAnimator.SetBool("canMove", false);
                return;
            }*/

            if (inRange[0].collider.CompareTag("Projectile")) { return; } //prevents the first couple of frames of the attack animation from playing if raycast encounters a defender's projetile

            if(this.gameObject.GetComponent<OrcBerserkerBehaviour>() && inRange[0].collider.gameObject.GetComponent<DefenderWarrior>())
            {
                //Berserker can't jump if there is more than one defender in a row
                RaycastHit2D[] defendersInARow = Physics2D.RaycastAll(this.gameObject.transform.position, Vector2.left, this.unitRange + 2, layerMask);
                
                if (defendersInARow.Length < 2 &&
                    this.gameObject.GetComponent<OrcBerserkerBehaviour>().Jump() == true)
                {
                    this.PlaySFXWithPitch(this.jumpSFX);
                    return;
                }
                
                
            }

            if (canAttack)
            {
                this.unitAnimator.SetBool("canMove", false);
                this.StartCoroutine(this.Attack());
            }
        }
        else
        {
            this.unitAnimator.SetBool("canMove", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Translate(Vector2.left * (this.movementSpeed * this.speedFactor)  * Time.deltaTime);
    }

    private IEnumerator Attack()
    {
        this.canAttack = false;
        this.unitAnimator.SetBool("isAttacking", true);
        this.PlaySFXWithPitch(this.attackSFX);

        yield return this.attackRate;

        this.canAttack = true;
    }

    public void SetIsAttackingFalse()
    {
        this.unitAnimator.SetBool("isAttacking", false);
    }

    public void SetMovementSpeed(float speed)
    {
        this.movementSpeed = speed;
    }

    public void SetSpriteColor(Color32 inColor)
    {
        this.unitSprite.color = inColor;
    }

    public void SetUnitAnimationSpeed(bool shouldSlowDown)
    {
        if (shouldSlowDown)
            this.unitAnimator.speed = 0.75f;
        else
            this.unitAnimator.speed = 1.0f;

    }

    public void SetSpeedFactor(float inSpeedFactor)
    {
        this.speedFactor = inSpeedFactor;
        this.attackRate = new WaitForSeconds(this.attackSpeed * inSpeedFactor);
    }

    public void SetSFXVolume(float inVolume)
    {
        this.audioSource.volume = inVolume;
    }

    public void PlayHitSFX()
    {
        this.PlaySFXWithPitch(this.weaponSFX);
    }

    public void PlayHurtSFX()
    {
        this.PlaySFXWithPitch(this.hurtSFX);
    }

    public void PlayDeathSFX()
    {
        this.PlaySFXWithPitch(this.dieSFX);
    }

    public void PlayBaseHitSFX()
    {
        this.PlaySFXWithPitch(this.baseHitSFX);
    }

    private void PlaySFXWithPitch(AudioClip inSFX)
    {
        this.audioSource.pitch = Random.Range(0.9f, 1.1f);
        this.audioSource.PlayOneShot(inSFX);
    }

    private void OnDestroy()
    {
        if(this.levelManager != null)
            this.levelManager.RemoveAttacker(this);
    }
}
