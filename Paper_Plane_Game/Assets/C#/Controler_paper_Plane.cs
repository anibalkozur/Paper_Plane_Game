using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneController : MonoBehaviour
{
    // Velocidad de movimiento del avión en el eje X
    public float speed = 4f;

    // Velocidad de caída del avión en el eje Y
    public float fallSpeed = 0.4f;

    // Límites de movimiento en el eje X
    public float minX = -9f;
    public float maxX = 9f;

    // Límite mínimo en el eje Y
    public float minY = -5f;

    //Límite máximo en el eje y

    public float maxY = 5f;

    // Dirección actual del movimiento en el eje X: 1 para derecha, -1 para izquierda
    private int direction = 1;
    private int stop = 1;

        // Velocidad de rotación del sprite
    public float rotationSpeed = 60f;

    void Update()
    {
        // Detecta si se presiona la tecla A o D para cambiar la dirección
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -1; // Cambia la dirección a la izquierda
            stop = 1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = 1; // Cambia la dirección a la derecha
            stop = 2;
        }


        // Rotación gradual de la nariz del avion hacia arriba
        if (Input.GetKey(KeyCode.W))
        {
            RotateSprite(-1);
        }
        // Rotación gradual de la nariz del avion hacia hacia abajo
        else if (Input.GetKey(KeyCode.S))
        {
            RotateSprite(1);
        }


        // Movimiento automático en el eje X según la dirección
        float moveX = direction * speed * Time.deltaTime;

        // Caída suave en el eje Y
        float moveY = -fallSpeed * Time.deltaTime;

        // Calcula la nueva posición en el eje X
        float newXPosition = transform.position.x + moveX;

        // Calcula la nueva posición en el eje Y
        float newYPosition = transform.position.y + moveY;

        // Aplica los límites a la nueva posición en el eje X
        newXPosition = Mathf.Clamp(newXPosition, minX, maxX);

        // Aplica el límite mínimo en el eje Y
        newYPosition = Mathf.Clamp(newYPosition, minY, maxY);

        // Actualiza la posición del avión en los ejes X y Y
        transform.position = new Vector3(newXPosition, newYPosition, transform.position.z);

        // Verifica la dirección de movimiento y ajusta la escala del transform para voltear el sprite
        if (direction < 0 && stop == 1) // Mover a la izquierda
        {
            // Voltea el sprite en el eje X
            transform.localScale = new Vector3(-1, 1, 1);

            float currentRotation = transform.rotation.eulerAngles.z;
            float correctRotation = currentRotation * -1;
            transform.rotation = Quaternion.Euler(0, 0, correctRotation);
            stop = 0;

        }
        else if (direction > 0 && stop == 2) // Mover a la derecha
        {
            // Restaura el sprite a su escala normal
            transform.localScale = new Vector3(1, 1, 1);

            float currentRotation = transform.rotation.eulerAngles.z;
            float correctRotation = currentRotation * -1;
            transform.rotation = Quaternion.Euler(0, 0, correctRotation);
            stop = 0;
        }
    }

        void RotateSprite(int rotationDirection)
    {
        // Calcula la rotación actual del objeto
        float currentRotation = transform.rotation.eulerAngles.z;

        // Calcula la nueva rotación basada en la velocidad de rotación y el tiempo
        float newRotation = currentRotation + rotationDirection * rotationSpeed * Time.deltaTime;

        // Aplica la rotación al objeto
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }
}
