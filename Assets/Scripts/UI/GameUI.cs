using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text textTimer;
    [SerializeField] private GameObject healthUIParent;
    [SerializeField] private GameObject lifeUIPrefab;
    [SerializeField] private TMP_Text textPoints;
    [SerializeField] private GameObject pauseMenu;

    [Space]
    [Header("SUBJECTS")]
    [SerializeField] private PlayerCollision subjectPlayerCollision;
    [SerializeField] private PlayerHealth subjectPlayerHealth;
    [SerializeField] private PlayerPoints subjectPlayerPoints;
    [SerializeField] private GameLoop subjectGameLoop;

    [SerializeField] private PlayerInputHandler inputHandler;

    private List<GameObject> lifeUIObjects = new List<GameObject>();

    private bool inPause = false;
    private bool inDeath = false;

    private void Awake()
    {
        subjectPlayerCollision.CollisionAction += Test;
        subjectPlayerPoints.OnAddPoints += UpdatePoints;
    }

    private void OnEnable()
    {
        subjectGameLoop.OnResetGame += Init;
        subjectPlayerHealth.OnLivesChanged += UpdateHealthUI;

        inputHandler.ActivateMenuPause += MenuPause;
    }

    private void OnDisable()
    {
        subjectGameLoop.OnResetGame -= Init;
        inputHandler.ActivateMenuPause -= MenuPause;
    }

    public void Init()
    {
        subjectPlayerHealth.OnLivesChanged += UpdateHealthUI;

        // puntos
        textPoints.text = "0";

        // tiempo
        Utils.ResetTimer();

        // UI de vida
        ResetUIHealth();
    }

    private void Start()
    {
        InitUIHealth();
    }

    private void Update()
    {
        textTimer.text = Utils.GetCurrentTimer();
    }

    private void OnDestroy()
    {
        subjectPlayerCollision.CollisionAction -= Test;
        subjectPlayerHealth.OnLivesChanged -= UpdateHealthUI;
        subjectPlayerPoints.OnAddPoints -= UpdatePoints;
    }

    private void Test()
    {
        Debug.Log("UI detecta colisión");
    }

    private void InitUIHealth()
    {
        inDeath = false;

        for (int i = 0; i < subjectPlayerHealth.InitiLives; i++)
        {
            GameObject lifeUI;
            if (i < lifeUIObjects.Count)
            {
                // Reutilizar objetos existentes
                lifeUI = lifeUIObjects[i];
                lifeUI.SetActive(true);
            }
            else
            {
                // Crear nuevos objetos si no existen
                lifeUI = Instantiate(lifeUIPrefab);
                lifeUI.transform.SetParent(healthUIParent.transform);
                lifeUI.transform.localScale = new Vector3(1, 1, 1);
                lifeUIObjects.Add(lifeUI);
            }
        }

        // Desactivar cualquier objeto de vida sobrante
        for (int i = subjectPlayerHealth.InitiLives; i < lifeUIObjects.Count; i++)
        {
            lifeUIObjects[i].SetActive(false);
        }
    }

    private void ResetUIHealth()
    {
        foreach (var lifeUI in lifeUIObjects)
        {
            lifeUI.SetActive(false); // Desactiva todas las vidas temporalmente
        }

        InitUIHealth(); // Reactiva las necesarias
    }

    private void UpdateHealthUI(int lives)
    {
        if (lives == 0)
            inDeath = true;

        if (lives >= 0 && lives < subjectPlayerHealth.InitiLives)
        {
            lifeUIObjects[subjectPlayerHealth.InitiLives - (lives + 1)]
                .GetComponent<Animator>().SetTrigger("Action");
        }
        else if (lives < 0)
        {
            subjectPlayerHealth.OnLivesChanged -= UpdateHealthUI;
        }
    }

    private void UpdatePoints()
    {
        textPoints.text = subjectPlayerPoints.Points.ToString();
    }

    private void MenuPause()
    {
        if (!inDeath)
        {
            inPause = !subjectGameLoop.InPause;
            subjectGameLoop.SetScaleTime(inPause);
            pauseMenu.SetActive(inPause);
        }
    }
}
