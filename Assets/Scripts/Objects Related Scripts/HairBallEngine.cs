using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HairBallEngine : MonoBehaviour
{
    // Start is called before the first frame update
    Transform playerTransform;//the player's transform
    bool hairBallRight;//where to fire the hair ball
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        if (playerTransform != null)//only when the player is alive the hairBall can be trul fired
        {
            if (playerTransform.position.x > transform.position.x)//check which diraction to throw the hairball
            {
                hairBallRight = true;
            }
            else
            {
                hairBallRight = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HairBallDiraction();
    }
    private void HairBallDiraction()
    {
        if (hairBallRight)//according to the zombie's direcation, fire the hairball at that same direcation
        {
            transform.Translate(2f * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        else
        {
            transform.Translate(-2f * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !collision.isTrigger)//if the player has collided with the hairball (and to skip the hitbox that saves the player from the enemy and to not be hit twice at once)
        {
            Destroy(gameObject);
            PlayerMovment.playerHP--;
            if (PlayerMovment.playerHP > 0)
            {
                playerTransform.position = new Vector2(0, 0);
            }
        }
        else if (collision.tag.Equals("Weapon"))//if the hairball hits an active ability destroy it self and do the following  
        {
            if (PlayerMovment.isDice)//if the player is dice lower the shield hit point for the current ability 
            {
                PlayerMovment.shieldHP--;
            }
            else
            {
                PlayerMovment.bladeInterrupted = true;// if the player is slice stop the blade ability completely 
            }
            Destroy(gameObject);
        }
        else if (!collision.tag.Equals("Enemy") && !collision.tag.Equals("HairBall"))//destroy at any other collision that isn't self or an enemy
        {
            Destroy(gameObject);
        }
    }
}
