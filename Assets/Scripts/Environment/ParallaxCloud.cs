using System;
using UnityEngine;

public class ParallaxCloud : MonoBehaviour
{
    private float length, startPos, initPos;
    public float speedParallax;
    private float tempo = 0;
    void Start()
    {
        initPos = transform.position.x;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        tempo += Time.deltaTime;
        float temp = (tempo * (1 - speedParallax));
        float dist = (tempo * speedParallax);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        if ((transform.position.x >= length + initPos) || (transform.position.x <= initPos - length))
        {
            startPos = initPos;
            tempo = 0;
        }
    }
}