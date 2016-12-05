using UnityEngine;

public class ClimbController : MonoBehaviour
{
    public bool debug;

    [SerializeField]
    [Range(0.01f, 2f)]
    private float maxDistance = 0.5f;

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

        Vector3 avgPos = Vector3.zero;
        for (int i = 0; i < limitPositions.Length; i++)
        {
            ray = new Ray(limitPositions[i].position + move, limitPositions[i].forward);

            if (!Physics.Raycast(ray, out hit, maxDistance, grabMask.value))
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
                ci.avgNormal += hit.normal;

                if (debug)
                    Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
            }
            avgPos += limitPositions[i].position + move;
        }

        ci.avgNormal /= 4f;
        avgPos /= 4f;

        ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit, maxDistance * 2f, grabMask.value))
        {
            //ci.avgNormal = hit.normal;
            ci.grabPosition = hit.point;

            if (hit.transform.GetComponent<shitscript>())
                ci.parentTransform = hit.transform.GetComponent<shitscript>().m_ParentBone;
            else
                ci.parentTransform = hit.transform;

            if (hit.transform.tag.Contains("Boss"))
                ci.isBoss = true;

            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
        }



        return ci;
    }
}

public struct ClimbInfo
{
    public bool handsConnected;
    public bool feetConnected;
    public Vector3 grabPosition;
    public Vector3 avgNormal;
    public Transform parentTransform;
    public bool isBoss;
}