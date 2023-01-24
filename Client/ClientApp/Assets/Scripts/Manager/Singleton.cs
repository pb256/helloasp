using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this as T;
            DontDestroyOnLoad(instance);
        }
    }
}
