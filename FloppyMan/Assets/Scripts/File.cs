using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class File : MonoBehaviour
{
    public float speed;
    public bool isFileInfected;

	// Use this for initialization
	void Start ()
    {
        speed = GameManager.Instance.speed;
        isFileInfected = false;

        if (Random.Range(0, 1000) % GameManager.Instance.infectedProbability == 0)
            isFileInfected = true;

        if (isFileInfected)
            gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x + -speed * Time.deltaTime, transform.position.y, 0.0f);
        //gameObject.GetComponent<Rigidbody>().velocity = new Vector3(speed * Time.deltaTime * -1.0f, 0.0f, 0.0f);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(-20.0f, transform.position.y, 0.0f), Time.deltaTime);
    }
}
