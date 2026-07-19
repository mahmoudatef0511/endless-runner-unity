using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private SpriteRenderer headerSr;

    private void Start()
    {
        headerSr.transform.parent = transform.parent;
        headerSr.transform.localScale = new Vector2(sr.bounds.size.x, 0.2f);
        headerSr.transform.position = new Vector2(transform.position.x, sr.bounds.max.y);
        sr.color = GameManager.instance.platformColor;
        headerSr.color = GameManager.instance.platformColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionPlayerComponent = collision.GetComponent<Player>();
        BoxCollider2D platformColliderComponent = GetComponent<BoxCollider2D>();
        if (collisionPlayerComponent != null && 
            collisionPlayerComponent.transform.position.y > 
            platformColliderComponent.bounds.min.y)
            headerSr.color = GameManager.instance.platformHeaderColor;
    }
}
