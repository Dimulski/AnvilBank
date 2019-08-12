using AutoMapper;

namespace AnvilBank.Common.AutoMapping.Contracts
{
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile mapper);
    }
}
