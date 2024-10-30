using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnRate = 1f;
    public float minHeight = -1f;
    public float maxHeight = 2f;
    public float verticalGap = 3f;
    [SerializeField] Pipes prefab;
    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        Transform pipes = BYPoolManager.instance.GetPool("Pipes").Spawn();
        pipes.SetPositionAndRotation(transform.position, Quaternion.identity);
        pipes.position += Vector3.up * Random.Range(minHeight, maxHeight);
        Pipes p = pipes.GetComponent<Pipes>();
        p.gap = verticalGap;
        Player.instance.pipesAdd?.Invoke(p);
    }

}
