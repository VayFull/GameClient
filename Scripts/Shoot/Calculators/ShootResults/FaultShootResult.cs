using GameClient.Scripts.Shoot.Weapons;
using UnityEngine;

namespace GameClient.Scripts.Shoot.Calculators.ShootResults
{
    public class FaultShootResult : ShootResult
    {
        public Vector3? HitPoint { get; }

        public FaultShootResult(Weapon weapon, int shootPlayerId) : base(weapon, shootPlayerId)
        {
            HitPoint = null;
        }
        
        public FaultShootResult(Weapon weapon, int shootPlayerId, Vector3 hitPoint) : base(weapon, shootPlayerId)
        {
            HitPoint = hitPoint;
        }
    }
}