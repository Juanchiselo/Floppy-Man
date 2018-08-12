using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Events
    public delegate void OnPlayerDeathHandler();
    public event OnPlayerDeathHandler onPlayerDeath;
    #endregion

    public float speed = 5.0f;
    public float jumpHeight = 200.0f;
    public bool isGrounded = true;
    private bool canChangeColor = true;
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
        //// Constantly keep the player moving to the right.
        //playerPosition.x = transform.position.x + speed * Time.deltaTime;
        //playerPosition.y = transform.position.y;
        //playerPosition.z = 0;
        //transform.position = playerPosition;

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    public void SetSpeed()
    {
        //speed = GameManager.Instance.GetSpeed();
    }

    // Jump function
    public void Jump()
    {
        if (isGrounded)
        {
            isGrounded = false;

            Vector3 jump = new Vector3(0.0f, jumpHeight, 0.0f);

            GetComponent<Rigidbody>().AddForce(jump);
        }
    }

    public void Slide()
    {
        if (isGrounded)
        {
            Debug.Log("I'm Sliding!");
        }
    }

    public void ChangeColor(string color)
    {
        if (canChangeColor)
        {
            if (string.Equals(color, "Red"))
                GetComponent<Renderer>().material.color = Color.red;

            if (string.Equals(color, "Blue"))
                GetComponent<Renderer>().material.color = Color.blue;
        }
    }


    // To detect whether the player is grounded.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
            isGrounded = true;

        if (collision.gameObject.tag == "Obstacle")
        {
            if (GetComponent<Renderer>().material.color
               == collision.gameObject.GetComponent<Renderer>().material.color)
            {
                collision.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            }
            else
            {
                if (onPlayerDeath != null)
                {
                    onPlayerDeath();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "File")
            Destroy(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Obstacle")
    //        canChangeColor = true;
    //}
}
