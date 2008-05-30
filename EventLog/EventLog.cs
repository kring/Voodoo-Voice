using System;
using System.Collections.Generic;
using System.Text;

namespace FuzzLab
{
    public enum EventType
    {
        Information,
        Warning,
        Error
    }

    public struct Event
    {
        public Event(EventType type, string subsystem, string text)
        {
            this.type = type;
            this.subsystem = subsystem;
            this.text = text;
        }

        public EventType type;
        public string subsystem;
        public string text;
    }

    public class EventLog
    {
        public static EventLog Instance
        {
            get { return s_eventLog; }
        }
        private static EventLog s_eventLog = new EventLog();

        public delegate void NewEventHandler(Event newEvent);
        public event NewEventHandler NewEvent;

        public EventLog()
        {
        }

        public void AddEvent(EventType type, string subsystem, string text)
        {
            AddEvent(new Event(type, subsystem, text));
        }

        public void AddEvent(Event eventToAdd)
        {
            RaiseNewEvent(eventToAdd);
        }

        protected void RaiseNewEvent(Event newEvent)
        {
            NewEventHandler temp = NewEvent;
            if (temp != null)
            {
                temp(newEvent);
            }
        }
    }
}
