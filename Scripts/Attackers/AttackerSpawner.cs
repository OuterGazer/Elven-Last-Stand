using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum LevelNumber
{
    isLevelOne,
    isLevelTwo,
    isLevelThree
}

public class AttackerSpawner : MonoBehaviour
{
    [SerializeField] Attacker[] orcEnemy;

    private bool canSpawn = true;
    public bool CanSpawn => this.canSpawn;
    public void SetCanSpawn(bool canSpawn)
    {
        this.canSpawn = canSpawn;
    }

    private LevelNumber playingLevel;
    public LevelNumber PlayingLevel => this.playingLevel;

    private float spawnRate;
    [SerializeField] float minSpawnRate = 3.0f;
    [SerializeField] float maxSpawnRate = 4.0f;

    private Vector2 spawnPoint;
    [SerializeField] int minLane = 1;
    [SerializeField] int maxLane = 6;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        this.GetCurrentLevel();

        do
        {
            switch (this.playingLevel)
            {
                case LevelNumber.isLevelOne:
                    yield return this.StartCoroutine(this.SpawnAttackersLevelOne());
                    break;
                case LevelNumber.isLevelTwo:
                    yield return this.StartCoroutine(this.SpawnAttackersLevelTwo());
                    break;
                case LevelNumber.isLevelThree:
                    yield return this.StartCoroutine(this.SpawnAttackersLevelThree());
                    break;
            }


        } while (this.canSpawn);
    }

    private void GetCurrentLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Level 1":
                this.playingLevel = LevelNumber.isLevelOne;
                break;
            case "Level 2":
                this.playingLevel = LevelNumber.isLevelTwo;
                break;
            case "Level 3":
                this.playingLevel = LevelNumber.isLevelThree;
                break;
        }
    }

    private IEnumerator SpawnAttackersLevelOne()
    {
        if (Time.timeSinceLevelLoad < 20) //The first 20 seconds spawn thugs at the standard random intervals
        {
            this.spawnRate = Random.Range(this.minSpawnRate, this.maxSpawnRate);
        }
        else
        {
            this.spawnRate = 3.0f; //The berserkers will appear in a rush wave
        }        

        yield return new WaitForSeconds(this.spawnRate);
        

        if (Time.timeSinceLevelLoad < 20) //The first 20 seconds only thugs appear, the last 10 will spawn some berserkers
        {
            this.spawnPoint = new Vector2(this.gameObject.transform.position.x,
                                      Random.Range(this.minLane, this.maxLane)); //thugs will spawn randomly in lanes 2, 3 and 4

            this.Spawn(this.orcEnemy[0]);
        }
        else
        {
            int laneSelection = Random.Range(0, 2);

            if(laneSelection == 0) //berserkers will only spawn in lanes 1 and 5
            {
                this.spawnPoint = new Vector2(this.gameObject.transform.position.x, 1);
            }
            else
            {
                this.spawnPoint = new Vector2(this.gameObject.transform.position.x, 5);
            }

            this.Spawn(this.orcEnemy[1]);
        }
    }

    private IEnumerator SpawnAttackersLevelTwo()
    {
        if (Time.timeSinceLevelLoad < 30) //The first 30 seconds spawn thugs at the standard random intervals and some occasional berserker
        {
            this.spawnRate = Random.Range(this.minSpawnRate, this.maxSpawnRate);
        }
        else
        {
            this.spawnRate = 3.5f; //The berserkers will appear in a rush wave
        }

        yield return new WaitForSeconds(this.spawnRate);


        if (Time.timeSinceLevelLoad < 30) //The first 30 seconds only thugs appear and an occasional berserker, the last 15 will spawn only berserkers
        {
            this.spawnPoint = new Vector2(this.gameObject.transform.position.x,
                                      Random.Range(this.minLane, this.maxLane)); //thugs will spawn randomly in lanes 1, 2 and 3

            int chooseOrcType = Random.Range(0, 10);

            if(chooseOrcType < 8)
                this.Spawn(this.orcEnemy[0]);
            else
                this.Spawn(this.orcEnemy[1]);
        }
        else
        {
            int laneSelection = Random.Range(0, 2);

            if (laneSelection == 0) //berserkers rush wave will only spawn in lanes 4 and 5
            {
                this.spawnPoint = new Vector2(this.gameObject.transform.position.x, 4);
            }
            else
            {
                this.spawnPoint = new Vector2(this.gameObject.transform.position.x, 5);
            }

            this.Spawn(this.orcEnemy[1]);
        }
    }

    private IEnumerator SpawnAttackersLevelThree()
    {
        if (Time.timeSinceLevelLoad < 40) //The first 40 seconds spawn thugs at lanes 1, 3 and 5 at the standard random intervals and some occasional berserker through lanes 2, 4
        {
            this.spawnRate = Random.Range(this.minSpawnRate, this.maxSpawnRate);
        }
        else if((Time.timeSinceLevelLoad > 40) && (Time.timeSinceLevelLoad < 50)) //a rush wave of Berserkers in lanes 2 and 4
        {
            this.spawnRate = 3.5f; //The berserkers will appear in a rush wave
        }
        else //the last 10 seconds is a rush wave of orc warriors in lanes 1, 3, 5
        {
            this.spawnRate = 2.5f; 
        }

        yield return new WaitForSeconds(this.spawnRate);


        if (Time.timeSinceLevelLoad < 40) //The first 40 seconds only thugs appear and an occasional berserker, the last 10 will spawn only warriors
        {
            this.ChooseLaneForEnemy();

            int chooseOrcType = Random.Range(0, 10);

            if (chooseOrcType < 7) //on this level we make the chance of a berserker appearing slightly higher than level 2
                this.Spawn(this.orcEnemy[0]);
            else
                this.Spawn(this.orcEnemy[1]);
        }
        else if ((Time.timeSinceLevelLoad > 40) && (Time.timeSinceLevelLoad < 50)) //a rush wave of Berserkers in lanes 2 and 4
        {
            int laneSelection = Random.Range(0, 2);

            if (laneSelection == 0) //berserkers rush wave will only spawn in lanes 2 and 4
            {
                this.spawnPoint = new Vector2(this.gameObject.transform.position.x, 2);
            }
            else
            {
                this.spawnPoint = new Vector2(this.gameObject.transform.position.x, 4);
            }

            this.Spawn(this.orcEnemy[1]);
        }
        else
        {
            this.ChooseLaneForEnemy();

            this.Spawn(this.orcEnemy[2]);
        }
    }

    private void ChooseLaneForEnemy()
    {
        do
        {
            int attackerLane = Random.Range(this.minLane, this.maxLane);

            if ((attackerLane == 2) || (attackerLane == 4))
            {
                continue;
            }

            this.spawnPoint = new Vector2(this.gameObject.transform.position.x,
                                          attackerLane); //thugs and warriors will spawn randomly in lanes 1, 2 and 3

            break;

        } while (true);
    }

    private IEnumerator SpawnAttackerDefault()
    {
        this.spawnRate = Random.Range(this.minSpawnRate, this.maxSpawnRate);

        yield return new WaitForSeconds(this.spawnRate);

        this.spawnPoint = new Vector2(this.gameObject.transform.position.x,
                                      Random.Range(this.minLane, this.maxLane));

        this.Spawn(this.orcEnemy[Random.Range(0, this.orcEnemy.Length)]);
    }

    private void Spawn(Attacker orcEnemy)
    {
        Attacker newEnemy = Instantiate<Attacker>(orcEnemy, this.spawnPoint, Quaternion.identity);

        this.SortSpriteOrder(newEnemy, this.spawnPoint.y);

        newEnemy.transform.parent = this.gameObject.transform;
    }

    private void SortSpriteOrder(Attacker inUnit, float inSpawnPointY)
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
        foreach (SpriteRenderer item in inChildrenArray)
        {
            item.sortingOrder = inSortingLayer;
        }
    }
}
