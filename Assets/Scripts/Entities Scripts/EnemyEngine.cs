using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEngine : MonoBehaviour
{
    // Start is called before the first frame update
    Transform playerTransform;//the player's transform
    float respawnX;//where i want to respawn my enemies x incase the fall into the pit
    float respawnY;//where i want to respawn my enemies y incase the fall into the pit
    public Rigidbody2D EnemyRB;
    public GameObject hairBallPreFab;
    CoolDowns hairBallCD;
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        respawnX = transform.position.x;
        respawnY = transform.position.y;
        hairBallCD = new CoolDowns(3f,Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        MoveZombieCat();
        ShotHairBall();
    }
    private void MoveZombieCat()//move the zombie cat close to the player
    {
        if (playerTransform != null)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1);
                EnemyRB.velocity = new Vector2(1f, EnemyRB.velocity.y);

            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1);
                EnemyRB.velocity = new Vector2(-1f, EnemyRB.velocity.y);
            }

        }
    }
    private void ShotHairBall()//when the zombie can shot a hair ball it will update the time where he can use it again
    {
        if (hairBallCD.IsCooldownOff() && playerTransform != null)
        {
            hairBallCD.LastTimeUsed = Time.time;
            Instantiate(hairBallPreFab, transform.position, hairBallPreFab.transform.localRotation);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Weapon") && !PlayerMovment.isDice)//if slice is using his ability kill the zombie and add to the kill count
        {
            Destroy(gameObject);
            PlayerMovment.kills++;
        }
        if (collision.collider.tag.Equals("DeathFloor"))//if the zombie steps over the edge of a platform, respawn him in his starting point 
        {
            transform.position = new Vector2(respawnX, respawnY);
        }
    }
}
