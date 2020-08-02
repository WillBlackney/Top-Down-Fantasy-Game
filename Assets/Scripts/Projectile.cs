using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Properties")]
    public Vector3 destinationPos;
    public Vector3 direction;
    public int damageAmount;
    public float moveSpeed;
    private bool readyToMove;
    public bool alreadyMadeCollision;
    public Player playerOwner;
    public Enemy enemyOwner;

    public void InitializeSetup(Player _playerOwner, Vector3 _startPos, Vector3 _destinationPos, int _damageAmount)
    {
        damageAmount = _damageAmount;
        transform.position = _startPos;
        destinationPos = new Vector3(_destinationPos.x, _destinationPos.y, 1);
        direction = (destinationPos - _startPos).normalized;
        playerOwner = _playerOwner;
        FaceDestination();
        readyToMove = true;
    }
    public void InitializeSetup(Enemy _enemyOwner, Vector3 _startPos, Vector3 _destinationPos, int _damageAmount)
    {
        damageAmount = _damageAmount;
        transform.position = _startPos;
        destinationPos = new Vector3(_destinationPos.x, _destinationPos.y, 1);
        direction = (destinationPos - _startPos).normalized;
        enemyOwner = _enemyOwner;
        FaceDestination();
        readyToMove = true;
    }

    void Update()
    {
        if (readyToMove)
        {
            Move();
        }
    }
    public void Move()
    {
        // Make sure Z position always remains at 0 (otherwise it will fuck up sorting order/visibility)
        Vector3 dirPos = transform.position += direction * moveSpeed * Time.deltaTime;
        Vector3 finalPos = new Vector3(dirPos.x, dirPos.y, 1);
        transform.position = finalPos;
    }
    public void FaceDestination()
    {
        Vector2 direction = destinationPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10000f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Did we hit an enemy or player?
        Player player = other.GetComponent<Player>();
        Enemy enemy = other.GetComponent<Enemy>();

        Debug.Log(gameObject.name + " collider has collision event with " + other.gameObject.name);

        if (enemy && playerOwner != null && alreadyMadeCollision == false)
        {
            Debug.Log("Player made projectile collided with enemy: " + enemy.gameObject.name);
            alreadyMadeCollision = true;
            CombatLogic.Instance.HandleDamage(enemy, damageAmount);
            Destroy(gameObject);
        }
        else if(player && enemyOwner != null && alreadyMadeCollision == false)
        {
            Debug.Log("Enemy made projectile collided with player");
            alreadyMadeCollision = true;
            CombatLogic.Instance.HandleDamage(player, damageAmount);
            Destroy(gameObject);
        }
    }
}
