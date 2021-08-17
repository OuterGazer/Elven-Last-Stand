using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenderUI : MonoBehaviour
{
    [SerializeField] Defender defenderPrefab;

    private DefenderSpawner defenderSpawner;
    [SerializeField] TextMeshProUGUI costText;

    private Color32 greyedOut = new Color32(80, 80, 80, 255);
    private Color32 brightYellow = new Color32(255, 246, 0, 255);
    private Color32 originalYellow;

    private void Start()
    {
        this.costText.text = this.defenderPrefab.ResourceCost.ToString();
        this.originalYellow = this.costText.color;
        this.defenderSpawner = GameObject.FindObjectOfType<DefenderSpawner>();
    }

    private void OnMouseDown()
    {
        DefenderUI[]  buttonCollection = GameObject.FindObjectsOfType<DefenderUI>();

        foreach (DefenderUI item in buttonCollection)
        {
            SpriteRenderer[] buttonComps = item.gameObject.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sprite in buttonComps)
            {
                if(sprite.color != Color.black)
                    sprite.color = this.greyedOut;
            }

            item.costText.color = originalYellow;
        }

        SpriteRenderer[] buttonComponents = this.gameObject.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer item in buttonComponents)
        {
            item.color = Color.white;
        }
        if (this.costText != null)
            this.costText.color = this.brightYellow;

        this.defenderSpawner.SetSelectedDefender(this.defenderPrefab);
    }
}
