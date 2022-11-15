using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] EyeBall ball;
    [SerializeField] Camera cam;
    [SerializeField] GameObject arrow;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] LayerMask rayLayer;
    [SerializeField] IkutBall cameraPivot;
    [SerializeField] Vector2 camSensitivity;
    [SerializeField] float shootForce;
    [SerializeField] Image aim;
    [SerializeField] LineRenderer line;
    [SerializeField] TMP_Text shootCountText;


    Vector3 lastMousePosition;
    Vector3 forceDir;
    Renderer [] arrowRends;
    Color [] arrowOriginalColors;
    float ballDistance;
    float forceFactor;
    bool isShooting;
    int shootCount = 0;

    public int ShootCount { get => shootCount;}

    private void Start()
    {
        ballDistance = Vector3.Distance(cam.transform.position, ball.Position) +1;
        arrowRends = arrow.GetComponentsInChildren<Renderer>();
        arrowOriginalColors = new Color[arrowRends.Length];
        for(int i=0; i< arrowRends.Length; i++)
        {
            arrowOriginalColors[i] = arrowRends[i].material.color;
        }
        arrow.SetActive (false);
        shootCountText.text = "Shoot Count : " +shootCount;

        line.enabled = false;
    }

    void Update()
    {
        
        if (ball.IsMoving || ball.IsTeleporting)
            return;

            aim.gameObject.SetActive(true);
            var rectx = aim.GetComponent<RectTransform>();
            rectx.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
        

        if(this.transform.position != ball.Position)
        {
            this.transform.position = ball.Position;

            aim.gameObject.SetActive(true);
            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = cam.WorldToScreenPoint(ball.Position);
        }    
        

        if (Input.GetMouseButtonDown(0))
        {
    
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(ray, ballDistance, ballLayer))
            {
                isShooting = true;
                Debug.Log("Click Ball");
                arrow.SetActive(true);
                line.enabled = true;
            }
            
        }
        
        if(Input.GetMouseButton(0) && isShooting == true)
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, ballDistance*2, rayLayer))
            {
                Debug.DrawLine(ball.Position, hit.point);

                var forceVector = ball.Position - hit.point;
                forceVector = new Vector3(forceVector.x, 0, forceVector.z);
                forceDir = forceVector.normalized;
                var forceMagnitude = forceVector.magnitude;
                Debug.Log(forceMagnitude);
                forceMagnitude = Mathf.Clamp(forceMagnitude, 0, 5);
                forceFactor = forceMagnitude/5;
            }

            this.transform.LookAt(this.transform.position + forceDir);
            arrow.transform.localScale = new Vector3(1+ 0.5f * forceFactor, 1+ 0.5f * forceFactor, 1+ 2 * forceFactor);

            for (int i=0; i < arrowRends.Length; i++)
            {
                arrowRends[i].material.color = Color.Lerp(arrowOriginalColors[i], Color.red, forceFactor);
            }

            var rect = aim.GetComponent<RectTransform>();
            rect.anchoredPosition = Input.mousePosition;

            var ballScreenPos = cam.WorldToScreenPoint(ball.Position);
            line.SetPositions(new Vector3[] {ballScreenPos, Input.mousePosition});
        }

        if(Input.GetMouseButton(0) && isShooting == false)
        {
            var current = cam.ScreenToViewportPoint(Input.mousePosition);
            var last = cam.ScreenToViewportPoint(lastMousePosition);
            var delta = current-last;

            cameraPivot.transform.RotateAround(ball.Position, Vector3.up, delta.x * camSensitivity.x);
            cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, delta.y * camSensitivity.y);

            var angle = Vector3.SignedAngle(Vector3.up, cam.transform.up, cam.transform.right);

            if(angle < 3)
                cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, 3 - angle);

            else if (angle > 65)
                cameraPivot.transform.RotateAround(ball.Position, cam.transform.right, 65 - angle);

                
        }

        if (Input.GetMouseButtonUp(0) && isShooting)
                {
                    ball.AddForce(forceDir*shootForce*forceFactor);
                    shootCount += 1;
                    shootCountText.text = "Shoot Count : " +shootCount;
                    forceFactor = 0;
                    forceDir = Vector3.zero;
                    isShooting = false;
                    arrow.SetActive(false);
                    aim.gameObject.SetActive(false);
                    line.enabled = false;
                }

        lastMousePosition = Input.mousePosition;
    }
}
