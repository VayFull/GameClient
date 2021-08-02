using GameClient.Scripts.Shoot.Types;
using GameClient.Scripts.Shoot.Weapons;

namespace GameClient.Scripts.Shoot.Calculators.ShootResults
{
    public abstract class ShootResult
    {
        public ShootResult(Weapon weapon, int shootPlayerId)
        {
            ShootPlayerId = shootPlayerId;
            Weapon = weapon;
        }
        
        public int ShootPlayerId { get; set; }
        public Weapon Weapon { get; set; }
    }
}