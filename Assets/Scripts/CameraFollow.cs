using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(10, 17, 17);

    void LateUpdate()
    {
        if (player != null)
        {
            // Pozi?ia dorit? a camerei
            Vector3 desiredPosition = player.position + offset;

            // Lerp pentru o tranzi?ie lin?
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Asigur?-te c? prive?te spre juc?tor (op?ional)
            transform.LookAt(player.position);
        }
    }
}
