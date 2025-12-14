using System;
using UnityEngine;


public class Bullet : Singleton<Bullet>
{
    // -------------------------------------------------------------------------
    // Public Events:
    // --------------
    //   BulletCollision
    // -------------------------------------------------------------------------

    #region .  Public Events  .

    public static event Action<Collision> BulletCollision = delegate { };

    #endregion



    // -------------------------------------------------------------------------
    // Public Properties:
    // ------------------
    //   Speed
    // -------------------------------------------------------------------------

    #region .  Public Properties  .

    public float Speed = 20f;

    #endregion



    // -------------------------------------------------------------------------
    // Private Properties:
    // -------------------
    //   _rigidbody
    // -------------------------------------------------------------------------

    #region .  Private Properties  .

    private Rigidbody _rigidbody;

    #endregion



    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Awake()
    //   Die()
    //   OnCollisionEnter()
    //   Update()
    // -------------------------------------------------------------------------

    #region .  Awake()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Awake()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Awake()
    {
        this._rigidbody = GetComponent<Rigidbody>();

    }   // Awake()
    #endregion


    #region .  Die()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Die()
    //   Description..:  Destroy the bullet GameObject.
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Die()
    {
        Destroy(this.gameObject);

    }   // Die()
    #endregion


    #region .  OnCollisionEnter()  .
    // -------------------------------------------------------------------------
    //   Method.......:  OnCollisionEnter()
    //   Description..:  Called when the bullet collides wih another GameObject.
    //                   If it is an enemy, call Die() to destroy the bullet.
    //   Parameters...:  Collider - the GameObject the bullet collides with.
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void OnCollisionEnter(Collision otherCollision)
    {
        if (otherCollision.gameObject.CompareTag("Enemy"))
        {
            // Fire this event.
            BulletCollision?.Invoke(otherCollision);

            this.Die();
        }
    }   // OnCollisionEnter()
    #endregion


    #region .  Update()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Update()
    //   Description..:  Handles the bullet movement.
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Update()
    {
        this._rigidbody.velocity = Vector3.forward * this.Speed;

    }   // Update()
    #endregion


}   // class Bullet
