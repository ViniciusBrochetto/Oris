using UnityEngine;

public class ClimbController : MonoBehaviour
{
    public bool debug;

    [SerializeField]
    [Range(0.01f, 2f)]
    private float maxDistance = 0.5f;

    [SerializeField]
    [Range(0.01f, 2f)]
    private float testRadius = 0.2f;

    [SerializeField]
    private LayerMask grabMask;

    [SerializeField]
    private Transform[] limitTop;
    [SerializeField]
    private Transform[] limitBotton;

    private Transform[] limitPositions;

    void Start()
    {
        limitPositions = new Transform[4];

        limitPositions[0] = limitBotton[0];
        limitPositions[1] = limitBotton[1];
        limitPositions[2] = limitTop[0];
        limitPositions[3] = limitTop[1];
    }

    void FixedUpdate()
    {
        //if (debug)
        //    Climb();
    }

    public ClimbInfo Climb()
    {
        return Climb(Vector3.zero);
    }

    public ClimbInfo Climb(Vector3 move)
    {
        ClimbInfo ci = new ClimbInfo();
        ci.feetConnected = true;
        ci.handsConnected = true;

        Ray ray;
        RaycastHit hit;
        for (int i = 0; i < limitPositions.Length; i++)
        {
            ray = new Ray(limitPositions[i].position + move, limitPositions[i].forward + move);

            if (!Physics.SphereCast(ray, testRadius, out hit, maxDistance, grabMask.value))
            {
                if (i < 2)
                    ci.feetConnected = false;
                else
                    ci.handsConnected = false;

                if (debug)
                    Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
            }
            else
            {
                if (debug)
                    Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            }

            if (debug)
            {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                g.transform.localScale = Vector3.one * testRadius * 2f;
                g.transform.position = ray.origin + (ray.direction.normalized * hit.distance);
                g.GetComponent<Collider>().enabled = false;
                Destroy(g, Time.fixedDeltaTime);
            }
        }

        return ci;
    }
}

public struct ClimbInfo
{
    public bool handsConnected;
    public bool feetConnected;
    public Vector3 grabPosition;
}