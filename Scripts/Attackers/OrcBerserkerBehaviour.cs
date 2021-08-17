using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcBerserkerBehaviour : MonoBehaviour
{
    private Animator unitAnimator;
    private int maxJumpCounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        this.unitAnimator = this.gameObject.GetComponent<Animator>();
    }

    public bool Jump()
    {
        if(maxJumpCounter < 1)
        {
            this.unitAnimator.SetTrigger("Jump");
            maxJumpCounter++;
            return true;
        }

        return false;
    }

    public void SetPositionAfterJump()
    {
        this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x - 2.5f,
                                                         this.gameObject.transform.position.y);
    }
}
