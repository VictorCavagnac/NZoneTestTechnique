using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectilePool", menuName = "Pool/Projectile Pool")]
public class ProjectilePoolSO : ComponentPoolSO<Projectile>
{
	[SerializeField] private ProjectileFactorySO _factory;

	public override IFactory<Projectile> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as ProjectileFactorySO;
		}
	}
}