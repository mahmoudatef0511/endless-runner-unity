using UnityEngine;

public class MovingTrap : Trap
{

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Transform[] movePoints;

    private int movePointIndex;

    protected override void Start()
    {
        base.Start();
        transform.position = movePoints[0].position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            movePoints[movePointIndex].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position,
            movePoints[movePointIndex].position) < .25f)
        {
            movePointIndex++;
            if (movePointIndex >= movePoints.Length)
                movePointIndex = 0;
        }

        if (transform.position.x > movePoints[movePointIndex].position.x)
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        else
            transform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.deltaTime));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
