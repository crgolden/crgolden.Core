namespace Clarity.Core.Profiles
{
    public abstract class Profile : AutoMapper.Profile
    {
        protected Profile()
        {
            CreateMap<Entity, Model>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.Updated, opt => opt.MapFrom(src => src.Updated));
        }
    }
}
