using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] int resourceAmountEasy = 150;
    [SerializeField] int resourceAmountNormal = 100;
    [SerializeField] int resourceAmountHard = 50;

    private int resourceAmount;
    public int ResourceAmount => this.resourceAmount;

    private TextMeshProUGUI resourceText;

    // Start is called before the first frame update
    void Start()
    {
        this.EstablishInitialResources();

        this.resourceText = this.gameObject.GetComponent<TextMeshProUGUI>();

        this.UpdateDisplay();
    }

    private void EstablishInitialResources()
    {
        int gameDifficulty = PlayerPrefsController.GetDifficultyValue();

        switch (gameDifficulty)
        {
            case 0:
                this.resourceAmount = this.resourceAmountEasy;
                break;
            case 1:
                this.resourceAmount = this.resourceAmountNormal;
                break;
            case 2:
                this.resourceAmount = this.resourceAmountHard;
                break;
        }
    }

    private void UpdateDisplay()
    {
        this.resourceText.text = resourceAmount.ToString();
    }

    public void AddResource(int inResourceAmount)
    {
        this.resourceAmount += inResourceAmount;

        this.UpdateDisplay();
    }

    public void SpendResource(int inResourceAmount)
    {
        if(this.resourceAmount >= inResourceAmount)
        {
            this.resourceAmount -= inResourceAmount;

            this.UpdateDisplay();
        }
    }
}
