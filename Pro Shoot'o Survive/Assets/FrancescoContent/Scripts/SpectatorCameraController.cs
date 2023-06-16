using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float minRotationX_Angle;
    [SerializeField] private float maxRotationX_Angle;
    private Vector3 moveVelocity;
    private Vector3 eulerAngles;
    private bool noclipMovement;
    private bool showMouseCursor;

    // Start is called before the first frame update
    void Start()
    {
        noclipMovement = true;
        showMouseCursor = true;
    }

    public void MoveCamera()
    {
        if (noclipMovement)
        {
            NoclipMovement();
        }

        else
        {
            UFO_Movement();
        }

        transform.position += moveVelocity * Time.deltaTime;
    }

    private void NoclipMovement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("HorizontalX"), Input.GetAxis("Vertical"), Input.GetAxis("HorizontalZ"));

        moveDir.x = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) ? 0 : moveDir.x;
        moveDir.y = Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E) ? 0 : moveDir.y;
        moveDir.z = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) ? 0 : moveDir.z;

        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        moveVelocity = transform.right * moveDir.x * moveSpeed;
        moveVelocity += transform.up * moveDir.y * moveSpeed;
        moveVelocity += transform.forward * moveDir.z * moveSpeed;
        moveVelocity *= 0.25f;
    }

    private void UFO_Movement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("HorizontalX"), Input.GetAxis("Vertical"), Input.GetAxis("HorizontalZ"));

        moveDir.x = Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) ? 0 : moveDir.x;
        moveDir.y = Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E) ? 0 : moveDir.y;
        moveDir.z = Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) ? 0 : moveDir.z;

        if (moveDir.magnitude > 1)
        {
            moveDir.Normalize();
        }

        float ufoForwardRadAngles = Mathf.Atan2(transform.right.z, transform.right.x);
        ufoForwardRadAngles += Mathf.PI * 0.5f;
        Vector3 ufoForward = new Vector3(Mathf.Cos(ufoForwardRadAngles), 0, Mathf.Sin(ufoForwardRadAngles));

        moveVelocity = transform.right * moveDir.x * moveSpeed;
        moveVelocity.y += moveDir.y * moveSpeed;
        moveVelocity += ufoForward * moveDir.z * moveSpeed;
        moveVelocity *= 0.25f;

        UI_Mngr.Instance.TextSprites["TextInfo"].text = ufoForward.ToString();
    }

    public void ToggleCameraMovement()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            noclipMovement = !noclipMovement;
        }
    }

    public void RotateCamera()
    {
        Vector3 rotationDir = new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        if (rotationDir.magnitude > 1)
        {
            rotationDir.Normalize();
        }

        eulerAngles += rotationSpeed * rotationDir * Time.deltaTime;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, minRotationX_Angle, maxRotationX_Angle);

        transform.eulerAngles = eulerAngles;
    }

    public void ToggleMouseCursorVisibilityEvent()
    {
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.F1))
        {
            ToggleMouseCursorVisibility();
        }
    }

    void ToggleMouseCursorVisibility()
    {
        showMouseCursor = !showMouseCursor;

        Cursor.visible = showMouseCursor;
        Cursor.lockState = showMouseCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif

        ToggleMouseCursorVisibilityEvent();
        ToggleCameraMovement();

        MoveCamera();
        RotateCamera();

        float fps = 1 / Time.deltaTime;

        //UI_Mngr.Instance.TextSprites["TextInfo"].text = fps.ToString();
        //UI_Mngr.Instance.TextSprites["TextInfo"].text = transform.right.ToString();
    }
}
