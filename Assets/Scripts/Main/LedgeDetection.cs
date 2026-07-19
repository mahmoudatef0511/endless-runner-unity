using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Player player;
    [SerializeField] private LayerMask whatIsGround;

    BoxCollider2D boxCd => GetComponent<BoxCollider2D>();

    private bool canDetect;

    void Start()
    {
        canDetect = true;    
    }

    // Update is called once per frame
    void Update()
    {
        if(canDetect)
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetect = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /* 
             Solves the issue with having two overlaping platforms and 
             prevents the ledge from being detected 
        */
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCd.bounds.center, boxCd.size, 0);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<PlatformController>() != null)
                return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetect = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
