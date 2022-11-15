using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkutBall : MonoBehaviour
{
    [SerializeField] EyeBall ball;
    [SerializeField] float speed=1;

    public bool IsMoving => this.transform.position == ball.Position;
    void Update()
    {
        if(this.transform.position == ball.Position)
            return;

        this.transform.position = Vector3.Lerp(this.transform.position, ball.Position, Time.deltaTime * speed);

        if(ball.IsMoving)
            return;
            
        if (Vector3.Distance(transform.position, ball.Position)< 0.1f)
        {
            transform.position = ball.Position;
        }
    }
}
