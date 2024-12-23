using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveFollower : Obstacle, IRandomScalable, IOrientable
{

    [SerializeField] private float timeToFollow;
    [SerializeField] private float speedFollow;
    [SerializeField] private GameObject target;

    public override void InitializeObstacle(Vector2 position, Vector2 direction)
    {
        rb.velocity = Vector2.zero;

        transform.position = position;
        Move(direction);
        RandomScale(0.75f, 1.75f);
        LookAt(direction);

        StartCoroutine(FollowTarget());
    }

    public void LookAt(Vector2 moveDirection)
    {
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void RandomScale(float min, float max)
    {
        float scale = Random.Range(min, max);
        transform.localScale = new Vector2(scale, scale);
    }

    private IEnumerator FollowTarget()
    {
        yield return new WaitForSeconds(timeToFollow);

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            LookAt(rb.velocity.normalized); 
        }
        else
        {
            Vector3 targetPosition = playerMovement.gameObject.transform.position;
            Vector2 direction = targetPosition - transform.position;

            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * speedFollow, ForceMode2D.Impulse);

            LookAt(direction.normalized);
        }
    }

}
