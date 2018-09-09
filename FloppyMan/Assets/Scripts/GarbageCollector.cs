using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Resets the multiplier if the player missed a non-infected file.
        if (other.gameObject.GetComponent<Renderer>().material.color != Color.red)
            GameManager.Instance.ResetMultiplier();
            
        Destroy(other.gameObject);
    }
}
