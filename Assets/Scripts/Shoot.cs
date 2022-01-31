using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField]
    float speed = 3.0f;

    void Start()
    {
        // Si el disparo es enemigo, se dirige hacia abajo
        if (gameObject.CompareTag("SmallEnemyShoot") || gameObject.CompareTag("BigEnemyShoot")) speed *= -1;
    }

    void Update()
    {
        // Movemos el disparo con MRU
        transform.position += new Vector3(0.0f, speed * Time.deltaTime, 0.0f);
        // Eliminamos el disparo al salir del escenario
        if (transform.position.y > 6.0f || transform.position.y < -4.0f) Destroy(gameObject);
    }

    
}
