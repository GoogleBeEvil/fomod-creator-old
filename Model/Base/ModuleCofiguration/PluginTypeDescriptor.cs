using System;
using System.Xml.Serialization;
using AspectInjector.Broker;
using FomodInfrastructure.Aspect;

namespace FomodModel.Base.ModuleCofiguration
{
    /// <summary>
    ///     Describes the type of a Plugin.
    /// </summary>
    [Aspect(typeof (AspectINotifyPropertyChanged))]
    [Serializable]
    public class PluginTypeDescriptor
    {
        [XmlElement("dependencyType", typeof (DependencyPluginType))]
        public DependencyPluginType DependencyType { get; set; }

        [XmlElement("type", typeof (PluginType))]
        public PluginType Type { get; set; }
    }
}