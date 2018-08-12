using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSpawner : MonoBehaviour
{
    private Transform target;
    public int targetIndex;
    public float speed;

	// Use this for initialization
	void Start ()
    {
        targetIndex = 0;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        target = GameManager.Instance.targets[targetIndex % GameManager.Instance.targets.Length].transform;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
            targetIndex++;
    }
}
