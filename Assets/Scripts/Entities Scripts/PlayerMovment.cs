using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovment : MonoBehaviour
{
    public static bool isDice;
    public static bool bladeInterrupted;
    public static int shieldHP;
    public static int playerHP;
    public static int kills;
    //animation controllers for the player and the weapon 
    public Animator playerAnim;
    public Animator weaponAnim;
    //speed values
    public Rigidbody2D playerRB2D;
    float MAX_SPEED = 10f;
    float xSpeed;
    //jump variables 
    bool canJump;
    bool jumpSave = false;
    //variables For the weapons cooldowns
    float bladeCooldown = 2f;
    float shieldCooldown = 4f;
    float lastTimeUsedShield;
    float lastTimeUsedBlade;
    float shieldDuration = 2f;
    float bladeDuration = 0.5f;
    public static CoolDowns bladeCD;
    public static CoolDowns shieldCD;
    public static bool firstTimeUse;

    // Start is called before the first frame update
    void Start()
    {
        //setting the variables 
        int mouseDecider = UnityEngine.Random.Range(0,2);
        if (mouseDecider > 0)
        {
            isDice = true;
        }
        else
        {
            isDice = false;
        }
        playerAnim.SetBool("IsDice", isDice);//switch to the mouse  
        weaponAnim.SetBool("IsDice", isDice);//switch to the mouse 
        kills = 0;
        playerHP = 3;
        xSpeed = 0f;
        lastTimeUsedShield = 0f;
        lastTimeUsedBlade = 0f;
        firstTimeUse = true;
        bladeInterrupted = false;
        shieldHP = 3;
        bladeCD = new CoolDowns(bladeCooldown, bladeDuration, lastTimeUsedBlade);
        shieldCD = new CoolDowns(shieldCooldown, shieldDuration, lastTimeUsedShield);
        //setting the variables 
        Physics2D.IgnoreCollision(GetComponent<PolygonCollider2D>(), GameObject.Find("Weapon").GetComponent<BoxCollider2D>());//ignore the collisions between the weapons and the player 
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHP <= 0)//the player is dead
        {
            SceneManager.LoadScene("Game Over");
            Destroy(gameObject);
        }
        else if (!CanvasManager.isPaused)//dont want to move if in paused State 
        {
            Movment();
            if (!weaponAnim.GetBool("IsUseAbility") && !weaponAnim.GetBool("IsInAbility"))//not using an ability at all.
            {
                UseAbility();
            }
            if (!weaponAnim.GetBool("IsInAbility") && weaponAnim.GetBool("IsUseAbility"))//only drawing out the weapon
            {
                AbilityActivasion();
            }
            if (weaponAnim.GetBool("IsInAbility") && weaponAnim.GetBool("IsUseAbility"))//fully activated
            {
                checkAbilityInUse();
            }
            SwitchMouse();
        }
    }
    /// <summary>
    /// Movment controlls the movment of the player
    /// </summary>
    private void Movment()
    {
        if (Input.GetKey(KeyCode.RightArrow))//pressing right
        {
            if (xSpeed < 0)//if the speed is still to the left slow the player down fast
            {
                xSpeed += 25f * Time.deltaTime;
            }
            if (xSpeed < MAX_SPEED)//can speed up the player to the disaired diraction
            {
                xSpeed += 8f * Time.deltaTime;
            }
            else//reached max speed
            {
                xSpeed = MAX_SPEED;
            }
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))//pressing left 
        {
            if (xSpeed > 0)//if the speed is still to the right slow the player down fast
            {
                xSpeed -= 25f * Time.deltaTime;
            }
            if (xSpeed > -MAX_SPEED)//can speed up the player to the disaired diraction
            {
                xSpeed -= 8f * Time.deltaTime;
            }
            else//reached max speed
            { 
                xSpeed = -MAX_SPEED; 
            }
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }
        else//if the player is not pressing left or right, stop him fast BUT not immediatly 
        {
            if (Math.Abs(xSpeed) - 25f * Time.deltaTime < 0)//fully Stopped
            {
                xSpeed = 0;
            }
            else if (xSpeed > 0)
            {
                xSpeed -= 25f * Time.deltaTime;
            }
            else if(xSpeed < 0)
            {
                xSpeed += 25f * Time.deltaTime;
            }
        }
        playerAnim.SetBool("IsWalking", Math.Abs(xSpeed) != 0);//setting walking animation according if the player moves
        weaponAnim.SetBool("IsWalking", Math.Abs(xSpeed) != 0);// setting walking animation according if the player moves
        playerAnim.SetBool("IsSprinting", Math.Abs(xSpeed) == MAX_SPEED);// setting sprinting animation according if the player moves
        weaponAnim.SetBool("IsSprinting", Math.Abs(xSpeed) == MAX_SPEED);// setting sprinting animation according if the player moves
        if (Input.GetKeyDown(KeyCode.Space) && canJump)//only when pressing space and when the player is on he ground
        {
            canJump = false;
            playerRB2D.AddForce(new Vector2(0, 400f));
            playerAnim.SetBool("IsJump", !canJump);// setting jumping animation
            weaponAnim.SetBool("IsJump", !canJump);// setting jumping animation 
        }
        playerRB2D.velocity = new Vector2(xSpeed, playerRB2D.velocity.y);

    }
    /// <summary>
    /// UseAbility checks if the player wants to use an ability 
    /// </summary>
    private void UseAbility()
    {
        if (Input.GetKeyDown(KeyCode.E))//The player can only use an ability when the cooldown is off while pressing E 
        {
            if (isDice)//according to which mouse the player is currently playing 
            {
                if (shieldCD.IsCooldownOff() || firstTimeUse)//is the cooldown off or is it the first ability used?
                { 
                    shieldHP = 3;
                    weaponAnim.SetBool("IsUseAbility", true);//start Activesion  animation 
                }
            }
            else
            {
                if (bladeCD.IsCooldownOff() || firstTimeUse)//is the cooldown off or is it the first ability used?
                {
                    bladeInterrupted = false;
                    weaponAnim.SetBool("IsUseAbility", true);//start Activesion  animation 
                }
            }
        }
    }
    /// <summary>
    /// AbilityActivasion Starts the activasion of the ability, not the ability duration it self
    /// </summary>
    private void AbilityActivasion()
    {
            if (weaponAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Ability_Activasion"))//check if the animation has been fully done if not, set the duration for the ability.
            {
                weaponAnim.SetBool("IsInAbility", true);//ability is fully out
            }
            else if (isDice)//set the time for the duration until the animation to draw the ability out is done
            {
                shieldCD.SetTimeWhenActive(Time.time);
            }
            else
            {
                bladeCD.SetTimeWhenActive(Time.time);
            }
    }
    /// <summary>
    /// checkAbilityInUse checks if the ability duration has reached the end and resets the ability functions so that the player can use an ability only after the deactivasion and the cooldowns or fully done
    /// </summary>
    private void checkAbilityInUse()
    {
        if (isDice)
        {
                if ((shieldCD.IsAbilityFinish()  && weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Active")) || (shieldHP <= 0 && weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Active")))
                {
                    shieldCD.LastTimeUsed = Time.time;//reset cooldown
                    weaponAnim.SetBool("IsUseAbility", false);
                    weaponAnim.SetBool("IsInAbility", false);
                    if (firstTimeUse)//if it was the first time ture it to false cause the nwext one isn't
                    {
                        firstTimeUse = false;
                    }
                }
        }
        else
        {
                if ((bladeCD.IsAbilityFinish() && weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Active")) || (bladeInterrupted && weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Active")))
                {
                    bladeCD.LastTimeUsed = Time.time;//reset cooldown
                    weaponAnim.SetBool("IsUseAbility", false);
                    weaponAnim.SetBool("IsInAbility", false);
                    if (firstTimeUse)//if it was the first time ture it to false cause the nwext one isn't
                    {
                        firstTimeUse = false;
                    }
                }
        }
    }
    /// <summary>
    /// SwitchMouse switches between Dice and Slice when the are not preforming/deactivating an ability and Q is pressed
    /// </summary>
    private void SwitchMouse()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !weaponAnim.GetBool("IsUseAbility") && !weaponAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Ability_Activasion"))
        {
            isDice = !isDice;
            playerAnim.SetBool("IsDice", isDice);//switch to the next mouse  
            weaponAnim.SetBool("IsDice", isDice);//switch to the next mouse 
        }
    }
    /// <summary>
    /// OnCollisionEnter2D is called every frame a collision of the polygon collider that has been made.
    /// </summary>
    /// <param name="collision"> the collision of the object the player collided with</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag.Equals("Enemy"))//if player thouches an enemy.
        {
            if (jumpSave)//if the box collider of the player also hit the enemy the player is saved and makes a jump on the enemy and kills it 
            {
                kills++;
                Destroy(collision.gameObject);
                playerRB2D.velocity = new Vector2(playerRB2D.velocity.x,0f);
                playerRB2D.AddForce(new Vector2(0f, 400f));
                jumpSave = false;
            }
            else//else the player Loses a life and respawns.
            {
                playerHP--;
                if (playerHP > 0)
                {
                    transform.position = new Vector2(0, 0);
                }
            }
        }
        if (collision.collider.tag.Equals("Ground"))//check if the player can jump 
        {
            canJump = true;
            playerAnim.SetBool("IsJump", !canJump);//currently not jumping
            weaponAnim.SetBool("IsJump", !canJump);//currently not jumping
        }
        else
        {
            canJump = false;
        }
        if (collision.collider.tag.Equals("DeathFloor"))//if the player fell to the pit lose a life and respawn
        {
            playerHP--;
            if (playerHP > 0)
            {
                transform.position = new Vector2(0, 0);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision">the collider of the object the player's box collider hit</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy"))
        {
            jumpSave = true;
        }
    }
}
