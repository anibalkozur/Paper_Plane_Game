using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_move : MonoBehaviour
{
    public float speed = 5.0f;  // Velocidad de movimiento
    public float minX = -9f;
    public float maxX = 9f;
    private Vector3 direction;

    void Start()
    {
        // Inicializa la dirección en una dirección aleatoria
        direction = GetRandomDirection();
        FlipSprite(direction);
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve el objeto en la dirección actual
        transform.Translate(direction * speed * Time.deltaTime);

        // Cambia la dirección aleatoriamente en intervalos de tiempo
        if (Random.Range(0f, 1f) < 0.01f)
        {
            direction = GetRandomDirection();
            FlipSprite(direction);
        }

        // Aplica los límites a la nueva posición
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX), 
            transform.position.y,
            transform.position.z);
    }

    Vector3 GetRandomDirection()
    {
        // Genera una dirección aleatoria
        float x = Random.Range(-1f, 1f);
        float y = 0f;  // Mantener y en 0 para movimiento en el plano XZ
        float z = 0f;
        return new Vector3(x, y, z).normalized;
    }

    void FlipSprite(Vector3 direction)
    {
        // Si se está moviendo hacia la derecha, voltea el sprite hacia la derecha
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // Si se está moviendo hacia la izquierda, voltea el sprite hacia la izquierda
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}


