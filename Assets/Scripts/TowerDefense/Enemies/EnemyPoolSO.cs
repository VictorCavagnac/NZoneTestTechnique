using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyPool", menuName = "Pool/Enemy Pool")]
public class EnemyPoolSO : ComponentPoolSO<Enemy>
{
	[SerializeField] private EnemyFactorySO _factory;

	public override IFactory<Enemy> Factory
	{
		get
		{
			return _factory;
		}
		set
		{
			_factory = value as EnemyFactorySO;
		}
	}
}