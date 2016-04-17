using UnityEngine;
using System.Collections;

public class Butterfly : MonoBehaviour
{
    public new Transform transform;

    void Awake()
    {
        transform = GetComponent<Transform>();
    }
}
