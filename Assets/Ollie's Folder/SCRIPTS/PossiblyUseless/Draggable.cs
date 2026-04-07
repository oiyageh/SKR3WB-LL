using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;
    private float distanceToCamera;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        // Get distance from camera so it doesn't snap weirdly
        distanceToCamera = Vector3.Distance(transform.position, cam.transform.position);
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (isDragging)
        {
            DragObject();
        }
    }

    void DragObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 point = ray.GetPoint(distanceToCamera);

        transform.position = point;
    }
}