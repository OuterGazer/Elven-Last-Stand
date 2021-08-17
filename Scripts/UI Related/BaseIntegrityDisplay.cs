using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseIntegrityDisplay : MonoBehaviour
{
    [SerializeField] int baseHealthEasy = 200;
    [SerializeField] int baseHealthNormal = 100;
    [SerializeField] int baseHealthHard = 50;
    private int baseHealth;
    public int BaseHealth => this.baseHealth;

    private TextMeshProUGUI baseHealthText;

    // Start is called before the first frame update
    void Start()
    {
        this.EstablishStartingResources();

        this.baseHealthText = this.gameObject.GetComponent<TextMeshProUGUI>();

        this.UpdateDisplay();
    }

    private void EstablishStartingResources()
    {
        int gameDifficulty = PlayerPrefsController.GetDifficultyValue();

        if (gameDifficulty == 0)
            this.baseHealth = this.baseHealthEasy;
        else if (gameDifficulty == 1)
            this.baseHealth = this.baseHealthNormal;
        else if (gameDifficulty == 2)
            this.baseHealth = this.baseHealthHard;
    }

    private void UpdateDisplay()
    {
        this.baseHealthText.text = "Base Integrity ~ " + this.baseHealth.ToString();
    }
    
    public void DamageBase(int inDamageAmount)
    {
        this.baseHealth -= inDamageAmount;
        this.UpdateDisplay();

        if (this.baseHealth <= 0)
        {
            this.baseHealth = 0;
            this.UpdateDisplay();

            LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
            levelManager.SendMessage("GameOver");
        }
    }
}
