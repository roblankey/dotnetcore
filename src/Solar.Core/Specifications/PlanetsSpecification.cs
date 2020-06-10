using Ardalis.Specification;
using Solar.Core.Entities;

namespace Solar.Core.Specifications
{
    public class PlanetsSpecification : BaseSpecification<Planet>
    {
        public PlanetsSpecification(string name, string universe)
            : base(p => (string.IsNullOrEmpty(name) || p.Name == name) 
                        && (string.IsNullOrEmpty(universe) || p.Universe == universe))
        {
            
        }
    }
}
