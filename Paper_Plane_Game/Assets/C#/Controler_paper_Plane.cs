using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlaneController : MonoBehaviour
{
    public float speed = 4f; // Velocidad inicial
    public float fallSpeed = 0.2f; // Velocidad de caída inicial
    public float minX = -9f;
    public float maxX = 9f;
    public float minY = -5f;
    public float maxY = 5f;
    private int direction = 1; // 1 para derecha, -1 para izquierda
    private int previousDirection = 1; // Para rastrear la dirección anterior
    private int stop = 0;
    public float rotationSpeed = 60f;
    public float maxRotationAngle = 45f;
    private float initialSpeed; // Para almacenar la velocidad inicial
    public float maxSpeed = 8f; // Velocidad máxima permitida
    private float maxFallSpeed = 2f; // Velocidad de caída máxima

    void Start()
    {
        initialSpeed = speed; // Guardar la velocidad inicial
    }

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

        // Ajustar la velocidad de caída y la velocidad del avión según el ángulo de la nariz
        AdjustSpeedAndFallSpeed();

        // Movimiento basado en la dirección y el ángulo de la nariz
        MovePlane();

        // Aplica los límites a la nueva posición
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z);

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

    // Método para ajustar la velocidad y la velocidad de caída
    void AdjustSpeedAndFallSpeed()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        currentRotation = NormalizeAngle(currentRotation);

        // Considera la dirección para determinar la inclinación
        if (direction == 1) // Movimiento a la derecha
        {
            if (currentRotation < 0)
            {
                // Nariz hacia abajo
                speed = initialSpeed + Mathf.Abs(currentRotation) / maxRotationAngle * initialSpeed;
            }
            else if (currentRotation > 0)
            {
                // Nariz hacia arriba
                speed = initialSpeed - currentRotation / maxRotationAngle * initialSpeed;
                speed = Mathf.Max(speed, 0f);
            }
        }
        else if (direction == -1) // Movimiento a la izquierda
        {
            if (currentRotation > 0)
            {
                // Nariz hacia abajo (pero hacia la izquierda)
                speed = initialSpeed + Mathf.Abs(currentRotation) / maxRotationAngle * initialSpeed;
            }
            else if (currentRotation < 0)
            {
                // Nariz hacia arriba (pero hacia la izquierda)
                speed = initialSpeed - Mathf.Abs(currentRotation) / maxRotationAngle * initialSpeed;
                speed = Mathf.Max(speed, 0f);
            }
        }

        // Limitar la velocidad máxima
        speed = Mathf.Clamp(speed, 0f, maxSpeed);

        // Ajustar la velocidad de caída según la velocidad actual
        if (speed < 2f)
        {
            fallSpeed = maxFallSpeed * (2f - speed) / 2f;
        }
        else
        {
            fallSpeed = 0.4f;
        }
    }

    // Método para mover el avión en la dirección en la que apunta la nariz
    void MovePlane()
    {
        // Calcula la dirección de movimiento en función del ángulo de inclinación
        Vector3 moveDirection;
        
        if (direction == 1)
        {
            moveDirection = transform.right; // Derecha
        }
        else
        {
            moveDirection = -transform.right; // Izquierda
        }

        // Movimiento basado en la dirección y la velocidad
        transform.position += moveDirection * speed * Time.deltaTime;

        // Caída en el eje Y según la velocidad de caída ajustada
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
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
