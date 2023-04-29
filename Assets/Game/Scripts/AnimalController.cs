using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public void Move(Vector2 pos)
    {
        rb.MovePosition(pos);
    }
    
}
