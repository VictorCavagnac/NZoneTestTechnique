using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileFactory", menuName = "Factory/Projectile Factory")]
public class ProjectileFactorySO : FactorySO<Projectile>
{
	public Projectile prefab = default;

	public override Projectile Create()
	{
		return Instantiate(prefab);
	}
}