namespace kScripts
{
    public class WayPoint : Portal
    {
        public WayPoint()
        {
        }

        public WayPoint(string _name, Vector3i _coords) : base(_name, _coords)
        {
        }
        public override void Teleport(EntityPlayer _entityPlayer, YRestraint _yRestraint = YRestraint.OnGround)
        {
            base.Teleport(_entityPlayer, _yRestraint);
        }
    }
}