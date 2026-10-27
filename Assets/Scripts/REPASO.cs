using UnityEngine;
using UnityEngine.InputSystem; //para poder acceder al input system
using UnityEngine.SceneManagement; //para poder cambiar de escenas

public class REPASO : MonoBehaviour
{
    //REPASO EXAMEN 2D


    /* ESCENARIO - PALETA:
    
    Windows - 2D - Tile Palette - Crear new Palette
    Crear una carpeta para guardar la paleta
    Buscar los sprites y arrastrarlos, guardarlos en la carpeta de paleta
    Se puede editar la paleta (mover, duplicar, borrar...) solo pulsando el lápiz al lado de la cuadrícula y seleccionándolo con el puntero de arriba a la izquierda.
    
    Pulsar a create tilemap para empezar a pintar.
    Pulsar el cuentagotas y seleccionar los tiles que quieras pintar y colocarlos en la escena.

    Seleccionar el cuentagotas cara vez que queramos seleccionar sprites diferentes, y luego pintar en la escena.

    (Hacer un escenario del tamaño de la cámara x3)


    Para poner colider en los sprites de la paleta:
    
    Seleccionar Tilemap - Inspector - añadir tilemap collider 2D (se ajusta automáticamente).
    Seleccionar Tilemap - Inspector - añadir composite collider 2D (junta los colliders de los tiles).
    
    Cambiar el Rigidbody de dynamic a static para que el escenario no caiga.
    Cambiar el tilemap collider - Composite Operation de None a Merge.
    */


    /* CÁMARA - CINEMACHINE:
    Seleccionar la caja de Package Manager - Unity Registry - Cinemachine

    Añadir en el inspector la CinemachineCamera.
    Seleccionamos “CinemachineCamera” y añadimos el objeto que queremos que siga la cámara en el apartado de “Tracking Target".

    Añadir un componente “Cinemachine Position Composer” para elegir parámetros.

    Para añadir límites necesitamos una extensión - CinemachineCamera - Add Extensions - Select - Cinemachine Confiner 2D - (y se añadirá en el inspector de la cámara)
    
    Crear un empty - añadirle un Box Collider 2D - Activar Is Trigger - Editar el Box Collider 2D para editar los bordes (los límites de la cámara)
    */


    /* MOVIMIENTO PERSONAJE + SALTO:

    En el personaje añadir un rigidbody y un Box Collider 2D (o una Capsule Collider 2D)
    Para que el personaje no rote - Rigidbody - Constraints - Freeze Rotation - Activar Z

    Mirar en el InputSystem para saber qué teclas hay que pulsar para que el personaje haga la acción.
    */


    /*GROUND SENSOR:
    
    Crear un empty con el nombre de Ground Sensor

    */
    

    /* ANIMACIÓN:
    
    Window - Animation - Animation
    Window - Animation - Animator

    Seleccionar el personaje - Crear animación - Guardar animación - Idle(con nombre de la animación, ej. Idle) - Activar Loop Time
    Seleccionar el personaje - Crear animación - Guardar animación - Jump - Desactivar Loop Time
    Seleccionar el personaje - Crear animación - Guardar animación - Run - Activar Loop Time

    Crear dos booleanas:
        - IsRunning
        - IsJumping

    En el Animator hacer las transiciones:
        - Idle --- Run: IsRunning True - Desactivar Exit Time
        - Idle --- Jump: IsJumping True - Desactivar Exit Time
        - Run --- Idle: IsRunning False - Desactivar Exit Time
        - Run --- Jump: IsJumping True - Desactivar Exit Time
        - Jump --- Idle: IsJumping False - IsRunning False - Desactivar Exit Time
        - Jump --- Run: IsRunning True - IsJumping False - Desactivar Exit Time
        (Desactivar Exit Time en todas)
    */

    

    //Código personaje:
    
    //using UnityEngine;
    //using UnityEngine.InputSystem;

    //Campo Rigidbody personaje
    private Rigidbody _rBody;

    //Campo InputAction de salto y movimiento
    private InputAction _jumpAction;
    private InputAction _moveAction;

    //Campo Vector2 para el movimiento
    private Vector2 _moveInput;

    //Campo velocidad y fuerza de salto:
    [SerializeField]private float _movementSpeed = 10f;
    [SerializeField]private float _jumpForce = 5f;

    //Campo GroundSensor:
    [SerializeField]private transform _groundSensor;
    [SerializeField]private float _sensorRadius = 0.5f;

