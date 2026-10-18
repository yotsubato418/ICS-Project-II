using UnityEngine;

public class ShieldCollectible : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BirdSpeedBoost boost = other.GetComponent<BirdSpeedBoost>();

            if (boost != null) //makes sure bird has the shield (collided w/ it)
            {
                boost.ActivateBoost();
                gameObject.SetActive(false); //turns the shield off when the pigeon collects it!
            }
        }
    }
}