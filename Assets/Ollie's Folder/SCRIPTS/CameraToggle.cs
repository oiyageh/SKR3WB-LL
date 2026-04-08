using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public float playerTurnSpeed = 10f;

    [Header("Camera Positions")]
    public Transform frontView;
    public Transform backView;

    [Header("Camera Movement")]
    public float camMoveSpeed = 5f;
    public float camRotateSpeed = 5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxOffset = 10f;

    private Transform targetView;
    private Quaternion targetPlayerRotation;

    float mouseX;
    float mouseY;

    void Start()
    {
        // Start facing forward
        targetView = frontView;
        targetPlayerRotation = Quaternion.Euler(0f, 0f, 0f);

        // Snap camera to start
        transform.position = frontView.position;
        transform.rotation = frontView.rotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandleInput();
        HandleMouse();
        RotatePlayer();
        MoveCamera();
    }

    void HandleInput()
    {
        // Face backward
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            SetBackward();
        }

        // Face forward
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            SetForward();
        }
    }

    void HandleMouse()
    {
        mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        mouseX = Mathf.Clamp(mouseX, -maxOffset, maxOffset);
        mouseY = Mathf.Clamp(mouseY, -maxOffset, maxOffset);
    }

    void RotatePlayer()
    {
        if (player == null) return;

        player.rotation = Quaternion.Lerp(
            player.rotation,
            targetPlayerRotation,
            Time.deltaTime * playerTurnSpeed
        );
    }

    void MoveCamera()
    {
        if (targetView == null) return;

        // Position
        transform.position = Vector3.Lerp(
            transform.position,
            targetView.position,
            Time.deltaTime * camMoveSpeed
        );

        // Rotation + mouse offset
        Quaternion baseRot = targetView.rotation;
        Quaternion mouseOffset = Quaternion.Euler(mouseY, mouseX, 0);
        Quaternion finalRot = baseRot * mouseOffset;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            finalRot,
            Time.deltaTime * camRotateSpeed
        );
    }

    void SetForward()
    {
        if (targetView == frontView) return;

        targetView = frontView;
        targetPlayerRotation = Quaternion.Euler(0f, 0f, 0f);
        ResetMouse();
    }

    void SetBackward()
    {
        if (targetView == backView) return;

        targetView = backView;
        targetPlayerRotation = Quaternion.Euler(0f, 180f, 0f);
        ResetMouse();
    }

    void ResetMouse()
    {
        mouseX = 0f;
        mouseY = 0f;
    }
}