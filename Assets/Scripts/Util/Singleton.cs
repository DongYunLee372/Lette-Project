using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
{ 
    private static T instance; 
    public static T Instance 
    { 
        get 
        { 
            if (instance == null) 
            { 
                //GameObject obj; 
                //obj = GameObject.Find(typeof(T).Name);

                instance = (T)FindObjectOfType(typeof(T));   
            }
            if (instance)
            {
                return instance;
            }
            return null; 
        } 
    } 
    
    public void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    } 
}
