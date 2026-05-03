using UnityEngine;

public class KGB_SquareProceed : MonoBehaviour
{
    Rigidbody2D rigid;

    void Start()
    {
        Debug.Log("유니티!"); 
        
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigid.AddForce(Vector2.right * 10f); 
    }
}