using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Transform target;

    public static CameraHandler instance;
    private Animator anim;

    [SerializeField] private float min_x, min_y, max_x, max_y;

    [SerializeField] private float speed;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!target) return;
        transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(target.position.x, min_x, max_x),
            Mathf.Clamp(target.position.y, min_y, max_y), -10), speed * Time.fixedDeltaTime);
    }
    
    public void ShakeCamera()
    {
        //anim.Play("Camera_Shake");
    }
    
}
