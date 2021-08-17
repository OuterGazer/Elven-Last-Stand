using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcThugBehaviour : MonoBehaviour
{
    private Animator unitAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        this.unitAnimator = this.gameObject.GetComponent<Animator>();
    }

    public void YouCantRun()
    {
        this.unitAnimator.SetBool("canRun", false);
    }
}
