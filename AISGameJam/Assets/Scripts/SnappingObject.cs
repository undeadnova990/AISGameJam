using TreeEditor;
using UnityEngine;

public class SnappingObject : MonoBehaviour
{
    Vector3 offset;
    public string DropAreaObjectTag = "DroppingArea";
    void OnMouseDrag()
    {
        // If the Mouse hits the Draggable Object and the left mouse click is held down, then move the object.

        

    }

    void OnMouseUp()
    {
        // if the left mouse click is released, snap the object onto an object that is a can allow the object to snap

        //Need to get the mouse position from the screen to hit an object.
        var OnMouseOverObject = Camera.main.transform.position;


    }

    void OnMouseDown()
    {
        // if the left mouse click is held down, have the object follow along the mouse's position

        //Need to get the mouse position from the screen to hit an object.
        var OnMouseOverObject = Camera.main.transform.position;


    }


    Vector3 MousePosition()
    {
        var mousePositonOnScreen = Input.mousePosition;
        mousePositonOnScreen.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePositonOnScreen);
    }
    
}
