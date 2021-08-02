using GameClient.Scripts.Shoot.Calculators.ShootResults;
using GameClient.Scripts.Shoot.Types;
using UnityEngine;

namespace GameClient.Scripts.Shoot.Weapons
{
    public abstract class Weapon
    {
        public float Damage { get; set; }
        public IWeaponType Type { get; set; }

        public Weapon(IWeaponType type)
        {
            Type = type;
        }
        
        public abstract ShootResult Shoot(Transform direction, int shootPlayerId);
    }
}