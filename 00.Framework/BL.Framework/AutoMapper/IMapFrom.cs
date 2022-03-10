using AutoMapper;

namespace BL.Framework.Mapper
{
	public interface IMapFrom<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
