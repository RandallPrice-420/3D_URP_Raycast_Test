
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Private Properties:
    // -------------------
    //   _instance
    // -------------------------------------------------------------------------

    #region .  Private Properties  .

    private static T _instance;

    #endregion


    // -------------------------------------------------------------------------
    // Public Properties:
    // ------------------
    //   Instance
    // -------------------------------------------------------------------------

    #region .  Public Properties  .

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    _instance = new GameObject(name: "Instance of " + typeof(T)).AddComponent<T>();
                }
            }

            return _instance;
        }

    }   // Instance

    #endregion


}   // class Singleton<T>
