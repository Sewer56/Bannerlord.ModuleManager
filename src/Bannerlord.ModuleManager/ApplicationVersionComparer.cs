﻿// <auto-generated>
//   This code file has automatically been added by the "Bannerlord.ModuleManager.Source" NuGet package (https://www.nuget.org/packages/Bannerlord.ModuleManager.Source).
//   Please see https://github.com/BUTR/Bannerlord.ModuleManager for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Bannerlord.ModuleManager.Source" folder and the "ApplicationVersionComparer.cs" file don't appear in your project.
//   * The added file is immutable and can therefore not be modified by coincidence.
//   * Updating/Uninstalling the package will work flawlessly.
// </auto-generated>

#region License
// MIT License
//
// Copyright (c) Bannerlord's Unofficial Tools & Resources
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

namespace Bannerlord.ModuleManager
{
    using global::System.Collections;
    using global::System.Collections.Generic;

#nullable enable
#pragma warning disable
#if !BANNERLORDBUTRMODULEMANAGER_PUBLIC
    internal
#else
    public
# endif
        class ApplicationVersionComparer : IComparer<ApplicationVersion?>, IComparer
    {
        /// <inheritdoc/>
        public int Compare(object? x, object? y) => Compare(x as ApplicationVersion, y as ApplicationVersion);

        /// <inheritdoc/>
        public virtual int Compare(ApplicationVersion? x, ApplicationVersion? y)
        {
            if (x is null && y is null)
                return 0;

            if (x is null)
                return -1;

            if (y is null)
                return 1;

            var versionTypeComparison = x.ApplicationVersionType.CompareTo(y.ApplicationVersionType);
            if (versionTypeComparison != 0) return versionTypeComparison;

            var majorComparison = x.Major.CompareTo(y.Major);
            if (majorComparison != 0) return majorComparison;

            var minorComparison = x.Minor.CompareTo(y.Minor);
            if (minorComparison != 0) return minorComparison;

            var revisionComparison = x.Revision.CompareTo(y.Revision);
            if (revisionComparison != 0) return revisionComparison;

            var changeSetComparison = x.ChangeSet.CompareTo(y.ChangeSet);
            if (changeSetComparison != 0) return changeSetComparison;

            return 0;
        }
    }
#pragma warning restore
#nullable restore
}
