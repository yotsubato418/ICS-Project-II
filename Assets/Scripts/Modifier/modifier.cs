using UnityEngine;

public class Modifier : MonoBehaviour
{
    // add modifier types here
    public enum ModifierType
    {
        SpeedUp
    }

    private ModifierType type;

    // spawn a modifier of a specific type at a specific position
    private static GameObject Spawn(
        GameObject prefab,
        Vector3 position,
        ModifierType modifierType)
    {
        GameObject obj = Instantiate(
            prefab,
            position,
            Quaternion.identity
        );

        Modifier modifier = obj.GetComponent<Modifier>();

        if (modifier == null)
        {
            Debug.LogError(
                "Modifier prefab needs a Modifier component!"
            );

            Destroy(obj);
            return null;
        }

        modifier.type = modifierType;
        return obj;
    }

    // spawn a speedup modifier at a specific position
    public static GameObject SpeedUp(
        GameObject prefab,
        Vector3 position)
    {
        return Spawn(
            prefab,
            position,
            ModifierType.SpeedUp
        );
    }

    // collision detection shared by all modifiers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
            
        SpeedUp bird = other.GetComponent<SpeedUp>();

        if (bird == null)
            return;

        // decide which effect to activate
        switch (type)
        {
            case ModifierType.SpeedUp:
                bird.Activate();
                break;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCollect();
        }

        gameObject.SetActive(false);
    }
}