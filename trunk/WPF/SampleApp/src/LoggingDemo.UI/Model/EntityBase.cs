using Mindscape.LightSpeed;

namespace LoggingDemo.UI.Model
{
    public class EntityBase : Entity<int>
    {
        public virtual bool IsNew
        {
            get { return EntityState == EntityState.New; }
        }
    }
}