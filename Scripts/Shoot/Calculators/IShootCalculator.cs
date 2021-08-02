using GameClient.Scripts.Shoot.Calculators.ShootResults;
using GameClient.Scripts.Shoot.Types;
using GameClient.Scripts.Shoot.Weapons;
using UnityEngine;

namespace GameClient.Scripts.Shoot.Calculators
{
    public interface IShootCalculator
    {
        ShootResult Shoot(Weapon weapon, Vector3 direction);
    }
}