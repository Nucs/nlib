// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.EntityFramework.Model
{
    public class StorageFunctionParameter
    {
        /// <summary>The mapping.</summary>
        public ConceptualFunctionParameter Mapping;

        /// <summary>The mode.</summary>
        public string Mode;

        /// <summary>The name.</summary>
        public string Name;

        /// <summary>The parent function.</summary>
        public StorageFunction ParentFunction;

        /// <summary>The type.</summary>
        public string Type;
    }
}