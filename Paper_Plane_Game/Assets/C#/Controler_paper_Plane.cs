using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneController : MonoBehaviour
{
    public float speed = 4f;
    public float fallSpeed = 0.4f;
    public float minX = -9f;
    public float maxX = 9f;
    public float minY = -5f;
    public float maxY = 5f;
    private int direction = 1; // 1 para derecha, -1 para izquierda
    private int previousDirection = 1; // Para rastrear la dirección anterior
    private int stop = 0;
    public float rotationSpeed = 60f;
    public float maxRotationAngle = 45f;

    void Update()
    {
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

        if (Input.GetKey(KeyCode.W))
        {
            RotateSprite(-direction);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            RotateSprite(direction);
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
        // Verifica si la dirección ha cambiado y ajusta la escala del transform para voltear el sprite
        if (direction != previousDirection)
        {
            if (direction == -1 && stop == 1) // Mover a la izquierda
            {
                // Voltea el sprite en el eje X
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction == 1 && stop == 2) // Mover a la derecha
            {
                // Restaura el sprite a su escala normal
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Alinea la rotación solo cuando cambia la dirección
            CorrectSpriteRotation();
            
            // Actualiza la dirección anterior
            previousDirection = direction;
        }
    }

    // Método para rotar el sprite gradualmente con límites
    void RotateSprite(int rotationDirection)
    {
        // Calcula la rotación actual del objeto
        float currentRotation = transform.rotation.eulerAngles.z;
        currentRotation = NormalizeAngle(currentRotation);

        // Calcula la nueva rotación basada en la velocidad de rotación y el tiempo
        float newRotation = currentRotation + rotationDirection * rotationSpeed * Time.deltaTime;

        // Limita la rotación a un rango de -45 a 45 grados
        if (newRotation > maxRotationAngle)
        {
            newRotation = maxRotationAngle;
        }
        else if (newRotation < -maxRotationAngle)
        {
            newRotation = -maxRotationAngle;
        }

        // Aplica la rotación al objeto
        transform.rotation = Quaternion.Euler(0, 0, newRotation);
    }

    // Método para normalizar el ángulo de rotación entre -180 y 180 grados
    float NormalizeAngle(float angle)
    {
        angle = (angle + 180f) % 360f - 180f;
        return angle;
    }

    // Método para alinear la rotación del sprite al cambiar de dirección
    void CorrectSpriteRotation()
    {
        // Corrige la rotación para mantener la nariz del avión hacia arriba al invertir la dirección
        float currentRotation = transform.rotation.eulerAngles.z;
        float correctRotation = -currentRotation;
        transform.rotation = Quaternion.Euler(0, 0, correctRotation);
    }
}

