using System.Data.Entity.Infrastructure;

namespace PRSClassesManagement
{
    class PRSContextFactory : IDbContextFactory<PRSContext>
    {
        public PRSContext Create()
        {
            return PRSContext.GetInstance();
        }
    }
}
