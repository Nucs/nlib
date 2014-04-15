// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

namespace Z.EntityFramework.Model
{
    public class ConceptualModel
    {
        /// <summary>The entities.</summary>
        public List<ConceptualEntity> Entities;

        /// <summary>The functions.</summary>
        public List<ConceptualFunction> Functions;

        /// <summary>The name.</summary>
        public string Name;
    }
}