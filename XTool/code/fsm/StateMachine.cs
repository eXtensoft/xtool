using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace XTool
{
    [Serializable]
    public class StateMachine
    {
        [XmlAttribute("beginState")]
        public string BeginState { get; set; }

        [XmlAttribute("endState")]
        public string EndState { get; set; }

        [XmlElement]
        public string CurrentState { get; set; }

        public Transition EndingTransition
        {
            get { return GetTransitionToEndState(); }
        }

        #region States (List<State>)

        private List<State> _States = new List<State>();

        /// <summary>
        /// Gets or sets the List<State> value for States
        /// </summary>
        /// <value> The List<State> value.</value>
        [XmlElement]
        public List<State> States
        {
            get { return _States; }
            set
            {
                if (_States != value)
                {
                    _States = value;
                }
            }
        }

        #endregion

        #region Transitions (List<Transition>)

        private List<Transition> _Transitions = new List<Transition>();

        /// <summary>
        /// Gets or sets the List<Transition> value for Transitions
        /// </summary>
        /// <value> The List<Transition> value.</value>
        [XmlElement]
        public List<Transition> Transitions
        {
            get { return _Transitions; }
            set
            {
                if (_Transitions != value)
                {
                    _Transitions = value;
                }
            }
        }

        #endregion

        #region constructors

        public StateMachine() { }

        public StateMachine(System.Xml.Linq.XDocument document)
        {
            BeginState = document.Root.Attribute("beginState").Value;
            EndState = document.Root.Attribute("endState").Value;
            States = (from x in document.Descendants("State")
                      select new State() { Name = x.Attribute("name").Value, Display = x.Attribute("display").Value }).ToList();


            Transitions = (from x in document.Descendants("Transition")
                           select new Transition()
                           {
                               Name = x.Attribute("name").Value,
                               OriginState = x.Attribute("origin").Value,
                               DestinationState = x.Attribute("destination").Value,
                               SortOrder = Convert.ToInt32(x.Attribute("order").Value)
                           }).ToList();
        }

        #endregion

        #region instance methods

        public Transition GetTransitionToEndState()
        {
            State current = GetCurrentState();
            Transition end = Transitions.FirstOrDefault(x => x.OriginState.Equals(current.Name) & x.DestinationState.Equals(EndState, StringComparison.OrdinalIgnoreCase));
            return end;
        }

        public State GetCurrentState()
        {
            string statename = (!String.IsNullOrEmpty(CurrentState)) ? CurrentState : BeginState;
            if (String.IsNullOrEmpty(statename))
            {
                throw new ArgumentOutOfRangeException("Current State cannot be null");
            }
            return GetState(statename);
        }

        public State GetState(string stateName)
        {
            return States.FirstOrDefault(x => x.Name.Equals(stateName, StringComparison.OrdinalIgnoreCase));
        }

        public List<Transition> GetTransitions()
        {
            State current = GetCurrentState();
            return Transitions.Where(x => x.OriginState.Equals(current.Name) & !x.DestinationState.Equals(EndState, StringComparison.OrdinalIgnoreCase)).OrderBy(y => y.SortOrder).ToList();
        }

        public void ExecuteAvailableTransition(string transitionName)
        {
            List<Transition> available = GetTransitions();
            if (available != null && available.Count > 0)
            {
                Transition transition = available.FirstOrDefault(x => x.Name.Equals(transitionName, StringComparison.OrdinalIgnoreCase));
                if (transition != null)
                {
                    transition.Execute(this);
                }                
            }
        }

        public void ExecuteTransition(string transitionName)
        {

            string currentState = this.CurrentState;
            Transition found = null;
            if (!String.IsNullOrEmpty(CurrentState))
            {
                found = Transitions.FirstOrDefault(x => x.Name.Equals(transitionName, StringComparison.OrdinalIgnoreCase) && x.OriginState.Equals(CurrentState, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                found = Transitions.FirstOrDefault(x => x.Name.Equals(transitionName, StringComparison.OrdinalIgnoreCase));
            }

            if (found != null)
            {
                found.Execute(this);
            }
        }

        public bool CanExecuteTransition(string transitionName)
        {
            Transition transition = Transitions.FirstOrDefault(x => x.Name.Equals(transitionName, StringComparison.OrdinalIgnoreCase));
            if (transition != null)
            {
                return (transition.CanTransition != null) ? transition.CanTransition() : true;
            }
            else
            {
                return true;
            }

        }

        #endregion
    }
}
