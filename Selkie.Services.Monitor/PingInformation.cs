using System;

namespace Selkie.Services.Monitor
{
    public class PingInformation : IPingInformation
    {
        private PingInformation(bool isUnknown)
        {
            m_ServiceName = "Unknown";
            m_IsUnknown = isUnknown;
        }

        public PingInformation()
        {
            m_IsUnknown = false;
        }

        public static readonly IPingInformation Unknown = new PingInformation(true);
        internal static readonly TimeSpan ExpiredAge = TimeSpan.FromSeconds(6);

        internal TimeSpan AgeOfPing
        {
            get
            {
                TimeSpan age = DateTime.Now - m_Received;

                return age;
            }
        }

        private readonly bool m_IsUnknown;
        private DateTime m_Received = DateTime.Now;
        private string m_ServiceName = string.Empty;

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        public string ServiceName
        {
            get
            {
                return m_ServiceName;
            }
            set
            {
                m_ServiceName = value;
            }
        }

        public DateTime Received
        {
            get
            {
                return m_Received;
            }
            set
            {
                m_Received = value;
            }
        }

        public bool IsRunning
        {
            get
            {
                TimeSpan age = AgeOfPing;

                bool isRunning = age < ExpiredAge;

                return isRunning;
            }
        }

        public double AgeInMilliseconds
        {
            get
            {
                double milliseconds = AgeOfPing.TotalMilliseconds;

                return milliseconds;
            }
        }
    }
}