using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public Transform playerTransform;//the player's transform
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //keep the camera on the player as long as the player doesn't reach the borders of the level
        if (playerTransform != null)
        {
            if (playerTransform.position.x >= 0 && playerTransform.position.x <= 101.5f && playerTransform.position.y <= 1f && playerTransform.position.y >= 0)
            {
                transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
            }
            else if (playerTransform.position.x >= 0 && playerTransform.position.x <= 101.5f)
            {
                transform.position = new Vector3(playerTransform.position.x, transform.position.y, -10f);
            }
            else if (playerTransform.position.y <= 1f && playerTransform.position.y >= 0)
            {
                transform.position = new Vector3(transform.position.x, playerTransform.position.y, -10f);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            }
        }
    }
}
