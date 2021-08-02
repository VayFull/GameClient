using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayer : MonoBehaviour
{
    public int Id { get; set; }
    public void Init(int id)
    {
        Id = id;
    }
}
