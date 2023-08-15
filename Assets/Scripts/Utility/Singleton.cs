using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = GameObject.Find(ClassName);

                if (obj == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        T prefab = Resources.Load<T>(ClassName);

                        if (prefab)
                        {
                            _instance = Instantiate(prefab);
                        }
                        else
                        {
                            _instance = new GameObject()
                            {
                                name = $"{ClassName} Singleton"
                            }.AddComponent<T>();
                        }
                    }
                }
                else
                    _instance = obj.GetComponent<T>();
            }
            return _instance;
        }
        set { _instance = value; }
    }

    static T _instance;

    protected virtual void Awake()
    {
        Instance = (T)this;
        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool dontDestroyOnLoad = false;

    public static string ClassName => typeof(T).Name;
}