    //Campo Animator:
    private Animator _animator;


    void Awake()
    {
        _rBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _jumpAction = InputSystem.actions["Jump"]();
        _moveAction = InputSystem.actions["Move"]();
    }

    void Update()
    {
        //El input de move = a la accion de moverse en vector2 (eje X)
        _moveInput = _moveAction.ReadValue<Vector2>();

        //Cuando el personaje se mueva en un eje mayor que 0, que vaya hacia la derecha (eje X positivo)
        if(_moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            //Animación de correr para cuando se mueva en el eje X.
            _animator.SetBool("IsRunning", true);
        }

        //Cuando el personaje se mueva en un eje menor que 0, que vaya hacia la izquierda (eje X negativo)
        else if(_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);

            //Animación de correr para cuando se mueva en el eje X.
            _animator.SetBool("IsRunning", true);
        }

        //Cuando no se esté moviendo, que no haga la animación de correr.
        _animator.SetBool("IsRunning", false);

        //Cuando se pulse el botón de saltar que el personaje salte y solamente cuando esté en el suelo
        if(_jumpAction.WasPressedThisFrame() && IsGrounded)
        {
            Jump();
        }

        //Comprueba el valor del GroundSensor, cuando NO esté en el suelo que haga la animación de salto.
        _animator.SetBool("IsJumping", !IsGrounded);
    }

    //Para que el personaje se mueve en el eje X:
    void FixedUpdate()
    {
        _rBody.linearVelocityX = _moveInput.x * _movementSpeed;
    }

    //Función de salto:
    void Jump()
    {
        _rBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    //Función para que detecte el suelo y que devuelva si es verdadero o falso (si está en el suelo o no):
    bool IsGrounded()
    {
        //Collider con forma de circulo donde hayamos puesto el empty de ground sensor y comprovará todo lo que haya dentro del circulo, cuando el suelo esté dentro querrá decir que el suelo está dentro del círculo.
        //Crear una layer con el nombre Ground y ponerla en el GroundSensor.
        Collider2D[] collider2D = Physics2D.OverlapCircleAll(_groundSensor.position, _sensorRadius);

        foreach (Collider2D item in collider2D)
        {
            if(item.gameObject.layer == 6)
            {
                return true;
            }
        }

        return false;
    }
    


    /* CREAR NUEVA ESCENA - GAME OVER:
    Añadir una estrella en escena
    Crear un Tag con el nombre Estrella
    Ponerle a la estrella el Tag de Estrella

    Crear un Tag con el nombre Player
    Ponerle al personaje el Tag de Player

    Crear un Script para Estrella
    Añadir el Script de Estrella a la Estrella

    Crear una nueva escena con el nombre "Game Over"
    Agregarla en build profiles (con la escena seleccionada)

    En el inspector añadir un canvas (poner sprites, panel, botones...)
    */

    //Código estrella:

    using UnityEngine;
    using UnityEngine.SceneManagement;
    
    //Cuando un objeto con el Tag "Player" colisione con la estrella, que cargue la escena de "Game Over"
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Game Over")
        }
    }


    /* RECOGER MONEDAS CON CONTADOR:

    Crear un script con el nombre GameManager
    Crear un empty con el nombre Game Manager y añadirle el script de GameManager

    Añadir una estrella en la escena

    Crear un canvas para poner el contador de las monedas; poner un texto y una imagen
    En el Game Manager añadir el texto que hemos creado (_coinsText) para que se vea la suma de las monedas

    Script para que se destruyan los Game Managers duplicados y para que se sumen las monedas en el Game Manager
    */

    //Código GameManager:

    using UnityEngine;
    using UnityEngine.UI;

    //Campo Instance
    public static GameManager Instance;

    //Campo Monedas
    public int coins = 0;

    //Campo Texto Monedas
    [SerializeField]private Text _coinsText;


    void Awake()
    {
        //Función de Instance - para que se destruya el script duplicado de GameManager, por si hay más de uno.
        //No marranada
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
        }

        //Marranada - se puede hacer
        Instance = this;
    }

    //A la variable de monedas que sume 1 (esto se verá en el Game Manager)
    public void AddCoins()
    {
        coins ++;
    }
    

    /*
    Crear un script para la moneda
    */

    //Código Moneda.
 
    using Unity.Engine;
    
    //Cuando un objeto con el Tag "Player" colisione con la moneda, se sumarán las monedas en el Game Manager y luego se destruirán.
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddCoins();
            Destroy(gameObject);
        }
    }
}