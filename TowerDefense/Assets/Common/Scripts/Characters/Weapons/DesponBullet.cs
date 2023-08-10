using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesponBullet : MonoBehaviour
{
    [SerializeField] private float timeSpawnBullet;

    private void Start()
    {
        if (gameObject != null)
        {
            Invoke(nameof(OnDespown), timeSpawnBullet);
        }
    }

    private void OnDespown()
    {
        Destroy(gameObject);
    }

}