using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public Animator switch_Q_Icon;
    public Image ability_E_Icon;
    public Image lifes;
    public Sprite[] lifeSprites;
    public Sprite ability_E_Blade;
    public Sprite ability_E_Shield;
    public TMP_Text pauseText;
    public static bool isPaused;
    float startTime;
    string formattedTime;
    TMP_Text timeText;
    TMP_Text zombieKills;
    // Start is called before the first frame update
    void Start()
    {
        //setting the variables 
        isPaused = false;
        startTime = Time.time;
        timeText = GameObject.Find("Time Text").GetComponent<TMP_Text>();
        zombieKills = GameObject.Find("Zombies Killed").GetComponent<TMP_Text>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player") != null && PlayerMovment.playerHP > 0)//as long as the player is alive preform the next actions
        {
           Check_For_Pause();
           Update_Q_Icon();
           Update_E_Icon();
           UpdateLifes();
           UpdateTime();
           UpdateKills();
        }
    }
    /// <summary>
    /// Check_For_Pause is checking if the player ever pressed ESC to pause the game
    /// </summary>
    private void Check_For_Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0f;//stop the floww of time in the game
                if (pauseText != null)
                {
                    pauseText.gameObject.SetActive(true);
                    pauseText.text = "Paused";
                }
            }
            else
            {
                Time.timeScale = 1f;//resume it 
                if (pauseText != null)
                    pauseText.gameObject.SetActive(false);
            }
        }
    }
    private void UpdateKills()//show the amount of zombie kills the player got.
    {
        zombieKills.text = ": " + PlayerMovment.kills;
    }
    /// <summary>
    /// Show the amount of the time that has passed since starting the level.
    /// </summary>
    private void UpdateTime()
    {
        // Format the elapsed time as minutes and seconds
        formattedTime = string.Format("{0:00}:{1:00}", Mathf.FloorToInt((Time.time - startTime) / 60), Mathf.FloorToInt((Time.time - startTime) % 60));
        timeText.text = "Time: " + formattedTime;
    }
    /// <summary>
    /// UpdateLifes Shows the current amount of lifes the player has
    /// </summary>
    private void UpdateLifes()
    {
        if (!PlayerMovment.isDice)//if dice show dice in the icon 
        {
            lifes.sprite = lifeSprites[PlayerMovment.playerHP - 1];
        }
        else
        {
            lifes.sprite = lifeSprites[lifeSprites.Length - PlayerMovment.playerHP];
        }
    }
    private void Update_Q_Icon()//update the icon according to which mouse (Dice/Slice) the player is currently playing
    {
        switch_Q_Icon.SetBool("IsDice", PlayerMovment.isDice);
    }
    /// <summary>
    /// Update E Icon Displays the current ability aviliable  according to which mouse the player is playing
    /// </summary>
    private void Update_E_Icon()
    {
        if (PlayerMovment.isDice)//is Dice?
        {
            ability_E_Icon.sprite = ability_E_Shield;//show the shield
            if ((PlayerMovment.shieldCD.IsCooldownOff() || PlayerMovment.firstTimeUse) )//when the cooldown is full it will refill the icon
            {
                ability_E_Icon.fillAmount = 1f;

            }
            else
            {
                ability_E_Icon.fillAmount = (Time.time - PlayerMovment.shieldCD.LastTimeUsed ) / PlayerMovment.shieldCD.CoolDownTime;
            }
        }
        else//not dice
        {
            ability_E_Icon.sprite = ability_E_Blade;//show the blade
            if (PlayerMovment.bladeCD.IsCooldownOff() || PlayerMovment.firstTimeUse)//when the cooldown is full it will refill the icon
            {
                ability_E_Icon.fillAmount = 1f;
            }
            else
            {
                ability_E_Icon.fillAmount = (Time.time - PlayerMovment.bladeCD.LastTimeUsed) / PlayerMovment.bladeCD.CoolDownTime;
            }
        }
        
    }
}
