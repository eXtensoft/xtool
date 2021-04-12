using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class Transition
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("Display")]
        public string Display { get; set; }

        [XmlAttribute("origin")]
        public string OriginState { get; set; }

        [XmlAttribute("destination")]
        public string DestinationState { get; set; }

        [XmlAttribute("order")]
        public int SortOrder { get; set; }

        [XmlIgnore]
        public Func<bool> CanTransition { get; set; }

        [XmlElement]
        public List<IEndpointAction> EndpointActions { get; set; }

        internal void Execute(StateMachine stateMachine)
        {
            State origin = stateMachine.GetState(OriginState);
            if (origin.EndpointActions != null)
            {
                var originactions = origin.EndpointActions.Where(x => x.Endpoint == EndpointOption.Departure);
                if (originactions != null)
                {
                    foreach (var item in originactions)
                    {
                        item.Execute();
                    }
                }
            }
            if (EndpointActions != null)
            {
                var departureactions = EndpointActions.Where(x => x.Endpoint == EndpointOption.Departure);
                if (departureactions != null)
                {
                    foreach (var item in departureactions)
                    {
                        item.Execute();
                    }
                }
            }
            State destination = stateMachine.GetState(DestinationState);
            if (destination.EndpointActions != null)
            {
                var destinationactions = destination.EndpointActions.Where(x => x.Endpoint == EndpointOption.Arrival);
                if (destinationactions != null)
                {
                    foreach (var item in destinationactions)
                    {
                        item.Execute();
                    }
                }
            }
            if (EndpointActions != null)
            {
                var arrivalactions = EndpointActions.Where(x => x.Endpoint == EndpointOption.Arrival);
                if (arrivalactions != null)
                {
                    foreach (var item in arrivalactions)
                    {
                        item.Execute();
                    }
                }
            }
            stateMachine.CurrentState = DestinationState;
        }
    }
}

