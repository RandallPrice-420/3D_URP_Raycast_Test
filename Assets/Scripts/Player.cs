using UnityEngine;


public class Player : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Public Properties:
    // ------------------
    //   BulletPrefab
    //   DeathEffectPrefab
    //   MuzzleFlashPrefab
    //   MuzzleFlashFirePoint
    //   MaxPosition
    //   MinPosition
    //   FireDirection
    //   RaycastDistance
    //   Speed
    // -------------------------------------------------------------------------

    #region .  Public Properties  .

    public GameObject BulletPrefab;
    public GameObject DeathEffectPrefab;
    public GameObject MuzzleFlashPrefab;
    public Transform  MuzzleFlashFirePoint;
    public Vector3    FireDirection   = Vector3.forward;
    public Vector3    MaxPosition     = new( 14, 0,  14);
    public Vector3    MinPosition     = new(-14, 0, -14);
    public float      RaycastDistance = 27f;
    public float      Speed           = 10f;

    #endregion


    // -------------------------------------------------------------------------
    // Private Properties:
    // -------------------
    //   _deathEffect
    //   _moveDirection
    //   _muzzleFlashEffect
    //   _raycastHit
    //   _rigidbody
    //   _horizontalInput
    //   _verticalInput
    //
    //   //_isShooting
    // -------------------------------------------------------------------------

    #region .  Private Properties  .

    private GameObject _deathEffect;
    private Vector3    _moveDirection;
    private GameObject _muzzleFlashEffect;
    private RaycastHit _raycastHit;
    private Rigidbody  _rigidbody;
    private float      _horizontalInput;
    private float      _verticalInput;

    //private readonly bool _isShooting = false;

    #endregion


    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Awake()
    //   BulletCollision()
    //   OnDisable()
    //   OnEnable()
    //   FixedUpdate()
    //   Shoot()
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


    #region .  BulletCollision()  .
    // -------------------------------------------------------------------------
    //   Method.......:  BulletCollision()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void BulletCollision(Collision otherCollision)
    {
        //Debug.Log($"Player.BulletCollision():  otherCollision = {otherCollision.gameObject.name}, otherCollision.contacts = {otherCollision.contacts}");

        this._deathEffect = Instantiate(this.DeathEffectPrefab, otherCollision.contacts[0].point, Quaternion.identity);

        switch (otherCollision.gameObject.name)
        {
            case "Enemy1":
            case "Enemy2":
            case "Enemy3":
                SoundManager.Instance.PlaySound($"Pop_{UnityEngine.Random.Range(1, 8)}");
                break;

            case "Enemy4":
                SoundManager.Instance.PlaySound("Explosion_1");
                break;
        }

        Destroy(this._deathEffect, 0.5f);

    }   // BulletCollision()
    #endregion


    #region .  OnDisable()  .
    // -------------------------------------------------------------------------
    //   Method.......:  OnDisable()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void OnDisable()
    {
        Bullet.BulletCollision -= this.BulletCollision;

    }   // OnDisable()
    #endregion


    #region .  OnEnable()  .
    // -------------------------------------------------------------------------
    //   Method.......:  OnEnable()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void OnEnable()
    {
        Bullet.BulletCollision += this.BulletCollision;

    }   // OnEnable()
    #endregion


    #region .  FixedUpdate()  .
    // -------------------------------------------------------------------------
    //   Method.......:  FixedUpdate()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void FixedUpdate()
    {
        this.transform.Translate(this.Speed * Time.deltaTime * this._moveDirection);

    }   // FixedUpdate()
    #endregion


    #region .  Shoot()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Shoot()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Shoot()
    {
        if (Physics.Raycast(this.MuzzleFlashFirePoint.position, this.transform.TransformDirection(this.FireDirection), out this._raycastHit, 100f))
        {
            //Debug.DrawRay(this.MuzzleFlashFirePoint.position,
            //              this.transform.TransformDirection(this.FireDirection) * this._raycastHit.distance,
            //              Color.red);

            this._muzzleFlashEffect = Instantiate(this.MuzzleFlashPrefab, this.MuzzleFlashFirePoint.position, Quaternion.identity);

            Instantiate(this.BulletPrefab, this.MuzzleFlashFirePoint.position, this.transform.rotation);
            SoundManager.Instance.PlaySound("Laser_Gun");

            Destroy(this._muzzleFlashEffect, 0.5f);
        }

    }   // Shoot()
    #endregion


    #region .  Update()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Update()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private void Update()
    {
        //Debug.DrawRay(this.MuzzleFlashFirePoint.position, this.transform.TransformDirection(this.FireDirection) * this.RaycastDistance, Color.red);

        this._horizontalInput = Input.GetAxis("Horizontal");
        this._verticalInput   = Input.GetAxis("Vertical");
        this._moveDirection   = new Vector3(this._horizontalInput, 0, this._verticalInput);

        //if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Shoot();
        }

    }   // Update()
    #endregion


}   // class Player
