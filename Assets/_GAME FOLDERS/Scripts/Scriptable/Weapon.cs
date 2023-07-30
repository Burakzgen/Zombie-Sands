using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public KeyCode activationKey;
    public float reloadTime;
    public float fireRate;
    public float damage;
    public int totalBullet;
    public int remainingBullet;
    public ParticleSystem bulletDecalEffect;
    public string fireAnimation;
}
