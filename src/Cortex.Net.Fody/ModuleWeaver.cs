﻿// <copyright file="ModuleWeaver.cs" company="Jan-Willem Spuij">
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
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Cortex.Net.Fody.Properties;
    using global::Fody;
    using Mono.Cecil;

    /// <summary>
    /// Orchestrates weaving of classes with Cortex.Net Observables, Actions and Reactions.
    /// </summary>
    public class ModuleWeaver : BaseModuleWeaver
    {
        /// <summary>
        /// Executes the <see cref="ModuleWeaver"/>.
        /// </summary>
        public override void Execute()
        {
            WeavingContext weavingContext = null;
            try
            {
                weavingContext =
                    this.ModuleDefinition.AssemblyReferences.Any(x => x.Name == "Cortex.Net.Blazor") ?
                    new BlazorWeavingContext(this) :
                    new WeavingContext(this);
#pragma warning disable CA1031 // Do not catch general exception types
            }
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.WriteWarning(Resources.CannotLoadTypes);
                return;
            }

            var reactiveObjectInterfaceWeaver = new ReactiveObjectInterfaceWeaver(this, weavingContext);
            var enumerableWeaver = new EnumerableInterfaceWeaver(this, reactiveObjectInterfaceWeaver, weavingContext);

            var actionWeaver = new ActionWeaver(this, reactiveObjectInterfaceWeaver, weavingContext);

            var observableWeaver = new ObservableWeaver(this, enumerableWeaver, reactiveObjectInterfaceWeaver, weavingContext);
            var computedWeaver = new ComputedWeaver(this, reactiveObjectInterfaceWeaver, weavingContext);

            actionWeaver.Execute();
            observableWeaver.Execute();
            computedWeaver.Execute();

            if (this.ModuleDefinition.AssemblyReferences.Any(x => x.Name == "Cortex.Net.Blazor"))
            {
                var blazorObserverWeaver = new BlazorObserverWeaver(this, reactiveObjectInterfaceWeaver, (BlazorWeavingContext)weavingContext);
                blazorObserverWeaver.Execute();
            }

            reactiveObjectInterfaceWeaver.Execute();
       }

        /// <summary>
        /// Return a list of assembly names for scanning. Used as a list for Fody.ModuleWeaver.FindType.
        /// </summary>
        /// <returns>All types in the references assembly.</returns>
        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "mscorlib";
            yield return "System";
            yield return "System.Runtime";
            yield return "System.Core";
            yield return "netstandard";
            yield return "System.Collections";
            yield return "System.ObjectModel";
            yield return "System.Threading";
            yield return "FSharp.Core";
        }
    }
}
