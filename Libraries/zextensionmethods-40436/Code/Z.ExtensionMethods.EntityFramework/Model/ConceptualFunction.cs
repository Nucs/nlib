// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

namespace Z.EntityFramework.Model
{
    public class ConceptualFunction
    {
        /// <summary>The mapping.</summary>
        public StorageFunction Mapping;

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>Options for controlling the operation.</summary>
        public List<ConceptualFunctionParameter> Parameters;

        /// <summary>Type of the return.</summary>
        public string ReturnType;
    }
}