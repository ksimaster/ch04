using UnityEngine;
using UnityEngine.UI;

public class CameraMover : MonoBehaviour
{
    public float MouseSensitivity;
    public float ScrollSensitivity;
    public float MinViewingAngle;
    public float MaxViewingAngle;
    public float RotationSmoothing;
    public float ScrollSmoothing;
    public float InitialRotation;
    public GameObject Board;

    public float zoomSpeed = 0.3f;
   // public Button inButton;
   // public Button outButton;

    private int boardSize;
    private float minOffset;
    private float maxOffset;
    private float localOffeset;
    private Vector3 localRotation;

    private void Start()
    {
        var tilesGenerator = Board.GetComponent<ITilesGenerator>();
        boardSize = tilesGenerator.BoardSize;
        SetInitialPosition();
        SetInitialRotation();
    }

    private void SetInitialPosition()
    {
        var initialOffset = boardSize * Mathf.Sqrt(2);
        localOffeset = initialOffset;
        minOffset = initialOffset / 10;
        maxOffset = initialOffset;
        transform.position = initialOffset * Vector3.back;
    }

    private void SetInitialRotation()
    {
        localRotation.y = InitialRotation; //Mouse y axis is x axis in world space.
        var initialRotation = Quaternion.Euler(localRotation.y, 0, 0);
        transform.parent.rotation = initialRotation;
    }

    private void LateUpdate()
    {
        GetMouseInput();
        GetScrollInput();
        //GetMultiInput();
        ChangePosition();
        ChangeRotation();
    }

    private void GetMouseInput()
    {
        if (!Input.GetMouseButton(1))
            return;
        var xAxis = Input.GetAxis("Mouse X");
        var yAxis = Input.GetAxis("Mouse Y");
        localRotation.x += xAxis * MouseSensitivity;
        localRotation.y -= yAxis * MouseSensitivity;
        localRotation.y = Mathf.Clamp(localRotation.y, MinViewingAngle, MaxViewingAngle);
    }

    private void GetScrollInput()
    {
        var scroll = Input.GetAxis("Mouse ScrollWheel");
        localOffeset -= scroll * ScrollSensitivity;
        localOffeset = Mathf.Clamp(localOffeset, minOffset, maxOffset);
    }

    private void GetMultiInput()
    {
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            //transform.localScale += new Vector3(deltaMagnitudeDiff * zoomSpeed, deltaMagnitudeDiff * zoomSpeed, deltaMagnitudeDiff * zoomSpeed);
            localOffeset = deltaMagnitudeDiff * zoomSpeed;
            localOffeset = Mathf.Clamp(localOffeset, minOffset, maxOffset);
        }
    }

    public void ZoomIn()
    {
        localOffeset -= zoomSpeed;
        localOffeset = Mathf.Clamp(localOffeset, minOffset, maxOffset);
    }

    public void ZoomOut()
    {
        localOffeset += zoomSpeed;
        localOffeset = Mathf.Clamp(localOffeset, minOffset, maxOffset);
    }

    private void ChangePosition()
    {
        var lerpOffset = Mathf.Lerp(transform.localPosition.z, -localOffeset, ScrollSmoothing * Time.deltaTime);
        transform.localPosition = lerpOffset * Vector3.forward;
    }

    private void ChangeRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        transform.parent.rotation =
            Quaternion.Lerp(transform.parent.rotation, targetRotation, RotationSmoothing * Time.deltaTime);
    }
}