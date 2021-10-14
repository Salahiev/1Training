using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // один экземпляр на объекте
public class moveObject : MonoBehaviour
{
    [SerializeField] Vector3 movePosition;
    [SerializeField] float moveSpeed;
    [SerializeField] [Range(0,1)] float moveProgress; // 0-не двигался, 1-полностью переместился
    Vector3 starPosition;
   
    // Start is called before the first frame update
    void Start()
    {
        starPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveProgress = Mathf.PingPong(Time.time*moveSpeed, 1);       // класс математический 
        Vector3 offset = movePosition * moveProgress;
        transform.position = starPosition + offset;

    }
}
