using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Settings;
using System;

namespace BL.API.UnitTests.Builders
{
    public class SeasonBuilder
    {
        private Guid _id;
        private string _title;
        private bool _onGoing;
        private DateTime _started;
        private DateTime _finished;
        public Region _region;

        public SeasonBuilder WithId(Guid? id)
        {
            _id = id ?? Guid.NewGuid();
            return this;
        }

        public SeasonBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public SeasonBuilder WithOnGoing(bool onGoing) 
        { 
            _onGoing = onGoing; 
            return this; 
        }

        public SeasonBuilder WithStarted(DateTime started)
        {
            this._started = started;
            return this;
        }

        public SeasonBuilder WithFinished(DateTime finished)
        {
            this._finished = finished; 
            return this;
        }

        public SeasonBuilder WithRegion(Region region) 
        { 
            _region = region;
            return this; 
        }

        public Season Build()
        {
            return new Season
            {
                Id = _id,
                Title = _title,
                OnGoing = _onGoing,
                Started = _started,
                Finished = _finished,
                Region = _region
            };
        }
    }
}
