using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform frontView;
    public Transform backView;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    [Header("Mouse Follow Settings")]
    public float mouseSensitivity = 2f;
    public float maxOffset = 10f;

    private Transform targetView;

    float mouseX;
    float mouseY;

    void Start()
    {
        targetView = backView;

        if (targetView != null)
        {
            transform.position = targetView.position;
            transform.rotation = targetView.rotation;
        }

        // Always visible cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandleInput();
        HandleMouse();
        MoveCamera();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetBackView();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetFrontView();
        }
    }

    void HandleMouse()
    {
        // Still reads mouse movement even when visible
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        mouseX = Mathf.Clamp(mouseX, -maxOffset, maxOffset);
        mouseY = Mathf.Clamp(mouseY, -maxOffset, maxOffset);
    }

    void MoveCamera()
    {
        if (targetView == null) return;

        transform.position = Vector3.Lerp(
            transform.position,
            targetView.position,
            Time.deltaTime * moveSpeed
        );

        Quaternion baseRotation = targetView.rotation;
        Quaternion mouseOffset = Quaternion.Euler(mouseY, mouseX, 0);
        Quaternion finalRotation = baseRotation * mouseOffset;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            finalRotation,
            Time.deltaTime * rotateSpeed
        );
    }

    public void SetFrontView()
    {
        targetView = frontView;
        ResetMouse();
    }

    public void SetBackView()
    {
        targetView = backView;
        ResetMouse();
    }

    void ResetMouse()
    {
        mouseX = 0f;
        mouseY = 0f;
    }
}