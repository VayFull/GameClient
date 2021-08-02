using GameClient.Scripts.Shoot.Types;
using GameClient.Scripts.Shoot.Weapons;

namespace GameClient.Scripts.Shoot.Calculators.ShootResults
{
    public class SuccessShootResult : ShootResult
    {
        public int HitPlayerId { get; set; }

        public SuccessShootResult(Weapon weapon, int shootPlayerId, int hitPlayerId) : base(weapon, shootPlayerId)
        {
            HitPlayerId = hitPlayerId;
        }
    }
}