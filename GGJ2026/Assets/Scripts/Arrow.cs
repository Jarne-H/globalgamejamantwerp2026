using UnityEngine;
using UnityEngine.InputSystem;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private int damage = 1;

    public int pierceLevel = 0; // 0 = no pierce, 1 = pierce 1 enemy, etc.

    [SerializeField]
    private float _maxSplitAngles = 15f; // minimum and maximum angle for split arrows

    public Vector3 direction = Vector3.up; // Direction of the arrow

    private void Start()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        //rotate the transform to face the mouse position in 2D space
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        direction = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    //if collides with object with tag "Enemy", destroy both arrow and damage the enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Arrow hit enemy: " + collision.gameObject.name);
            // Damage the enemy
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.AdjustHealth(-damage);
            }
            if(pierceLevel <= 0)
            {
                Destroy(gameObject); // Destroy the arrow if it has no pierce ability
            }
            else
            {
                pierceLevel--; // Decrease pierce level
            }
        }
    }
}
