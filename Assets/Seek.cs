using UnityEngine;

public class Seek : MonoBehaviour
{
    public Transform target;       // El objetivo hacia el que se mover� el objeto
    public float maxSpeed = 5f;    // Velocidad m�xima
    public float maxForce = 0.1f;  // Fuerza m�xima
    public float approachRadius = 6f; // Radio de aproximaci�n

    private Vector3 velocity;      // Velocidad actual del objeto

    void Update()
    {
        // Llama a la funci�n seek y actualiza la posici�n del objeto
        Vector3 steer = SeekTarget(target.position);
        velocity += steer;

        // Limita la velocidad a la velocidad m�xima
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;

        // Actualiza la posici�n del objeto
        transform.position += velocity * Time.deltaTime;

        // Gira el objeto hacia la direcci�n del movimiento
        if (velocity != Vector3.zero)
            transform.forward = velocity.normalized;
    }

    Vector3 SeekTarget(Vector3 targetPos)
    {
        // Calcula el vector deseado hacia el objetivo
        Vector3 desired = (targetPos - transform.position).normalized * maxSpeed;
        float distance = Vector3.Distance(transform.position, targetPos);

        // Si est� dentro del radio de aproximaci�n, reduce la velocidad
        if (distance < approachRadius)
        {
            desired *= distance / approachRadius;
        }

        // Calcula el vector de direcci�n (steering) limitando la fuerza
        Vector3 steer = desired - velocity;
        if (steer.magnitude > maxForce)
            steer = steer.normalized * maxForce;

        return steer;
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            // Dibuja una l�nea verde representando la velocidad
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + velocity);

            // Dibuja una l�nea roja representando el vector deseado hacia el objetivo
            Vector3 desired = (target.position - transform.position).normalized * maxSpeed;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + desired);

            // Dibuja el c�rculo blanco del radio de aproximaci�n alrededor del objetivo
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(target.position, approachRadius);
        }
    }
}
