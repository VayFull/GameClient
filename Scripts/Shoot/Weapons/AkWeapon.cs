using GameClient.Scripts.Shoot.Calculators.ShootResults;
using GameClient.Scripts.Shoot.Types;
using UnityEngine;

namespace GameClient.Scripts.Shoot.Weapons
{
    public class AkWeapon : Weapon
    {
        public AkWeapon(IWeaponType type) : base(type)
        {
        }

        public override ShootResult Shoot(Transform direction, int shootPlayerId)
        {
            var ray = new Ray(direction.position, direction.forward);
            Physics.Raycast(ray, out var raycastHit, 100f);

            if (raycastHit.collider == null || !raycastHit.collider.CompareTag("OtherPlayer"))
            {
                if (raycastHit.transform != null)
                    return new FaultShootResult(this, shootPlayerId, raycastHit.point);
                
                return new FaultShootResult(this, shootPlayerId);
            }


            var otherPlayerId = raycastHit.collider.gameObject.GetComponentInParent<OtherPlayer>().Id;
            return new SuccessShootResult(this, shootPlayerId, otherPlayerId);
        }
    }
}