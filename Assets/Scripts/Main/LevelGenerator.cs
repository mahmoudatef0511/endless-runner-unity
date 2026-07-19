using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private Transform[] levelParts;
    [SerializeField] private Vector3 nextPartPos;
    [SerializeField] private float distanceToSpawn;
    [SerializeField] private float distanceToDelete;
    [SerializeField] private Transform player;

    void Update()
    {
        DeletePlatform();
        GeneratePlatform();
    }

    private void GeneratePlatform()
    {
        while(Vector2.Distance(player.transform.position, nextPartPos) < distanceToSpawn)
        {
            Transform levelPart = levelParts[Random.Range(0, levelParts.Length)];
            //Vector2 newPosition = new Vector2(nextPartPos.x - levelPart.Find("StartPoint").position.x, levelPart.Find("StartPoint").position.y);
            Vector2 newPosition = new Vector2(nextPartPos.x - levelPart.Find("StartPoint").position.x, -4);
            Transform newPart = Instantiate(levelPart, newPosition, transform.rotation, transform);
            nextPartPos = newPart.Find("EndPoint").position;
        }
    }

    private void DeletePlatform()
    {
        if(transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);
            if (Vector2.Distance(player.transform.position, partToDelete.transform.position) > distanceToDelete)
                Destroy(partToDelete.gameObject);
        }
    }
}
