namespace kScripts
{
    public class SimplePoint : Portal
    {
        public SimplePoint() : base()
        {
        }
        public SimplePoint(string name, Vector3i getBlockPosition) : base(name, getBlockPosition)
        {
            
        }



        public override void Teleport(EntityPlayer _entityPlayer, YRestraint _yRestraint = YRestraint.OnGround)
        {
            base.Teleport(_entityPlayer, _yRestraint);
        }
    }
}