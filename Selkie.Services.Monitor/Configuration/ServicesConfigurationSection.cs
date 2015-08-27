using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Selkie.Services.Monitor.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ServicesConfigurationSection
        : ConfigurationSection,
          IServicesConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ServicesCollection Instances
        {
            get
            {
                return ( ServicesCollection ) this [ "" ];
            }
            set
            {
                this [ "" ] = value;
            }
        }
    }

    public interface IServicesConfigurationSection
    {
        [NotNull]
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        ServicesCollection Instances { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ServicesCollection
        : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ( ( ServiceElement ) element ).ServiceName;
        }
    }

    [ExcludeFromCodeCoverage]
    public class ServiceElement : ConfigurationElement
    {
        public static readonly ServiceElement Unknown = new ServiceElement(true);
        private readonly bool m_IsUnknown;

        public ServiceElement()
        {
        }

        private ServiceElement(bool isUnknown)
        {
            m_IsUnknown = isUnknown;
        }

        public bool IsUnknown
        {
            get
            {
                return m_IsUnknown;
            }
        }

        [NotNull]
        [ConfigurationProperty("serviceName", IsRequired = true)]
        public string ServiceName
        {
            get
            {
                return ( string ) base [ "serviceName" ];
            }
            set
            {
                base [ "serviceName" ] = value;
            }
        }

        [NotNull]
        [ConfigurationProperty("fileName", IsRequired = true)]
        public string FileName
        {
            get
            {
                return ( string ) base [ "fileName" ];
            }
            set
            {
                base [ "fileName" ] = value;
            }
        }

        [NotNull]
        [ConfigurationProperty("folderName", IsRequired = true)]
        public string FolderName
        {
            get
            {
                return ( string ) base [ "folderName" ];
            }
            set
            {
                base [ "folderName" ] = value;
            }
        }

        [NotNull]
        [ConfigurationProperty("workingFolder", IsRequired = true)]
        public string WorkingFolder
        {
            get
            {
                return ( string ) base [ "workingFolder" ];
            }
            set
            {
                base [ "workingFolder" ] = value;
            }
        }
    }
}