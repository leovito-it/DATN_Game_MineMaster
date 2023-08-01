using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{

    private static T _instance = null;
    private static bool applicationIsQuitting = false;

    //  [System.Obsolete("instance is deprectaed, plase use Instance instaed")]
    //  public static T instance {
    //      get {
    //          return Instance;
    //      }
    //  }
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.Log(typeof(T) + " [Mog.Singleton] is already destroyed. Returning null. Please check HasInstance first before accessing instance in destructor.");
                return null;
            }
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    //                  呼び出し時にインスタンスなければGame Objectを生成
                    _instance = new GameObject().AddComponent<T>();
                    _instance.gameObject.name = _instance.GetType().Name;
                    Debug.LogWarning(typeof(T) + "is nothing");
                }
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }

    }
    protected virtual void Awake()
    {
        if (!HasInstance)
        {
            _instance = (T)this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // インスタンスがあるかを返す、実態はIsDestroyedの逆
    public static bool HasInstance
    {
        get
        {
            return !IsDestroyed;
        }
    }

    // インスタンスが無いかを返す
    public static bool IsDestroyed
    {
        get
        {
            if (_instance == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}