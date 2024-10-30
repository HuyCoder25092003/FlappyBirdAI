using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    public Transform top;
    public Transform bottom;
    public Transform distanceTopLeftWin, distanceTopRightWin, distanceBottomRightWin, distanceBottomLeftWin;
    public float speed = 5f;
    public float gap = 3f;
    float leftEdge;
    void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;
        top.position += Vector3.up * gap / 2;
        bottom.position += Vector3.down * gap / 2;
    }
    void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.left;
        if (transform.position.x < leftEdge)
        {
            BYPoolManager.instance.GetPool("Pipes").Despawn(transform);
            Player.instance.pipesRemove?.Invoke(this);
        }
    }
}
