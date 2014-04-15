// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;

namespace Z.EntityFramework.Model
{
    public class StorageFunction
    {
        /// <summary>true to aggregate.</summary>
        public bool Aggregate;

        /// <summary>true to built in.</summary>
        public bool BuiltIn;

        /// <summary>true if this object is composable.</summary>
        public bool IsComposable;

        /// <summary>The mapping.</summary>
        public ConceptualFunction Mapping;

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>true to niladic function.</summary>
        public bool NiladicFunction;

        /// <summary>The parameter type semantics.</summary>
        public string ParameterTypeSemantics;

        /// <summary>Options for controlling the operation.</summary>
        public List<StorageFunctionParameter> Parameters;

        /// <summary>The schema.</summary>
        public string Schema;
    }
}