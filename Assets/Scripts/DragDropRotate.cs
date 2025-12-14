using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class DragDropRotate : MonoBehaviour
{
    // =========================================================================
    // Instructions:
    //   Add this script to any GameObjects, but it is recommended to add it to
    //   its own empty GameObject.
    //   
    //   The GameObjects you want to drag and drop must have a Collider component
    //   (it can be any of the Collider components).
    //
    // Input:
    //   Unity Input System (New)
    //
    // Reference:
    //   Easy Drag and Drop with Input System 2D | 3D - Unity Tutorial - YouTube
    //   https://www.youtube.com/watch?v=HfqRKy5oFDQ&t=573s
    // =========================================================================


    // -------------------------------------------------------------------------
    // Public Properties:
    // ------------------
    //   MouseDragPhysicsSpeed
    //   MouseDragSpeed
    //   RotateSpeed
    // -------------------------------------------------------------------------

    #region .  Public Properties  .

    public float MouseDragPhysicsSpeed = 10.0f;
    public float MouseDragSpeed        = 0.1f;
    public float RotateSpeed           = 20f;

    #endregion


    // -------------------------------------------------------------------------
    // SerializeField Properties:
    // --------------------------
    //   _mouseLeftClick
    //   _mouseRightClick
    //   _mousePosition
    // -------------------------------------------------------------------------

    #region .  SerializeField Properties  .

    [Header("Input Actions")]
    [SerializeField] private InputAction _mouseLeftClick;
    [SerializeField] private InputAction _mouseRightClick;
    [SerializeField] private InputAction _mousePosition;

    #endregion


    // -------------------------------------------------------------------------
    // Private Properties:
    // -------------------
    //   _isDragging
    //   _isRotating
    //   _mainCamera
    //   _startMousePosition
    //   _TMP_ObjectName
    //   _velocity
    // -------------------------------------------------------------------------

    #region .  Private Properties  .

    private Vector3  _currentMousePosition;
    private bool     _isDragging;
    private bool     _isRotating;
    private Camera   _mainCamera;
    private float    _startMousePosition;
    private TMP_Text _TMP_ObjectName = null;
    private Vector3  _velocity       = Vector3.zero;

    private bool _isClickedOn
    {
        get
        {
            Ray ray = this._mainCamera.ScreenPointToRay(this._currentMousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.transform == this.transform;
            }

            return false;
        }
    }
    #endregion


    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   Awake()
    //   DoMousePress()
    //   DragUpdate()
    //   OnDisable()
    //   OnEnable()
    //   MouseLeftPressed()
    // -------------------------------------------------------------------------

    #region .  Awake()  .
    // -------------------------------------------------------------------------
    //   Method.......:  Awake()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private void Awake()
    {
        this._mainCamera = Camera.main;

        try
        {
            this._TMP_ObjectName      = GameObject.Find("TMP_ObjectName").GetComponent<TMP_Text>();
            this._TMP_ObjectName.text = "";
        }
        catch { }

    }   // Awake()
    #endregion


    #region .  DoMousePress()  .
    // -------------------------------------------------------------------------
    //   Method.......:  DoMousePress()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private void DoMousePress(string coroutineName)
    {
        Ray ray = this._mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit hit);

        if (hit.collider != null)
        {
            if (this._TMP_ObjectName != null)
            {
                this._TMP_ObjectName.text = hit.collider.name;
            }

            StartCoroutine(coroutineName, hit.collider.gameObject);
        }

    }   // DoMousePress()
    #endregion


    #region .  DragUpdate()  .
    // -------------------------------------------------------------------------
    //   Method.......:  DragUpdate()
    //   Description..:  
    //   Parameters...:  GameObject
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------

    private IEnumerator DragUpdate(GameObject clickedObject)
    {
        float initialDistance = Vector3.Distance(clickedObject.transform.position, this._mainCamera.transform.position);
        clickedObject.TryGetComponent<Rigidbody>(out var rigidbody);

        while (this._mouseLeftClick.ReadValue<float>() != 0)
        {
            Ray ray = this._mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (rigidbody != null)
            {
                Vector3 direction  = ray.GetPoint(initialDistance) - clickedObject.transform.position;
                rigidbody.velocity = direction * this.MouseDragPhysicsSpeed;

                yield return new WaitForFixedUpdate();
            }
            else
            {
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position,
                                                                      ray.GetPoint(initialDistance),
                                                                      ref _velocity,
                                                                      this.MouseDragSpeed);
                yield return null;
            }
        }

    }   // DragUpdate()
    #endregion


    #region .  OnDisable()  .
    // -------------------------------------------------------------------------
    //   Method.......:  OnDisable()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private void OnDisable()
    {
        this._mouseLeftClick.Disable();
        this._mouseLeftClick.performed -= _ => { if (this._isClickedOn) DoMousePress("DragUpdate"); };
        this._mouseLeftClick.canceled  -= _ => { this._isDragging = false; };

        this._mouseRightClick.canceled -= _ => { this._isRotating = false; };

        this._mousePosition.Disable();
        this._mousePosition.performed  -= context => { this._currentMousePosition = context.ReadValue<Vector2>(); };

    }   // OnDisable()
    #endregion


    #region .  OnEnable()  .
    // -------------------------------------------------------------------------
    //   Method.......:  OnEnable()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------

    private void OnEnable()
    {
        this._mouseLeftClick.Enable();
        this._mouseLeftClick.performed  += _ => { if (this._isClickedOn) DoMousePress("DragUpdate"); };
        this._mouseLeftClick.canceled   += _ => { this._isDragging = false; };

        this._mouseRightClick.Enable();
        this._mouseRightClick.performed += _ => { if (this._isClickedOn) DoMousePress("RotateUpdate"); };
        this._mouseRightClick.canceled  += _ => { this._isRotating = false; };

        this._mousePosition.Enable();
        this._mousePosition.performed   += context => { this._currentMousePosition = context.ReadValue<Vector2>(); };

    }   // OnEnable()
    #endregion


    #region .  RotateUpdate()  .
    // -------------------------------------------------------------------------
    //   Method.......:  RotateUpdate()
    //   Description..:  Rotates the given GameObject around the X axis based on
    //                   the mouse movement.  If FreezeRotationY is true, the
    //                   rotation will be frozen.
    //   Parameters...:  GameObject:  the object to rotate
    //   Returns......:  Vector3
    // -------------------------------------------------------------------------
    private IEnumerator RotateUpdate(GameObject clickedObject)
    {
        while (_mouseRightClick.ReadValue<float>() != 0)
        {
            float mousePositionX = Mouse.current.position.ReadValue().x;
            float mouseMovement  = mousePositionX - this._startMousePosition;

            clickedObject.transform.Rotate(Vector3.up, -mouseMovement * this.RotateSpeed * Time.deltaTime);
            this._startMousePosition = mousePositionX;

            yield return null;
        }

    }   // RotateUpdate()
    #endregion


}   // class DragDropRotate
