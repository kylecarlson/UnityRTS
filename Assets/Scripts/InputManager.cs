using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public bool invertZoom;
    public float panSpeed;
    public float zoomSpeed;
    public float rotateSpeed;
    public float rotateAmount;
    public GameObject selectedObject;
    private float minHeight = 10f;
    private float maxHeight = 100f;
    private ObjectInfo selectedInfo;
    private Quaternion rotation;

    private float panDetect; // Set on Start()
    public float panModif = 0.05f; // 5% of the screen on each side is used for movement

    // Start is called before the first frame update
    void Start() {
        rotation = Camera.main.transform.rotation;

        panDetect = (Screen.width + Screen.height) / 2 * panModif;
    }

    // Update is called once per frame
    void Update() {
        MoveCamera();
        RotateCamera();

        // Reset Camera
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            Camera.main.transform.rotation = rotation;
        }

        // Left Mouse Click
        if ( Input.GetMouseButtonDown( 0 ) ) {
            LeftClick();
        }
    }

    public void LeftClick() {
        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if ( Physics.Raycast( ray, out hit, 100 ) ) {
            if ( hit.collider.tag == "Ground" && selectedObject != null ) {
                selectedInfo = selectedObject.GetComponent<ObjectInfo>();
                selectedInfo.isSelected = false;
                selectedObject = null;
                Debug.Log( "Deselected" );
            } else if ( hit.collider.tag == "Selectable" ) {
                selectedObject = hit.collider.gameObject;
                selectedInfo = selectedObject.GetComponent<ObjectInfo>();
                selectedInfo.isSelected = true;
                Debug.Log( "Selected" + selectedInfo.objectName );
            }
        }
    }

    void MoveCamera() {
        float moveX, moveY, moveZ;
        moveX = moveY = moveZ = 0;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if ( Input.GetKey( KeyCode.A ) || xPos > 0 && xPos < panDetect ) {
            moveX -= panSpeed;
        } else if ( Input.GetKey( KeyCode.D ) || xPos < Screen.width && xPos > Screen.width - panDetect ) {
            moveX += panSpeed;
        }

        if (Input.GetKey(KeyCode.W) || yPos < Screen.height && yPos > Screen.height - panDetect)
        {
            moveZ += panSpeed;
        }
        else if (Input.GetKey(KeyCode.S) || yPos > 0 && yPos < panDetect)
        {
            moveZ -= panSpeed;
        }

        Vector3 direction = new Vector3( moveX, moveY, moveZ );

        Camera.main.transform.Translate(direction, Space.Self);

        moveY = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, moveY, Camera.main.transform.position.z);
    }

    void RotateCamera() {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        if ( Input.GetMouseButton( 2 ) ) {
            destination.x -= Input.GetAxis( "Mouse Y" ) * rotateAmount;
            destination.y += Input.GetAxis( "Mouse X" ) * rotateAmount;
        }

        if ( destination != origin ) {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards( origin, destination, Time.deltaTime * rotateSpeed );
        }
    }
}
