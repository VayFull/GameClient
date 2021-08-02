using GameClient.Scripts.Shoot.Calculators.ShootResults;
using GameClient.Scripts.Shoot.Types;
using UnityEngine;

namespace GameClient.Scripts.Shoot.Weapons
{
    public class KnifeWeapon : Weapon
    {
        public KnifeWeapon(IWeaponType type) : base(type)
        {
        }

        public override ShootResult Shoot(Transform direction, int shootPlayerId)
        {
            throw new System.NotImplementedException();
        }
    }
}