using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void KillConfirmed(Character character);

public class GameManager : MonoBehaviour
{
    public event KillConfirmed killConfirmedEvent;

    private Camera mainCamera;

    private static GameManager instance;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }


    [SerializeField]
    private Player player;

    private Enemy currentTarget;
    private int targetIndex;

    public void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ClickTarget();

        NextTarget();
    }

    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                DeSelectTarget();
                SelectTarget(hit.collider.GetComponent<Enemy>());
            }

            else
            {
                UIManager.MyInstance.HideTargetFrame();

                DeSelectTarget();

                currentTarget = null;
                player.MyTarget = null;
            }
        }
        else if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            IInteractable entity = hit.collider.gameObject.GetComponent<IInteractable>();

            if (hit.collider != null && (hit.collider.tag == "Enemy" || hit.collider.tag == "Interactable") && player.MyInteractables.Contains(entity))
            {
                entity.Interact();
            }
        }
    }

    private void NextTarget()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            DeSelectTarget();
            if (Player.MyInstance.MyAttackers.Count > 0)
            {
                SelectTarget(Player.MyInstance.MyAttackers[targetIndex]);
                targetIndex++;

                if (targetIndex >= Player.MyInstance.MyAttackers.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void DeSelectTarget()
    {
        if(currentTarget != null)
        {
            currentTarget.DeSelect();
        }
    }

    private void SelectTarget(Enemy enemy)
    {
        currentTarget = enemy;
        player.MyTarget = currentTarget.Select();
        UIManager.MyInstance.ShowTargetFrame(currentTarget);
    }

    public void OnKillConfirmed(Character character)
    {
        if (killConfirmedEvent != null)
        {
            killConfirmedEvent(character);
        }
    }
}
