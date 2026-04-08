using UnityEngine;

public class DraggableIngredient : MonoBehaviour
{
    public IngredientData data;

    private Camera cam;
    private bool isDragging;

    private float distanceFromCamera = 3f;
    public float scrollSpeed = 2f;
    public float minDistance = 1f;
    public float maxDistance = 6f;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void Update()
    {
        if (!isDragging) return;

        // Scroll to move closer/further
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distanceFromCamera -= scroll * scrollSpeed;
        distanceFromCamera = Mathf.Clamp(distanceFromCamera, minDistance, maxDistance);

        // Follow mouse in world
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 target = ray.origin + ray.direction * distanceFromCamera;

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 15f);
    }
}