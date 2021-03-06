﻿// <copyright file="WeavingContext.cs" company="Jan-Willem Spuij">
// Copyright 2019 Jan-Willem Spuij
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace Cortex.Net.Fody
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Cortex.Net.Fody.Properties;
    using global::Fody;
    using Mono.Cecil;

    /// <summary>
    /// Context class for Weaving.
    /// </summary>
    public class WeavingContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WeavingContext"/> class.
        /// </summary>
        /// <param name="moduleWeaver">Moduleweaver to use.</param>
        public WeavingContext(ModuleWeaver moduleWeaver)
        {
            this.CortexNetISharedState = TryResolveFromReference(moduleWeaver, "Cortex.Net.ISharedState", "Cortex.Net");
            this.CortexNetSharedState = TryResolveFromReference(moduleWeaver, "Cortex.Net.SharedState", "Cortex.Net");
            this.CortexNetApiActionAttribute = TryResolveFromReference(moduleWeaver, "Cortex.Net.Api.ActionAttribute", "Cortex.Net");
            this.CortexNetIReactiveObject = TryResolveFromReference(moduleWeaver, "Cortex.Net.IReactiveObject", "Cortex.Net");
            this.CortexNetApiActionExtensions = TryResolveFromReference(moduleWeaver, "Cortex.Net.Api.ActionExtensions", "Cortex.Net");
            this.CortexNetApiComputedAttribute = TryResolveFromReference(moduleWeaver, "Cortex.Net.Api.ComputedAttribute", "Cortex.Net");
            this.CortexNetTypesDeepEnhancer = TryResolveFromReference(moduleWeaver, "Cortex.Net.Types.DeepEnhancer", "Cortex.Net");
            this.CortexNetTypesObservableObject = TryResolveFromReference(moduleWeaver, "Cortex.Net.Types.ObservableObject", "Cortex.Net");
            this.CortexNetComputedValueOptions = TryResolveFromReference(moduleWeaver, "Cortex.Net.ComputedValueOptions`1", "Cortex.Net");
            this.CortexNetTypesObservableCollection = TryResolveFromReference(moduleWeaver, "Cortex.Net.Types.ObservableCollection`1", "Cortex.Net");
            this.CortexNetTypesObservableSet = TryResolveFromReference(moduleWeaver, "Cortex.Net.Types.ObservableSet`1", "Cortex.Net");
            this.CortexNetTypesObservableDictionary = TryResolveFromReference(moduleWeaver, "Cortex.Net.Types.ObservableDictionary`2", "Cortex.Net");
            this.CortexNetApiObservableAttribute = TryResolveFromReference(moduleWeaver, "Cortex.Net.Api.ObservableAttribute", "Cortex.Net");
            this.CortexNetCoreActionExtensions = TryResolveFromReference(moduleWeaver, "Cortex.Net.Core.ActionExtensions", "Cortex.Net");
            this.CortexNetCoreActionRunInfo = TryResolveFromReference(moduleWeaver, "Cortex.Net.Core.ActionRunInfo", "Cortex.Net");
            this.MicrosoftAspNetCoreComponentsInjectAttribute = TryResolveFromReference(moduleWeaver, "Microsoft.AspNetCore.Components.InjectAttribute", "Microsoft.AspNetCore.Components", false);
            this.SystemRuntimeCompilerServicesCompilerGeneratedAttribute = TryResolveFromScannedAssemblies(moduleWeaver, "System.Runtime.CompilerServices.CompilerGeneratedAttribute");
            this.SystemDiagnosticsDebuggerBrowsableAttribute = TryResolveFromScannedAssemblies(moduleWeaver, "System.Diagnostics.DebuggerBrowsableAttribute");
            this.SystemObject = TryResolveFromScannedAssemblies(moduleWeaver, "System.Object");
            this.SystemAction = Enumerable.Range(0, 16).Select(x => TryResolveFromScannedAssemblies(moduleWeaver, x == 0 ? "System.Action" : $"System.Action`{x}")).ToList().AsReadOnly();
            this.SystemFunc = Enumerable.Range(0, 16).Select(x => TryResolveFromScannedAssemblies(moduleWeaver, $"System.Func`{x + 1}")).ToList().AsReadOnly();
            this.SystemThreadingTasksTask = TryResolveFromScannedAssemblies(moduleWeaver, "System.Threading.Tasks.Task");
            this.SystemRuntimeCompilerServicesAsyncStateMachineAttribute = TryResolveFromScannedAssemblies(moduleWeaver, "System.Runtime.CompilerServices.AsyncStateMachineAttribute");
        }

        /// <summary>
        /// Gets type reference to Cortex.Net.ISharedState.
        /// </summary>
        public TypeReference CortexNetISharedState { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.ISharedState.
        /// </summary>
        public TypeReference CortexNetSharedState { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Api.ActionAttribute.
        /// </summary>
        public TypeReference CortexNetApiActionAttribute { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.IReactiveObject.
        /// </summary>
        public TypeReference CortexNetIReactiveObject { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Api.ActionExtensions.
        /// </summary>
        public TypeReference CortexNetApiActionExtensions { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Api.ComputedAttribute.
        /// </summary>
        public TypeReference CortexNetApiComputedAttribute { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Types.DeepEnhancer.
        /// </summary>
        public TypeReference CortexNetTypesDeepEnhancer { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Types.ObservableObject.
        /// </summary>
        public TypeReference CortexNetTypesObservableObject { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.ComputedValueOptions`1.
        /// </summary>
        public TypeReference CortexNetComputedValueOptions { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Types.ObservableCollection`1.
        /// </summary>
        public TypeReference CortexNetTypesObservableCollection { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Types.ObservableSet`1.
        /// </summary>
        public TypeReference CortexNetTypesObservableSet { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Types.ObservableDictionary`2.
        /// </summary>
        public TypeReference CortexNetTypesObservableDictionary { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Api.ObservableAttribute.
        /// </summary>
        public TypeReference CortexNetApiObservableAttribute { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Core.ActionExtensions.
        /// </summary>
        public TypeReference CortexNetCoreActionExtensions { get; }

        /// <summary>
        /// Gets type reference to Cortex.Net.Core.ActionRunInfo.
        /// </summary>
        public TypeReference CortexNetCoreActionRunInfo { get; }

        /// <summary>
        /// Gets type reference to Microsoft.AspNetCore.Components.InjectAttribute.
        /// </summary>
        public TypeReference MicrosoftAspNetCoreComponentsInjectAttribute { get; }

        /// <summary>
        /// Gets type reference to System.Runtime.CompilerServices.CompilerGeneratedAttribute.
        /// </summary>
        public TypeReference SystemRuntimeCompilerServicesCompilerGeneratedAttribute { get; }

        /// <summary>
        /// Gets type reference to System.Diagnostics.DebuggerBrowsableAttribute.
        /// </summary>
        public TypeReference SystemDiagnosticsDebuggerBrowsableAttribute { get; }

        /// <summary>
        /// Gets type reference to System.Object.
        /// </summary>
        public TypeReference SystemObject { get; }

        /// <summary>
        /// Gets action type references.
        /// </summary>
        public IReadOnlyList<TypeReference> SystemAction { get; }

        /// <summary>
        /// Gets func type references.
        /// </summary>
        public IReadOnlyList<TypeReference> SystemFunc { get; }

        /// <summary>
        /// Gets a reference to System.Threading.Tasks.Task.
        /// </summary>
        public TypeReference SystemThreadingTasksTask { get; }

        /// <summary>
        /// Gets a reference to System.Runtime.CompilerServices.AsyncStateMachineAttribute.
        /// </summary>
        public TypeReference SystemRuntimeCompilerServicesAsyncStateMachineAttribute { get; }

        /// <summary>
        /// Tries to resolve a type from a preference.
        /// </summary>
        /// <param name="moduleWeaver">The module weaver to use.</param>
        /// <param name="fullName">The fullname of the type.</param>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="throwOnFailure">Throw an exception on failure.</param>
        /// <returns>A type reference.</returns>
        protected static TypeReference TryResolveFromReference(ModuleWeaver moduleWeaver, string fullName, string assemblyName, bool throwOnFailure = true)
        {
            if (moduleWeaver is null)
            {
                throw new System.ArgumentNullException(nameof(moduleWeaver));
            }

            try
            {
                var assembly = moduleWeaver.ModuleDefinition.AssemblyResolver.Resolve(moduleWeaver.ModuleDefinition.AssemblyReferences.FirstOrDefault(asm => asm.Name == assemblyName));
                return moduleWeaver.ModuleDefinition.ImportReference(assembly.MainModule.GetType(fullName));
            }
            catch
            {
                if (throwOnFailure)
                {
                    moduleWeaver.WriteWarning(string.Format(CultureInfo.CurrentCulture, Resources.AssemblyOrTypeNotFoundReferences, fullName, assemblyName));
                    throw;
                }

                return null;
            }
        }

        /// <summary>
        /// Tries to resolve a type from a preference.
        /// </summary>
        /// <param name="moduleWeaver">The module weaver to use.</param>
        /// <param name="fullName">The fullname of the type.</param>
        /// <returns>A type reference.</returns>
        protected static TypeReference TryResolveFromScannedAssemblies(ModuleWeaver moduleWeaver, string fullName)
        {
            if (moduleWeaver is null)
            {
                throw new System.ArgumentNullException(nameof(moduleWeaver));
            }

            try
            {
                var type = moduleWeaver.FindTypeDefinition(fullName);
                var result = moduleWeaver.ModuleDefinition.ImportReference(type);
                return result;
            }
            catch
            {
                moduleWeaver.WriteWarning(string.Format(CultureInfo.CurrentCulture, Resources.AssemblyOrTypeNotFoundScan, fullName));
                throw;
            }
        }
    }
}
