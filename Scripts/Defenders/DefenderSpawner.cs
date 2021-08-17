using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderSpawner : MonoBehaviour
{
    private Defender defender;

    private GameObject defenderParent;
    const string DEFENDER_PARENT_NAME = "Playfield Manager";

    private GameObject projectileParent;
    public GameObject ProjectileParent => this.projectileParent;
    const string PROJECTILE_PARENT_NAME = "Projectile Manager";

    private void Start()
    {
        this.SetParents();
    }

    /// <summary>
    /// Set parents for defenders and projectiles so the hierarchy stays a bit cleaner
    /// </summary>
    private void SetParents()
    {
        this.defenderParent = GameObject.Find(DEFENDER_PARENT_NAME);
        this.projectileParent = GameObject.Find(PROJECTILE_PARENT_NAME);

        if (this.defenderParent == null)
        {
            this.defenderParent = new GameObject(DEFENDER_PARENT_NAME);
        }

        if(this.projectileParent == null)
        {
            this.projectileParent = new GameObject(PROJECTILE_PARENT_NAME);
        }
    }

    public void SetSelectedDefender(Defender defenderToSpawn)
    {
        this.defender = defenderToSpawn;
    }

    public void OnMouseDown()
    {
        if (this.defender != null)
            this.TryPlaceDefender();
    }

    private void TryPlaceDefender()
    {
        int currentResourceAmount = GameObject.FindObjectOfType<ResourceDisplay>().ResourceAmount;

        if(this.defender.ResourceCost <= currentResourceAmount)
        {
            this.SpawnDefender(this.GetSquareClicked());
            //the cost reduction is right beneath the actual spawning code
        }
    }

    private Vector2 GetSquareClicked()
    {
        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(clickPos);

        return this.SnapToGrid(worldPos);
    }

    private Vector2 SnapToGrid(Vector2 worldPos)
    {
        float gridPointX;
        float gridPointY;

        //due to how the grid is constructed, we need to check for the X, as the squares all fall mostly between the world coordinates
        if ((worldPos.x - (int)worldPos.x) < 0.50f)
            gridPointX = Mathf.Floor(worldPos.x);
        else
            gridPointX = Mathf.Ceil(worldPos.x);

        //There is a slight offset in the grid of around 0.11 among all rows. It could happen that clicking near the square above has the unit spawn in the square above, instead of the correct one
        if (worldPos.y - (int)worldPos.y <= 0.11f) 
            gridPointY = Mathf.Floor(worldPos.y);
        else
            gridPointY = Mathf.Ceil(worldPos.y);

        return new Vector2(gridPointX, gridPointY);
    }

    private void SpawnDefender(Vector2 spawnPoint)
    {
        if (this.IsSquareFree(spawnPoint))
        {
            GameObject newDefender = Instantiate<GameObject>(this.defender.gameObject, spawnPoint, Quaternion.identity);
            this.SortSpriteOrder(newDefender, spawnPoint.y);
            GameObject.FindObjectOfType<ResourceDisplay>().SpendResource(this.defender.ResourceCost);

            newDefender.transform.SetParent(this.defenderParent.transform);
            //newDefender.transform.parent = this.defenderParent.transform; //Both lines are equivalent, but the first one is more elegant to me
        }            
    }

    private bool IsSquareFree(Vector2 spawnPoint)
    {
        Defender[] existingDefenders = GameObject.FindObjectsOfType<Defender>();

        foreach(Defender item in existingDefenders)
        {
            if (spawnPoint.Equals(item.transform.position))
            {
                return false;
            }
        }

        return true;
    }

    private void SortSpriteOrder(GameObject inUnit, float inSpawnPointY)
    {
        SpriteRenderer[] childrenArray = inUnit.GetComponentsInChildren<SpriteRenderer>();
        switch (inSpawnPointY)
        {
            case 1:
                this.SortAllChildren(childrenArray, 9);
                break;
            case 2:
                this.SortAllChildren(childrenArray, 7);
                break;
            case 3:
                this.SortAllChildren(childrenArray, 5);
                break;
            case 4:
                this.SortAllChildren(childrenArray, 3);
                break;
            case 5:
                this.SortAllChildren(childrenArray, 1);
                break;
        }
    }

    private void SortAllChildren(SpriteRenderer[] inChildrenArray, int inSortingLayer)
    {
        foreach(SpriteRenderer item in inChildrenArray)
        {
            item.sortingOrder = inSortingLayer;
        }
    }
}
