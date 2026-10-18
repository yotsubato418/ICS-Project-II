using UnityEngine;

public class ShieldCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BirdSpeedBoost boost = other.GetComponent<BirdSpeedBoost>();

            if (boost != null)
            {
                boost.ActivateBoost();
                gameObject.SetActive(false);
            }
        }
    }
}