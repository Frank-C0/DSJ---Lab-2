using UnityEngine;

public class Seek : MonoBehaviour
{
    public Transform target;       // El objetivo hacia el que se moverá el objeto
    public float maxSpeed = 5f;    // Velocidad máxima
    public float maxForce = 0.1f;  // Fuerza máxima
    public float approachRadius = 6f; // Radio de aproximación

    private Vector3 velocity;      // Velocidad actual del objeto

    void Update()
    {
        // Llama a la función seek y actualiza la posición del objeto
        Vector3 steer = SeekTarget(target.position);
        velocity += steer;

        // Limita la velocidad a la velocidad máxima
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;

        // Actualiza la posición del objeto
        transform.position += velocity * Time.deltaTime;

        // Gira el objeto hacia la dirección del movimiento
        if (velocity != Vector3.zero)
            transform.forward = velocity.normalized;
    }

    Vector3 SeekTarget(Vector3 targetPos)
    {
        // Calcula el vector deseado hacia el objetivo
        Vector3 desired = (targetPos - transform.position).normalized * maxSpeed;
        float distance = Vector3.Distance(transform.position, targetPos);

        // Si está dentro del radio de aproximación, reduce la velocidad
        if (distance < approachRadius)
        {
            desired *= distance / approachRadius;
        }

        // Calcula el vector de dirección (steering) limitando la fuerza
        Vector3 steer = desired - velocity;
        if (steer.magnitude > maxForce)
            steer = steer.normalized * maxForce;

        return steer;
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            // Dibuja una línea verde representando la velocidad
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + velocity);

            // Dibuja una línea roja representando el vector deseado hacia el objetivo
            Vector3 desired = (target.position - transform.position).normalized * maxSpeed;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + desired);

            // Dibuja el círculo blanco del radio de aproximación alrededor del objetivo
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(target.position, approachRadius);
        }
    }
}
