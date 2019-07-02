using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    const float MOVEMENT_SPEED = 1f, ROTATION_SPEED = 1f;
    bool isCoroutineRunning;
    public Transform mainMenuPosition, standardPosition, zoomInPosition;
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        MoveCameraToMainMenuPosition();
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator MoveCameraToPosition(Transform destination)
    {
        isCoroutineRunning = true;
        float totalDistance = Vector3.Distance(transform.position, destination.position);
        float initialAngle = Vector3.Angle(transform.forward, destination.forward);

        float actualDistance = Vector3.Distance(transform.position, destination.position);
        float actualAngle = Vector3.Angle(transform.forward, destination.forward);
        while (actualDistance > 0.1f || actualAngle > 1)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, MOVEMENT_SPEED / actualDistance * totalDistance * Time.fixedDeltaTime);
            transform.forward = Vector3.Lerp(transform.forward, destination.forward, ROTATION_SPEED / actualAngle * initialAngle * Time.fixedDeltaTime);
            actualDistance = Vector3.Distance(transform.position, destination.position);
            actualAngle = Vector3.Angle(transform.forward, destination.forward);
            yield return new WaitForFixedUpdate();
            Debug.Log("He salido de la subrutina");
            // yield return new WaitForEndOfFrame();
        }
        transform.forward = destination.forward;
        transform.position = destination.position;
        isCoroutineRunning = false;
    }
    public void moveCamera(Transform destination)
    {
        Debug.Log("Está la corutina cargando: " + isCoroutineRunning);
        if (!isCoroutineRunning)
        {
            StartCoroutine(MoveCameraToPosition(destination));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(MoveCameraToPosition(destination));
        }
    }
    public void MoveCameraToStandardPosition()
    {
        moveCamera(standardPosition);
    }
    public void MoveCameraToMainMenuPosition()
    {
        moveCamera(mainMenuPosition);
    }
    public void MoveCameraToZoomInPosition()
    {
        Debug.Log(zoomInPosition);
        moveCamera(zoomInPosition);
    }
}
