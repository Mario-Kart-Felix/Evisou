using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Evisou.Core.Config
{
    [Serializable]
    public class AdminMenuConfig : ConfigFileBase
    {
        public AdminMenuConfig()
        {
        }

        public AdminMenuGroup[] AdminMenuGroups { get; set; }
    }

    [Serializable]
    public class AdminMenuGroup
    {
        public List<AdminMenu> AdminMenuArray { get; set; }

        [XmlAttribute("id")]
        //[JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [XmlAttribute("name")]
        //[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [XmlAttribute("url")]
        //[JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [XmlAttribute("icon")]
        //[JsonProperty(PropertyName = "Icon")]
        public string Icon { get; set; }

        [XmlAttribute("permission")]
        //[JsonProperty(PropertyName = "permission")]
        public string Permission { get; set; }

        [XmlAttribute("info")]
        //[JsonProperty(PropertyName = "info")]
        public string Info { get; set; }
    }

    [Serializable]
    public class AdminMenu
    {
        [XmlAttribute("id")]
        //[JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        //[JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [XmlAttribute("url")]
        //[JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [XmlAttribute("info")]
        //[JsonProperty(PropertyName = "info")]
        public string Info { get; set; }

        [XmlAttribute("icon")]
        //[JsonProperty(PropertyName = "icon")]
        public string Icon { get; set; }

        [XmlAttribute("permission")]
        //[JsonProperty(PropertyName = "permission")]
        public string Permission { get; set; }
    }
}
