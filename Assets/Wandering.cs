using UnityEngine;

public class Wander : MonoBehaviour
{
    public float maxSpeed = 4f;                   // Velocidad m�xima
    public float maxForce = 0.4f;                 // Fuerza m�xima
    public float wanderRingDistance = 3f;         // Distancia al c�rculo de deambulaci�n
    public float wanderRingRadius = 2f;           // Radio del c�rculo de deambulaci�n
    public bool useImprovedWander = true;         // Alterna entre wandering b�sico y mejorado

    private Vector3 velocity;                     // Velocidad actual
    private Vector3 wanderTarget;                 // Objetivo para el wandering b�sico
    private float wanderUpdateTime = 2f;          // Tiempo entre actualizaciones del wander b�sico
    private float timeSinceLastWander;            // Tiempo desde la �ltima actualizaci�n del wander

    void Start()
    {
        // Inicializar wander target
        timeSinceLastWander = wanderUpdateTime;
    }

    void Update()
    {
        if (useImprovedWander)
        {
            // Wander mejorado
            velocity += WanderImproved();
        }
        else
        {
            // Wander b�sico
            timeSinceLastWander += Time.deltaTime;
            if (timeSinceLastWander > wanderUpdateTime)
            {
                timeSinceLastWander = 0f;
                wanderTarget = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            }

            velocity += SeekTarget(wanderTarget);
        }

        // Limita la velocidad a la velocidad m�xima
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;

        // Actualiza la posici�n
        transform.position += velocity * Time.deltaTime;

        // Gira el objeto hacia la direcci�n del movimiento
        if (velocity != Vector3.zero)
            transform.forward = velocity.normalized;
    }

    Vector3 SeekTarget(Vector3 targetPos)
    {
        // Comportamiento Seek para deambular hacia un objetivo
        Vector3 desired = (targetPos - transform.position).normalized * maxSpeed;
        Vector3 steer = desired - velocity;

        if (steer.magnitude > maxForce)
            steer = steer.normalized * maxForce;

        return steer;
    }

    Vector3 WanderImproved()
    {
        // Proyecta un punto en el futuro seg�n la velocidad actual
        Vector3 futurePos = transform.position + velocity.normalized * wanderRingDistance;

        // Desplaza un objetivo dentro del c�rculo alrededor del punto futuro
        float randomAngle = Random.Range(0f, 360f);
        Vector3 displacement = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * wanderRingRadius;
        Vector3 wanderPoint = futurePos + displacement;

        return SeekTarget(wanderPoint);
    }

    void OnDrawGizmos()
    {
        // Dibuja una l�nea verde representando la velocidad
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        if (useImprovedWander)
        {
            // Dibuja el c�rculo de wandering mejorado
            Vector3 futurePos = transform.position + velocity.normalized * wanderRingDistance;
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(futurePos, wanderRingRadius);

            Gizmos.color = Color.cyan;
            float randomAngle = Random.Range(0f, 360f);
            Vector3 displacement = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)) * wanderRingRadius;
            Vector3 wanderPoint = futurePos + displacement;
            Gizmos.DrawLine(futurePos, wanderPoint);
        }
        else
        {
            // Dibuja el objetivo aleatorio en wandering b�sico
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(wanderTarget, 0.2f);
        }
    }
}
