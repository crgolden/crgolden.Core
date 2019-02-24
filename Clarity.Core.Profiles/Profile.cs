namespace Clarity.Core
{
    public abstract class Profile : AutoMapper.Profile
    {
        protected Profile()
        {
            CreateMap<Entity, Model>().IncludeAllDerived();
        }
    }
}
