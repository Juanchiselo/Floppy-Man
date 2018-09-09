using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Events
    public delegate void OnPlayerDeathHandler();
    public event OnPlayerDeathHandler onPlayerDeath;
    #endregion

    [Header("Jump Variables")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;
    public float jumpVelocity = 10.0f;

    public bool isGrounded = true;
    public int jumpsLeft = 2;

    private Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isGrounded = false;

            if (!isGrounded && jumpsLeft > 0)
            {
                jumpsLeft--;
                GetComponent<Rigidbody>().velocity = Vector3.up * jumpVelocity;
            }
        }

        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = true;
            jumpsLeft = 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "File")
        {
            if (other.gameObject.GetComponent<Renderer>().material.color != Color.red)
            {
                GameManager.Instance.score += GameManager.Instance.multiplier;
                GameManager.Instance.multiplier++;
            }
            else
            {
                GameManager.Instance.multiplier = 1;

                if (GameManager.Instance.AntivirusCount > 0)
                    GameManager.Instance.AntivirusCount--;
                else
                {
                    Destroy(gameObject);
                    GameManager.Instance.GameOver();
                }
            }
        }
        else if (other.gameObject.tag == "Antivirus")
        {
            if (other.gameObject.GetComponent<Renderer>().material.color != Color.red)
            {
                if(GameManager.Instance.AntivirusCount < 3)
                    GameManager.Instance.AntivirusCount++;
            }
            else
                GameManager.Instance.AntivirusCount = 0;
        }

        GUIManager.Instance.UpdateGUI();
        Destroy(other.gameObject);
    }
}
