﻿using YamlDotNet.Serialization;

namespace TextRpgMaker.Models
{
    public class Element
    {
        [YamlMember(Alias = "id")]
        public virtual string Id { get; set; }
    }
}