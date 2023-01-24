using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T instance { get; private set; }

    protected void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this as T;
    }
}
