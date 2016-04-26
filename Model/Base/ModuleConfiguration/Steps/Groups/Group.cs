using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using FomodModel.Base.ModuleConfiguration.Enums;
using FomodModel.Base.ModuleConfiguration.Steps.Groups.Plugins;

namespace FomodModel.Base.ModuleConfiguration.Steps.Groups
{
    /// <summary>
    /// A group of plugins.
    /// </summary>
    [Serializable]
    public class Group
    {
        /// <summary>
        /// The list of plugins in the group.
        /// </summary>
        [XmlElement("plugins", Form = XmlSchemaForm.Unqualified)]
        public PluginList Plugins { get; set; }

        /// <summary>
        /// The name of the group.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// The type of the group.
        /// </summary>
        [XmlAttribute("type")]
        public GroupType Type { get; set; }
    }
}