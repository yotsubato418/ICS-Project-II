using UnityEngine;

public class CollectibleCollision : MonoBehaviour //gets the game object !
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BirdSpeedBoost boost = other.GetComponent<BirdSpeedBoost>(); 

            if (boost != null) 
            {
                boost.ActivateBoost(); 

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayCollect();
                }

                gameObject.SetActive(false); //turns the shield off when the pigeon collects it!
            }
        }
    }
}