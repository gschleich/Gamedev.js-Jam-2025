using UnityEngine;
using System.Collections.Generic;

public class PortalActivator : MonoBehaviour
{
    [Header("Assign your portal GameObject here")]
    public GameObject portal;

    private List<GameObject> enemies = new List<GameObject>();
    private bool portalActivated = false;

    void Start()
    {
        if (portal == null)
        {
            Debug.LogError("PortalActivator: Portal not assigned!");
            return;
        }

        portal.SetActive(false); // start portal hidden

        GameObject[] enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemyArray)
        {
            if (enemy.activeInHierarchy)
            {
                enemies.Add(enemy);
            }
        }

        Debug.Log("Enemies found at start: " + enemies.Count);
    }

    void Update()
    {
        if (!portalActivated)
        {
            Debug.Log("Checking enemies...");

            bool allEnemiesDefeated = true;

            foreach (GameObject enemy in enemies)
            {
                if (enemy != null && enemy.activeInHierarchy)
                {
                    Debug.Log("Enemy still alive: " + enemy.name);
                    allEnemiesDefeated = false;
                    break;
                }
            }

            if (allEnemiesDefeated)
            {
                ActivatePortal();
            }
        }
    }


    private void ActivatePortal()
    {
        portal.SetActive(true);
        portalActivated = true;
        Debug.Log("All enemies defeated! Portal activated.");
    }
}
