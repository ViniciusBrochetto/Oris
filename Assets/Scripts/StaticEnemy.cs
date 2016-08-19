using UnityEngine;
using System.Collections;

public class StaticEnemy : MonoBehaviour
{

    private Transform m_Player;

    [SerializeField]
    private float m_WakeUpDistance;
    [SerializeField]
    private float m_AttackDistance;
    [SerializeField]
    private float m_AttackDelay;
    [SerializeField]
    private float m_AttackSpeed;

    [SerializeField]
    private bool m_AttackHead;
    [SerializeField]
    private bool m_TrackPosition;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(DetectPlayer());
    }

    IEnumerator DetectPlayer()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, m_Player.position) < m_AttackDistance)
            {
                //Ataca
                GetComponent<Renderer>().material.color = Color.red;
                yield return StartCoroutine(Attack());
            }
            else if (Vector3.Distance(transform.position, m_Player.position) < m_WakeUpDistance)
            {
                GetComponent<Renderer>().material.color = Color.yellow;
                //Entra em alerta
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.gray;
                //Dorme
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Attack()
    {
        Vector3 c_OriginalPosition = transform.position;
        Vector3 c_PlayerPosition = m_Player.position;

        if (m_AttackHead)
        {
            c_PlayerPosition = Vector3.Scale(c_PlayerPosition, new Vector3(1f, 0f, 1f));
            c_PlayerPosition.y = transform.position.y;
        }

        yield return new WaitForSeconds(m_AttackDelay);

        float c_Time = 0;
        while (c_Time < m_AttackSpeed)
        {
            transform.position = Vector3.Slerp(c_OriginalPosition, c_PlayerPosition, c_Time / m_AttackSpeed);
            c_Time += Time.deltaTime;

            if (m_TrackPosition && (c_Time / m_AttackSpeed) < 0.5f)
            {
                if (m_AttackHead)
                {
                    c_PlayerPosition = m_Player.position;
                    c_PlayerPosition = Vector3.Scale(c_PlayerPosition, new Vector3(1f, 0f, 1f));
                    c_PlayerPosition.y = transform.position.y;
                }
                else
                {
                    c_PlayerPosition = m_Player.position;
                }
            }

            yield return new WaitForEndOfFrame();
        }


        c_Time = 0;
        while (c_Time < m_AttackSpeed)
        {
            transform.position = Vector3.Slerp(c_PlayerPosition, c_OriginalPosition, c_Time / m_AttackSpeed);
            c_Time += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = c_OriginalPosition;

        yield return 0;
    }
}
