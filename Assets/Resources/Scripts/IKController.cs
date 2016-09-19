﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{
    protected Animator animator;

    public bool ikActive = false;

    bool rightHandIK, leftHandIK = false;
    public bool useHandsIK, useFeetIK;

    public Transform rightShoulder, leftShoulder;

    public Transform rightHandPos, leftHandPos = null;
    public Transform rightFootPos, leftFootPos = null;

    public Transform lookObj = null;


    public float dist;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (useHandsIK)
        {
            if (!rightShoulder || !leftShoulder)
                return;

            Physics.SphereCast(rightShoulder.position, 0.3f, transform.right, out hit, 0.25f);

            if (hit.collider)
            {
                rightHandIK = true;
                rightHandPos.up = (hit.normal).normalized;
                rightHandPos.position = hit.point - Vector3.up * 0.1f + rightHandPos.up * 0.05f + transform.forward * 0.2f;
            }
            else
            {
                rightHandIK = false;
            }

            Physics.SphereCast(leftShoulder.position, 0.3f, -transform.right, out hit, 0.25f);

            if (hit.collider)
            {
                leftHandIK = true;
                leftHandPos.up = (hit.normal).normalized;
                leftHandPos.position = hit.point - Vector3.up * 0.1f + leftHandPos.up * 0.05f + transform.forward * 0.2f;
            }
            else
            {
                leftHandIK = false;
            }
        }

        if (useFeetIK)
        {
            Vector3 v = transform.position + Vector3.up * 0.5f;

            if (Physics.Raycast(v, Vector3.down, out hit, 3f))
            {
                rightFootPos.position = hit.point;
                dist = hit.distance;
                rightFootPos.rotation = Quaternion.FromToRotation(transform.up, hit.normal);
            }
        }

    }

    public void SetHandsIK(bool active)
    {
        leftHandIK = active;
        rightHandIK = active;
    }

    public void SetHandsIK(bool active, Transform lHand, Transform rHand)
    {
        leftHandIK = active;
        rightHandIK = active;
        leftHandPos = lHand;
        rightHandPos = rHand;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }
                if (rightHandPos != null)
                {
                    if (rightHandIK)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos.position);

                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandPos.rotation);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
                    }

                    if (leftHandIK)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos.position);

                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.rotation);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
                    }
                }
                if (rightFootPos != null)
                {
                    animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos.position);

                    dist = Mathf.Max(0f, dist);

                    if (dist - 0.5f <= 0.5f)
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f - (dist / 0.5f));
                        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f - (dist / 0.5f));
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
                    }

                    animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootPos.rotation);
                }
            }
        }
    }
}
