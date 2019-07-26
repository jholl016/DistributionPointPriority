using System;
using System.Collections.ObjectModel;

namespace Distribution_Point_Priority
{
    public class DistributionPoint
    {
        string server;
        int priority;
        int newPriority;
        bool shared;

        public DistributionPoint(string server, int priority, int newPriority, bool shared)
        {
            this.server = server;
            this.priority = priority;
            this.newPriority = newPriority;
            this.shared = shared;
        }

        public string Server
        {
            get { return this.server; }
        }

        public int Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }

        public int NewPriority
        {
            get { return this.newPriority; }
            set { this.newPriority = value; }
        }

        public bool Shared
        {
            get { return this.shared; }
            set { this.shared = value; }
        }
    }
}
