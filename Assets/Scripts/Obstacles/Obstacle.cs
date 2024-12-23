using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Obstacle : MonoBehaviour, IDestruible
{
    [SerializeField] private float speed;
    private PlayerCollision subjectPlayerCollision;
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

    private void OnEnable()
    {
        //StartCoroutine(DestroyByTime(gameObject, timeToDestroy));

        subjectPlayerCollision.CollisionActionObject += DestroyNow; // aquí ejecutaré próximamente una animación que hará que se desactive el objeto

        foreach(Bound bound in subjectBoundList)
        {
            bound.CollisionBoundAction += DestroyNow;
        }
    }

    private void OnDisable()
    {
        subjectPlayerCollision.CollisionActionObject -= DestroyNow;

        foreach (Bound bound in subjectBoundList)
        {
            bound.CollisionBoundAction -= DestroyNow;
        }
    }

    public virtual void Move(Vector2 direction)
    {
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    public abstract void InitializeObstacle(Vector2 position, Vector2 direction);

    public void DestroyNow(GameObject objToDestroy)
    {
        objToDestroy.SetActive(false);
    }
}