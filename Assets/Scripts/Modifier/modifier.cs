using UnityEngine;

public class Modifier : MonoBehaviour
{
    public enum ModifierType
    {
        SpeedUp,
        SlowDown,
        FlipGravity,
        FlipDirection,
        MakesSmaller,
        Ghost
    }

    private ModifierType type;

    // create a modifier of any type
    public static GameObject Spawn(
        GameObject prefab,
        Vector3 position,
        ModifierType modifierType)
    {
        if (prefab == null)
        {
            Debug.LogError("Modifier prefab is missing!");
            return null;
        }

        GameObject obj = Instantiate(
            prefab,
            position,
            Quaternion.identity
        );

        Modifier modifier = obj.GetComponent<Modifier>();

        if (modifier == null)
        {
            Debug.LogError("Prefab needs a Modifier component!");
            Destroy(obj);
            return null;
        }

        modifier.type = modifierType;
        obj.SetActive(true);

        return obj;
    }

    // shared collision detection
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        BirdModifiers bird =
            other.GetComponent<BirdModifiers>();

        if (bird == null)
            return;

        bird.ActivateModifier(type);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCollect();

        gameObject.SetActive(false);
    }
}