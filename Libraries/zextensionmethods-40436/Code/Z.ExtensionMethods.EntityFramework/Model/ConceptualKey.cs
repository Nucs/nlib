// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

namespace Z.EntityFramework.Model
{
    public class ConceptualKey
    {
        /// <summary>The parent entity.</summary>
        public ConceptualEntity ParentEntity;

        /// <summary>The properties.</summary>
        public List<ConceptualProperty> Properties;
    }
}