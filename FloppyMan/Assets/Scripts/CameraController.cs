using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5.0f;
    private Vector3 playerPosition = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        // Set the color of the cube to red.
        //GetComponent<Renderer>().material.color = Color.red;

        // Freeze the rotation in all axes.
        //transform.GetComponent<Rigidbody>().constraints =
        //    RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly keep the player moving to the right.
        playerPosition.x = transform.position.x + speed * Time.deltaTime;
        playerPosition.y = transform.position.y;
        playerPosition.z = 0;
        transform.position = playerPosition;
    }

    public void SetSpeed()
    {
        //speed = GameManager.Instance.GetSpeed();
    }
}
