using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanAndZoom : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomMin = 50;
    public float zoomMax = 80;
    public float speed = 10;
    float startHeight;
    private void Start()
    {
        startHeight = Camera.main.transform.position.y;
    }

    void Update()
    {
        if (Input.touchCount == 3)
           Pan();

        if (Input.touchCount == 2)
            Zoom();

        if (right)
            Camera.main.transform.Translate(1 * speed * Time.deltaTime, 0, 0, Space.World);
        else if (left)
            Camera.main.transform.Translate(-1 * speed * Time.deltaTime, 0, 0, Space.World);
        else if (up)
            Camera.main.transform.Translate(0, 0, 1 * speed * Time.deltaTime, Space.World);
        else if (down)
            Camera.main.transform.Translate(0, 0, -1 * speed * Time.deltaTime, Space.World);

        if (plus)
            Camera.main.transform.Rotate(1 * speed * Time.deltaTime, 0, 0);
        else if (minus)
            Camera.main.transform.Rotate(-1 * speed * Time.deltaTime, 0, 0);

        Camera.main.transform.eulerAngles = new Vector3(Mathf.Clamp(Camera.main.transform.eulerAngles.x, 45, 88)
            , Camera.main.transform.eulerAngles.y, Camera.main.transform.eulerAngles.z);

        Zoom(Input.GetAxis("Mouse ScrollWheel"));

    }

    bool right, left, up, down,plus,minus;

    public void LeftPan() {
        left = !left;
    }

    public void RightPan() {
        right = !right;
    }

    public void UpPan() {
        up = !up;
    }

    public void DownPan() {
        down = !down;
    }

    public void RotatePlus() {
        plus = !plus;
    }

    public void RotateMinus(){
        minus = !minus;
    }

    void Zoom(float difference)
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - 10 * difference, zoomMin, zoomMax);
    }

    void Zoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;
        float difference = currentMagnitude - prevMagnitude;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - difference * 0.1f, zoomMin, zoomMax);
    }

    void Pan() {
        if (Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(1).deltaPosition;
            Camera.main.transform.Translate(touchDeltaPosition.x * speed * Time.deltaTime,
                touchDeltaPosition.y * speed * Time.deltaTime, 0);

            Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, -100, 100),
                Mathf.Clamp(Camera.main.transform.position.y, startHeight, startHeight),
                Mathf.Clamp(Camera.main.transform.position.z, -100, 100));
        }
    }
}
