using System.Collections;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IDestruible
{
    [SerializeField] private float speed;
    private PlayerCollision subjectPlayerCollision;

    [Space]
    [SerializeField] private Bound[] subjectBoundList;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        subjectPlayerCollision = FindObjectOfType<PlayerCollision>(this);
    }

    private void Start()
    {
        subjectBoundList = FindObjectsOfType<Bound>();

        foreach (Bound bound in subjectBoundList)
        {
            bound.CollisionBoundAction += DestroyNow;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            subjectPlayerCollision.CollisionPowerUpTarget += MakeEffect;
            subjectPlayerCollision.CollisionPowerUpObject += DestroyNow;
        }
    }

    private void OnEnable()
    {
        foreach (Bound bound in subjectBoundList)
        {
            bound.CollisionBoundAction += DestroyNow;
        }
    }

    private void OnDisable()
    {
        subjectPlayerCollision.CollisionPowerUpTarget -= MakeEffect;
        subjectPlayerCollision.CollisionPowerUpObject -= DestroyNow;

        foreach (Bound bound in subjectBoundList)
        {
            bound.CollisionBoundAction -= DestroyNow;
        }
    }

    public virtual void Move(Vector2 direction)
    {
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    public abstract void Init(Vector2 position, Vector2 direction);
    public abstract void MakeEffect(GameObject target);

    public void DestroyNow(GameObject objToDestroy)
    {
        objToDestroy.SetActive(false);
    }
}
