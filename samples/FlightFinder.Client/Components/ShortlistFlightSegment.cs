﻿// <copyright file="ShortlistFlightSegment.cs" company=".NET Foundation">
// Copyright (c) .NET Foundation. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// these files except in compliance with the License. You may obtain a copy of the
// License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the
// specific language governing permissions and limitations under the License.
// </copyright>

namespace FlightFinder.Client.Components
{
    using FlightFinder.Shared;
    using Microsoft.AspNetCore.Components;

    /// <summary>
    /// A component for a Flight Segment displayed on the shortlist.
    /// </summary>
    public partial class ShortlistFlightSegment
    {
        /// <summary>
        /// Gets or sets the flight segment to display.
        /// </summary>
        [Parameter]
        public FlightSegment Flight { get; set; }
    }
}
