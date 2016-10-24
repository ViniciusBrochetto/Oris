using UnityEngine;
using System.Collections.Generic;

public class CameraFixedPath : MonoBehaviour
{

    public Transform startPos, endPos;
    public List<Transform> curve;
    public bool climbOnly = true;

    public Transform camDirection;

    public Vector3 GetPosition(Vector3 playerPos)
    {
        Vector3 start = Vector3.Min(endPos.position, startPos.position);
        Vector3 end = Vector3.Max(endPos.position, startPos.position);

        Vector3 SE = endPos.position - startPos.position;
        Vector3 SP = playerPos - startPos.position;

        //Get closest point on line
        Vector3 res = startPos.position + Vector3.Dot(SP, SE) / Vector3.Dot(SE, SE) * SE;

        float distSE = Vector3.Distance(start, end);
        float distSP = Vector3.Distance(start, res);

        float resF = distSP / distSE;

        //TO-DO Bezier -- Get position based on resF ratio of distance between Start and End position
        Debug.DrawLine(res, startPos.position, Color.yellow);

        return res;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FreeLookCam cam = GameObject.FindObjectOfType<FreeLookCam>();
            cam.SetFixedCam(this);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            FreeLookCam cam = GameObject.FindObjectOfType<FreeLookCam>();
            cam.SetFixedCam(null);
        }
    }
}
