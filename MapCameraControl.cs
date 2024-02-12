using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCameraControl : MonoBehaviour
{
    private PlayerInputs cameraActions;
    private InputAction movement;
    private Transform cameraTransform;

    private int cameraMode;//0 = free move, 1 = selected

    //horizontal motion
    [SerializeField]
    private float maxSpeed = 5f;
    private float speed;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float damping = 15f;

    //Vertical motion - zooming
    [SerializeField]
    private float stepSize = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minHeight = 5f;
    [SerializeField]
    private float maxHeight = 50f;
    [SerializeField]
    private float zoomSpeed = 2f;

    //rotation
    [SerializeField]
    private float maxRotationSpeed = 1f;

    //screen edge motion
    [SerializeField]
    [Range(0f, 0.1f)]
    private float edgeTolerance = 0.05f;
    [SerializeField]
    private bool useScreenEdge = true;

    private Vector3 targetPosition;
    private Vector3 currentPosition;

    private float zoomHeight;

    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    Vector3 startDrag;

    private GameObject orbitTargetObject;
    [SerializeField]
    private Vector3 orbitTargetPosition;

    [SerializeField]
    private float orbitDistance = 4.0f;

    [SerializeField]
    private float orbitHeight = 5.0f;


    [SerializeField]
    private float orbitRotationSpeed = 10.0f;


    private float currentRotationAngle;
    private float currentHeight;

    public float targetHeight = 0f;

    private Vector3 lookAtLocation;

    [SerializeField]public Transform target; // The target position to pan towards
    public float panSpeed = 2.0f;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        //SetMode(1);
    }
    public void SetMode(int x)
    {
        cameraMode = x;
        switch (cameraMode)
        {
            case 0:
                OnEnable();
                break;
            case 1:
                OnDisable();
                break;
            default:
                break;
        }
    }
    public void SetRotateTarget(GameObject x)
    {
        orbitTargetObject = x;
    }

    private void MoveToSeeTarget()
    {
        // Calculate the desired position for the camera to keep the target in the center.
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        // Smoothly interpolate the camera's position towards the target.
        transform.position = Vector3.Lerp(transform.position, targetPosition, panSpeed * Time.deltaTime);

        // Calculate the desired camera position based on the target's position
       // Vector3 desiredCameraPos = target.position - (transform.forward * Vector3.Distance(transform.position, target.position));

        // Move the camera towards the desired position
        //transform.position = Vector3.Lerp(transform.position, desiredCameraPos, panSpeed * Time.deltaTime);

        if(Vector3.Distance(targetPosition, transform.position) < 0.5f)
        {
            SetMode(0);
        }
    }
    public void MakeLookAt(Transform x)
    {
        target = x;
        SetMode(1);
    }

    public Vector3 GetFocalPoint()
    {
        return lookAtLocation;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        Vector3 cameraForward = mainCamera.transform.forward;
         lookAtLocation = cameraPosition + cameraForward * (targetHeight - cameraPosition.y) / cameraForward.y;

        //lookAtLocation = transform.position + transform.forward * (targetHeight - transform.position.y) / transform.forward.y;
       // Debug.Log(lookAtLocation);
        // Set the look-at location (You can use this vector for any purpose you need)
        Debug.DrawLine(transform.position, lookAtLocation, Color.red);

        switch (cameraMode)
        {
            case 0:
                //OnEnable();
                GetKeyboardMovement();
                if (useScreenEdge)
                    CheckMouseAtScreenEdge();
                DragCamera();

                UpdateVelocity();
                UpdateCameraPosition();
                UpdateBasePosition();
                currentPosition = this.transform.position;
                break;
            case 1:
                //OnDisable();
                MoveToSeeTarget();
                //RotateAroundPoint();
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        cameraActions = new PlayerInputs();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);

        lastPosition = this.transform.position;
        movement = cameraActions.Camera.Movement;
        //cameraActions.Camera.RotateCamera.performed += RotateCamera;
        cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
        cameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        //cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
        cameraActions.Disable();
    }

    private void RotateAroundPoint()
    {
        if (orbitTargetPosition != null && orbitTargetObject != null)
        {
            orbitTargetPosition = orbitTargetObject.transform.position;
            currentRotationAngle += orbitRotationSpeed * Time.deltaTime;

            Quaternion rotation = Quaternion.Euler(0.0f, currentRotationAngle, 0.0f);
            Vector3 position = rotation * new Vector3(0.0f, orbitHeight, -orbitDistance) + orbitTargetPosition;

            transform.position = position;
            transform.LookAt(orbitTargetPosition);
        }
        else
        {
            SetMode(0);
        }


    }

    public void SetUseScreenEdge(bool x)
    {
        useScreenEdge = x;
    }
    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        //Debug.Log("x: "+ movement.ReadValue<Vector2>().x);
        //Debug.Log("y: " + movement.ReadValue<Vector2>().y);
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
            + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f) targetPosition += inputValue;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }


    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }


    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed)
            return;

        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * stepSize;
            if (zoomHeight < minHeight) zoomHeight = minHeight;
            else if (zoomHeight > maxHeight) zoomHeight = maxHeight;
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width) moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width) moveDirection += GetCameraRight();

        if (mousePosition.y < edgeTolerance * Screen.height) moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height) moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }

    private void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed) return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.rightButton.wasPressedThisFrame) startDrag = ray.GetPoint(distance);
            else
                targetPosition += startDrag - ray.GetPoint(distance);
        }
    }
}
