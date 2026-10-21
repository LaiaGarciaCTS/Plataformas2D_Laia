using UnityEngine;
using UnityEngine.InputSystem;

public class Mimik : MonoBehaviour
{
    [SerializeField]private int _maxHealth = 4;
    [SerializeField]private int _actualHealth;

    private Animator _animator;
    private int _attackEnemy = 2;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _attackSound;


    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Attack()
    {
        _animator.SetTrigger("IsAttacking");


        if (_audioSource != null && _attackSound != null)
        {
            _audioSource.PlayOneShot(_attackSound);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _actualHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        _actualHealth -= damage;
        
        if(_actualHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController _playerScript = collision.gameObject.GetComponent<PlayerController>();
            _playerScript.TakeDamage(_attackEnemy);
            Attack();
        }
    }
}