using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

public class SnappingObject : MonoBehaviour
{
    /* 
    CREDITS: 
    Drag and Drop Function - AIA's "Unity Drag and Drop Script | (Unity 3D tutorial)"; https://www.youtube.com/watch?v=uNCCS6DjebA

    Object Snapping to Layer - Google Gemini AI
    using UnityEngine;
    public class FilteredRaycast : MonoBehaviour
    {
        // Select ONLY the layers you want to hit in the Unity Inspector dropdown
        [SerializeField] private LayerMask targetLayers; 
        public float maxDistance = 100f;

        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // The ray will pass cleanly through any layer not checked in targetLayers
            if (Physics.Raycast(ray, out hit, maxDistance, targetLayers))
            {
                Debug.Log($"Directly hit target behind obstacles: {hit.collider.name}");
            }
        }
    }


    */

    private GameObject selectedObject;

    [SerializeField]
    private LayerMask targetLayers;


    public string draggableObjectTag = "DragObject";

    public string dropAreaTagObject = "DragAndDropArea";

    public float rotationAmount = 90;


    void Update()
    {
        // The drag and drop function
        if(Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                // If the selectedObject; Assign and Pick up
                // For picking up a new object

                RaycastHit hit = MouseProjection();


                if(hit.collider != null)
                {
                    // if we hit a collider in the selectedObject, 
                    if(!hit.collider.CompareTag(draggableObjectTag))
                    {
                        // If the selectedObject does not have a specific string tag == draggableObjectTag
                        // then return nothing
                        return;
                    }

                    selectedObject = hit.collider.gameObject;
                    Debug.Log($"{selectedObject.name} is the selected object");

                    selectedObject.transform.rotation = Quaternion.identity; // resets the selectedObject's rotation;

                    if(selectedObject.GetComponent<Rigidbody>()) ToggleObjectGravityAndPhysics(true);

                    //selectedObject.GetComponent<Rigidbody>().isKinematic = true;

                    //ResetSelectedObjects(true, true);

                    // Makes the cursor invisible while selecting the object
                    Cursor.visible = false;
                }
            }
            else
            {
                // Sets the selectedObject Down
                RaycastHit hit = MouseProjection();

                Debug.Log(hit.collider.name);

                ObjectDragger(0);

                Ray layerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit layerHit;

                if(Physics.Raycast(layerRay, out layerHit, 100f, targetLayers))
                {
                    Vector3 selectedObjectOnArea;

                    bool floorPanel = layerHit.collider.gameObject.GetComponent<DropAreaPanelLocation>().floorDropArea;
                    bool wallPanel = layerHit.collider.gameObject.GetComponent<DropAreaPanelLocation>().wallDropArea;

                    float objectOffsetAmount = layerHit.collider.gameObject.GetComponent<DropAreaPanelLocation>().objectOffset;
                    // If the DropArea is a FloorArea Add to y coord
                    if(floorPanel)
                    {
                        selectedObjectOnArea = new Vector3(
                        layerHit.transform.position.x,
                        layerHit.transform.position.y + (selectedObject.transform.localScale.y / 2) + objectOffsetAmount,
                        layerHit.transform.position.z);
                    }

                    // If the DropArea is a WallArea Add to x coord
                    else if(wallPanel)
                    {
                        selectedObjectOnArea = new Vector3(
                        layerHit.transform.position.x + (selectedObject.transform.localScale.x / 2) + objectOffsetAmount,
                        layerHit.transform.position.y,
                        layerHit.transform.position.z);
                    }
                    else
                    {
                        selectedObjectOnArea = new Vector3(
                        layerHit.transform.position.x,
                        layerHit.transform.position.y,
                        layerHit.transform.position.z);
                    }

                    //selectedObject.transform.position = layerHit.transform.position;
                    selectedObject.transform.position = selectedObjectOnArea;
                }

                if(selectedObject.GetComponent<Rigidbody>()) ToggleObjectGravityAndPhysics(false);

                selectedObject = null;

                Cursor.visible = true;
            }
        }

        if(selectedObject != null)
        {
            // Where our selectedObject will be dragged around the environment
            ObjectDragger(.25f);

            // Rotates the selectedObject using the right click
            if(Input.GetMouseButtonDown(1))
            {
                selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                    selectedObject.transform.rotation.eulerAngles.x,
                    selectedObject.transform.rotation.eulerAngles.y + rotationAmount,
                    selectedObject.transform.rotation.eulerAngles.z
                ));
            }
        }
    }

    RaycastHit MouseProjection()
    {
        Vector3 mouseScreenPositionFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane
        );
        Vector3 mouseScreenPositionNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
        );
        Vector3 worldMousePositionFar = Camera.main.ScreenToWorldPoint(mouseScreenPositionFar);
        Vector3 worldMousePositionNear = Camera.main.ScreenToWorldPoint(mouseScreenPositionNear);

        RaycastHit objectHit;
        Physics.Raycast(
            worldMousePositionNear, 
            worldMousePositionFar - worldMousePositionNear, 
            out objectHit);
        
        // Debug.Log(objectHit.collider.name);
        // Debug.Log(objectHit.distance);
        // Debug.Log(objectHit.point);

        return objectHit;
    }

    void ObjectDragger(float objectHoverAmount)
    {
        Vector3 object3DPosition = new Vector3(
                Input.mousePosition.x, 
                Input.mousePosition.y, 
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(object3DPosition);

            selectedObject.transform.position = new Vector3(
                worldPosition.x, 
                //objectHoverAmount, 
                worldPosition.y,
                worldPosition.z);
    }

    void ResetSelectedObjects(bool disableObjectGravity, bool resetObjectOrientation)
    {
        if(selectedObject == null) return;

        selectedObject.GetComponent<Rigidbody>().isKinematic = disableObjectGravity;

        // resets the selectedObject's rotation
        selectedObject.transform.rotation = Quaternion.identity;
    }

    void ToggleObjectGravityAndPhysics(bool disable)
    {
        if(selectedObject == null) return;
        Rigidbody selectedObjectRB = selectedObject.GetComponent<Rigidbody>();
        
        if(disable)
        {
            selectedObjectRB.isKinematic = true;
            selectedObjectRB.detectCollisions = false;
        }

        if(!disable)
        {
            selectedObjectRB.isKinematic = false;
            selectedObjectRB.detectCollisions = true;
        }
    }
}
