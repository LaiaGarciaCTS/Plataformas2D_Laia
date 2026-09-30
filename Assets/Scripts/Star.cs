using UnityEngine;

public class Star : MonoBehaviour
{

    private AudioSource _starAudioSource;
    [SerializeField]private AudioClip _starAudio;

    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;


    void Awake()
    {
        _starAudioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    void PlaySFX()
    {
        _starAudioSource.PlayOneShot(_starAudio);
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
            GameManager.Instance.AddStar();
            PlaySFX();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
    
}
