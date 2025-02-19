using System.Collections;
using UnityEngine;

public class BulletSpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public IEnumerator SpawnBulletsFromLeftWithDelay(int bulletCount, float delay, CharacterStats stats)
    {
        Bounds bounds = boxCollider.bounds;
        
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 spawnPos = new Vector3(bounds.min.x, Random.Range(bounds.min.y, bounds.max.y), 0);
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            bullet.GetComponent<DeathBringerBullet>().Setup(Vector2.right, stats);
            yield return new WaitForSeconds(delay);
        }
    }

    public IEnumerator SpawnBulletsFromRightWithDelay(int bulletCount, float delay, CharacterStats stats)
    {
        Bounds bounds = boxCollider.bounds;
        
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 spawnPos = new Vector3(bounds.min.x, Random.Range(bounds.min.y, bounds.max.y), 0);
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            bullet.GetComponent<DeathBringerBullet>().Setup(Vector2.left, stats);
            SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
            sr.flipX = true;
            yield return new WaitForSeconds(delay);
        }
    }

}
