using System.Net;
using Ardalis.Specification;
using Solar.Core.Entities;

namespace Solar.Core.Specifications
{
    public class PlanetsSpecification : BaseSpecification<Planet>
    {
        public PlanetsSpecification(string universe, string name = null)
            : base(p => 
                p.Universe == WebUtility.UrlDecode(universe) && 
                (string.IsNullOrEmpty(name) || p.Name == WebUtility.UrlDecode(name ?? string.Empty)))
        {
        }
    }
}
