using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyFactory", menuName = "Factory/Enemy Factory")]
public class EnemyFactorySO : FactorySO<Enemy>
{
	public Enemy prefab = default;

	public override Enemy Create()
	{
		return Instantiate(prefab);
	}
}