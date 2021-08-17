using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [Tooltip("Level duration in seconds.")]
    [SerializeField] float levelTime = 10.0f;

    [SerializeField] LevelManager targetObject;

    private Slider levelTimeSlider;
    private bool isLevelFinished = false;

    private void Start()
    {
        this.levelTimeSlider = this.gameObject.GetComponent<Slider>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!this.isLevelFinished)
        {
            this.levelTimeSlider.value = ((Time.timeSinceLevelLoad / levelTime) * 100);

            this.isLevelFinished = (Time.timeSinceLevelLoad >= this.levelTime);

            if (this.isLevelFinished)
            {
                this.targetObject.SendMessage("StopSpawningAttackers");
                this.gameObject.GetComponentInChildren<Animator>().speed = 0;
            }
        }
    }
}
