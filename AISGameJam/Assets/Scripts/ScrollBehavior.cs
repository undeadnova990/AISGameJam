using UnityEngine;
using UnityEngine.UIElements;

public class ScrollBehavior : MonoBehaviour
{
    // This moves the camera left or right by a certain fixed amount so it does not scroll away 
    // from the environment

    public float panSpeed = 20f;
    public float paddingThickness = 20f;

    public float scrollSpeed = 2000f;

    public Vector2 panLimitX = new Vector2(-20, 20);
    public Vector2 panLimitZ = new Vector2(-20, 20);
    public Vector2 zoomLimity = new Vector2(5, 20);

    bool controlCamera = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            controlCamera = !controlCamera;
        }

        if (!controlCamera)
            return;


        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - paddingThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= paddingThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - paddingThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= paddingThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        // scrolling to zoom in/out
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * Time.deltaTime * scrollSpeed;

        pos.x = Mathf.Clamp(pos.x, panLimitX.x, panLimitX.y);
        pos.z = Mathf.Clamp(pos.z, panLimitZ.x, panLimitZ.y);
        pos.y = Mathf.Clamp(pos.y, zoomLimity.x, zoomLimity.y);

        transform.position = pos;
    }

    // private void OnEnable()
    // {
    //     var uiDocument = GetComponent<UIDocument>();
    //     var root = uiDocument.rootVisualElement;

    //     root.RegisterCallback<WheelEvent>(OnMouseWheelScroll);
    // }

    // private void OnDisable()
    // {
    //     var uiDocument = GetComponent<UIDocument>();
    //     if (uiDocument != null)
    //     {
    //         uiDocument.rootVisualElement.UnregisterCallback<WheelEvent>(OnMouseWheelScroll);
    //     }
    // }

    // void OnMouseWheelScroll(WheelEvent scrolled)
    // {
    //     // Gets the float value for how much the player scrolls
    //     float scrollAmount = scrolled.delta.y;

    //     Debug.Log($"Scrolled to {scrollAmount}");

    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    
}
