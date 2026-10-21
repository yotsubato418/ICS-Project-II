using UnityEngine;

public class ShieldCollectible : MonoBehaviour //gets the game object 
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BirdSpeedBoost boost = other.GetComponent<BirdSpeedBoost>(); 

            if (boost != null) //makes sure bird has the shield (collided w/ it)
            {
                boost.ActivateBoost(); //calls ActivateBoost() instead of duplicating the speed-changing and timer stuff 

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlayCollect();
                }

                gameObject.SetActive(false); //turns the shield off when the pigeon collects it!
            }
        }
    }
}