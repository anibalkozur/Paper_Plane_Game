using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneController : MonoBehaviour
{
    // Velocidad de movimiento del avión
    public float speed = 10f;

    // Velocidad de caída del avión
    public float fallSpeed = 0.5f;

    // Límites de movimiento en el eje X
    public float minX = -10f;
    public float maxX = 10f;

    // Límite mínimo en el eje Y
    public float minY = -5f;

    void Update()
    {
        // Obtén la entrada horizontal del teclado (A/D o flechas izquierda/derecha)
        float horizontal = Input.GetAxis("Horizontal");

        // Calcula el movimiento deseado en el eje X
        float moveX = horizontal * speed * Time.deltaTime;

        // Calcula la nueva posición en el eje X
        float newXPosition = transform.position.x + moveX;

        // Aplica los límites a la nueva posición en el eje X
        newXPosition = Mathf.Clamp(newXPosition, minX, maxX);

        // Calcula la nueva posición en el eje Y con una caída suave
        float newYPosition = transform.position.y - fallSpeed * Time.deltaTime;

        // Aplica el límite mínimo en el eje Y
        newYPosition = Mathf.Clamp(newYPosition, minY, Mathf.Infinity);

        // Actualiza la posición del avión
        transform.position = new Vector3(newXPosition, newYPosition, transform.position.z);

        // Verifica la dirección de movimiento y ajusta la escala del transform para voltear el sprite
        if (horizontal < 0) // Mover a la izquierda
        {
            // Voltea el sprite en el eje X
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontal > 0) // Mover a la derecha
        {
            // Restaura el sprite a su escala normal
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
