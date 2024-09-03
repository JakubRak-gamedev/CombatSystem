using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform DesiredTransform;

    private void Update()
    {
        transform.position = DesiredTransform.position;
    }
}
