using BL.API.Core.Domain.Settings;
using System;

namespace BL.API.UnitTests.Builders
{
    public class RegionBuilder
    {
        private string _shortName;
        private string _name;
        private Guid _id;

        public RegionBuilder WithShortName(string shortname)
        {
            _shortName = shortname;
            return this;
        }

        public RegionBuilder WithName(string name)
        {
            this._name = name;
            return this;
        }

        public RegionBuilder WithId(Guid? id)
        {
            _id = id ?? Guid.NewGuid();
            return this;
        }

        public Region Build()
        {
            return new Region
            {
                Id = _id,
                Name = _name,
                ShortName = _shortName
            };
        }
    }
}
