using UnityEngine;

public class ShieldCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
             Debug.Log("Bird collected shield!");//test !!!!

        }
    }
}