using UnityEngine;

public class Health : MonoBehaviour
{

    private AudioSource _healthAudioSource;
    [SerializeField]private AudioClip _healthAudio;

    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]private int _maxHealth = 100;
    [SerializeField]private int _actualHealth;


    void Awake()
    {
        _healthAudioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    void PlaySFX()
    {
        _healthAudioSource.PlayOneShot(_healthAudio);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.AddHealth();
            PlaySFX();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
