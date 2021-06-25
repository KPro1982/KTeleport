using System;

namespace kScripts
{
    public class HomePoint : Portal
    {
        protected static DateTime _timeLastCreated;
        protected static bool isFirstTime;


        public HomePoint() : base()
        {
        }
        public HomePoint(string name, Vector3i getBlockPosition) : base(name, getBlockPosition)
        {
            double _minDuration = 0;
            double _duration = 0;

            
                
            if (KHelper.GetXmlProperty("KTeleport", "SaveHomeDelaySeconds") != "")
            {
                _minDuration = int.Parse(KHelper.GetXmlProperty("KTeleport", "SaveHomeDelaySeconds"));
            }

            if (!isFirstTime)
            {
                _duration = (double) DateTime.Now.Subtract(_timeLastCreated).TotalSeconds;
            }
      
            if (isFirstTime || _duration >= _minDuration)
            {
                IsValid = true;
                isFirstTime = false;
                _timeLastCreated = DateTime.Now;
            }
            else
            {
                KHelper.EasyLog($"Too soon. You can reset home in {(int)(_minDuration - _duration)/60} minutes.");
                IsValid = false;
            }
            
            
        }



        public override void Teleport(EntityPlayer _entityPlayer, YRestraint _yRestraint = YRestraint.OnGround)
        {
            base.Teleport(_entityPlayer, _yRestraint);
        }
    }
}