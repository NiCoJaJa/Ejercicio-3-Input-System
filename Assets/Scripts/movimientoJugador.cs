using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float velocidadCorrer = 9f;
    public float fuerzaSalto = 7f;

    private Rigidbody rb;
    private bool estaEnSuelo;

    private float movimientoHorizontal;
    private float movimientoVertical;
    private float velocidadActual;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        print("Teclas de movimiento: W, A, S, D. Tecla para correr: Shift izquierdo. Tecla para saltar: Espacio.");
    }

    void Update()
    {
        // Captar las teclas de movimiento
        movimientoHorizontal = 0f;
        movimientoVertical = 0f;

        if (Input.GetKey(KeyCode.W))
            movimientoVertical = 1f;

        if (Input.GetKey(KeyCode.S))
            movimientoVertical = -1f;

        if (Input.GetKey(KeyCode.D))
            movimientoHorizontal = 1f;

        if (Input.GetKey(KeyCode.A))
            movimientoHorizontal = -1f;

        // Detectar correr
        velocidadActual = velocidad;

        if (Input.GetKey(KeyCode.LeftShift))
            velocidadActual = velocidadCorrer;

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            estaEnSuelo = false;
        }
    }

    private void FixedUpdate()
    {
        // Movimiento con Rigidbody
        Vector3 movimiento = new Vector3(
            movimientoHorizontal,
            0f,
            movimientoVertical
        );

        movimiento = movimiento.normalized;

        Vector3 nuevaPosicion = rb.position +
            movimiento * velocidadActual * Time.fixedDeltaTime;

        rb.MovePosition(nuevaPosicion);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            foreach (ContactPoint contacto in collision.contacts)
            {
                if (Vector3.Dot(contacto.normal, Vector3.up) > 0.5f &&
                    rb.linearVelocity.y <= 0.1f)
                {
                    estaEnSuelo = true;
                    return;
                }
            }
        }
    }

    
}
