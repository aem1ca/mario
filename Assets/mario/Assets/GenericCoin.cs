using UnityEngine;

public abstract class GenericCoin : MonoBehaviour
{
    protected GameGui gameGui;

    void Start()
    {
        gameGui = GameObject.Find("GameUI").GetComponent<GameGui>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectMyself();
            Destroy(gameObject);
        }
    }

    public void CollectAndDestroy()
    {
        CollectMyself();
        Destroy(gameObject);
    }

    public abstract void CollectMyself();
}
