namespace GameClient.Scripts.Shoot.Calculators.ShootResults
{
    public class ShootResultHandler
    {
        private Communicator Communicator { get; set; }

        public ShootResultHandler(Communicator communicator)
        {
            Communicator = communicator;
        }

        public void Handle(ShootResult shootResult)
        {
            if (shootResult is SuccessShootResult successShootResult)
                HandleSuccessResult(successShootResult);
            else
                HandleFaultResult(shootResult as FaultShootResult);
        }

        private void HandleSuccessResult(SuccessShootResult successShootResult)
        {
            Communicator.RegisterSuccessShot(successShootResult);
        }

        private void HandleFaultResult(FaultShootResult faultShootResult)
        {
            if (faultShootResult.HitPoint == null)
            {
                Communicator.RegisterFaultShot(faultShootResult);
            }
            else
            {
                Communicator.RegisterFaultShotWithMarker(faultShootResult);
            }  
        }
    }
}