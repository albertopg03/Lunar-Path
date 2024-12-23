using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckController : MonoBehaviour
{
    [SerializeField] private GameObject parentPoints;
    [SerializeField] private List<GameObject> pointList;
    [SerializeField] private GameObject check;
    [SerializeField] private PlayerCollision subjectPlayerCollision;

    private int currentNodeIndex = 0;
    private List<int> initNodeIndexList = new List<int>(0);
    private List<int> currentNodeIndexList = new List<int>(0);

    private GameObject checkCreated;

    private Animator anim;

    private void Awake()
    {
        for(int i = 0; i < parentPoints.transform.childCount; i++)
        {
            pointList.Add(parentPoints.transform.GetChild(i).gameObject);
            initNodeIndexList.Add(i);
        }
    }

    private void Start()
    {
        Init();

        subjectPlayerCollision.CollisionCheckAction += SetNextCheckPosition;
    }

    private void OnDestroy()
    {
        subjectPlayerCollision.CollisionCheckAction -= SetNextCheckPosition;
    }

    private void Init()
    {
        checkCreated = Instantiate(check, pointList[Random.Range(1, pointList.Count)].transform.position, Quaternion.identity);

        anim = checkCreated.GetComponent<Animator>();
    }

    private void SetNextCheckPosition()
    {
        currentNodeIndexList = new List<int>(initNodeIndexList);

        currentNodeIndexList.Remove(currentNodeIndex);

        int randomIndexInList = Random.Range(0, currentNodeIndexList.Count);
        int nextNodeIndex = currentNodeIndexList[randomIndexInList];

        checkCreated.transform.position = pointList[nextNodeIndex].transform.position;
        ExecuteAnimation();

        currentNodeIndex = nextNodeIndex;
    }

    private void ExecuteAnimation()
    {
        anim.SetTrigger("Activate");
    }
}
