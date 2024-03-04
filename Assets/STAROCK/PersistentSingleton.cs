using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace STAROCK
{
    public class PersistentSingleton <T> : MonoBehaviour where T:Component
    {
        [HideInInspector]
        public bool AutoUnparentOnAwake = true;
        
        public static bool HasInstance => _instance != null;
        public static T Current => _instance;
		
        protected static T _instance;
        protected bool _enabled;
        
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T> ();
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject ();
                        obj.name = typeof(T).Name + "_AutoCreated";
                        _instance = obj.AddComponent<T> ();
                    }
                }
                return _instance;
            }
        }
        
        
        protected virtual void Awake ()
        {
            InitializeSingleton();
        }
        
        
        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (AutoUnparentOnAwake)
            {
                this.transform.SetParent(null);
            }

            if (_instance == null)
            {
                //If I am the first instance, make me the Singleton
                _instance = this as T;
                DontDestroyOnLoad (transform.gameObject);
                _enabled = true;
            }
            else
            {
                //If a Singleton already exists and you find
                //another reference in scene, destroy it!
                if(this != _instance)
                {
                    Destroy(this.gameObject);
                }
            }
        }
       
    }

}
