using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : BYSingletonMono<Player>
{
    [SerializeField] float initialVelocity = 5;
    float time = 0;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float distanceLose;
    SpriteRenderer spriteRenderer;
    int spriteIndex = 0; 
    bool canJump = true;
    [SerializeField] float closeDistanceThreshold;
    List<Pipes> pipesList = new List<Pipes>();
    public UnityEvent<Pipes> pipesAdd;
    public UnityEvent<Pipes> pipesRemove;
    Pipes nearestPipe;
    float width;
    float height;
    Transform trans;
    GameObject ground;
    private void Awake()
    {
        trans = transform;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Time.timeScale = 0;
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        ground = GameObject.Find("Ground");
    }
    void OnEnable()
    {
        Vector3 position = trans.position;
        position.y = 0f;
        trans.position = position;
        pipesAdd.AddListener(RegisterPipe);
        pipesRemove.AddListener(UnregisterPipe);
    }
    void RegisterPipe(Pipes pipe)
    {
        pipesList.Add(pipe);
    }
    void UnregisterPipe(Pipes pipe)
    {
        pipesList.Remove(pipe);
    }
    void Update()
    {
        if (Time.timeScale == 0)
            return;
        if (canJump)
        {
            time = 0;
            canJump = false;
            Invoke(nameof(ResetJump), 0.5f);
        }

        time += Time.deltaTime;
        float displacement = initialVelocity * time + 0.5f * gravity * time * time;
        trans.position = Vector3.up * displacement + new Vector3(trans.position.x, 0, trans.position.z);
        FindPipes();
    }
    void ResetJump() => canJump = true;
    void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= SpriteLibControl.instance.sprites.Count)
        {
            spriteIndex = 0;
        }

        if (spriteIndex < SpriteLibControl.instance.sprites.Count && spriteIndex >= 0)
        {
            int index = spriteIndex + 1;
            spriteRenderer.sprite = SpriteLibControl.instance.GetSpriteByName("Bird_0"+index);
            Vector2 spriteSize = spriteRenderer.bounds.size;
            width = spriteSize.x;
            height = spriteSize.y;
        }
    }
    void FindPipes()
    {
        if (pipesList.Count == 0)
            return;

        float minDistance = Mathf.Infinity;
        foreach (Pipes pipe in pipesList)
        {
            if (pipe != null)
            {
                float distance = Vector3.Distance(trans.position, pipe.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPipe = pipe;
                }
            }
        }
        initialVelocity = CheckHeightPipes(nearestPipe,-0.1f);
    }
    float CheckHeightPipes(Pipes nearestPipes, float offSet)
    {
        return nearestPipes.distanceBottomLeftWin.position.y - ground.transform.position.y +
            ground.transform.localScale.y / 2 + offSet;
    }

}
