﻿// <auto-generated>
//   This code file has automatically been added by the "Bannerlord.ModuleManager.Source" NuGet package (https://www.nuget.org/packages/Bannerlord.ModuleManager.Source).
//   Please see https://github.com/BUTR/Bannerlord.ModuleManager for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Bannerlord.ModuleManager.Source" folder and the "AlphanumComparatorFast.cs" file don't appear in your project.
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
    using global::System;
    using global::System.Collections;
    using global::System.Collections.Generic;
    using global::System.Text;

    /// <summary>
    /// Alphanumeric sort. This sorting algorithm logically handles numbers in strings. It makes sense to humans.
    /// Highway names like 50F and 100F will be sorted wrongly with ASCII sort.
    /// It is different from alphabetic, ASCII or numeric sorting. This algorithmic approach is used in file managers.
    /// </summary>
    internal sealed class AlphanumComparatorFast : IComparer<string?>, IComparer
    {
        /// <inheritdoc/>
        public int Compare(object? x, object? y) => Compare(x as string, y as string);

        /// <inheritdoc/>
        public int Compare(string? s1, string? s2)
        {
            if (s1 is null && s2 is null)
                return 0;
            if (s1 is null)
                return -1;
            if (s2 is null)
                return 1;

            var len1 = s1.Length;
            var len2 = s2.Length;
            if (len1 == 0 && len2 == 0)
                return 0;
            if (len1 == 0)
                return -1;
            if (len2 == 0)
                return 1;

            var marker1 = 0;
            var marker2 = 0;
            while (marker1 < len1 || marker2 < len2)
            {
                if (marker1 >= len1)
                {
                    return -1;
                }
                if (marker2 >= len2)
                {
                    return 1;
                }
                var ch1 = s1[marker1];
                var ch2 = s2[marker2];

                var chunk1 = new StringBuilder();
                var chunk2 = new StringBuilder();

                while (marker1 < len1 && (chunk1.Length == 0 || InChunk(ch1, chunk1[0])))
                {
                    chunk1.Append(ch1);
                    marker1++;

                    if (marker1 < len1)
                        ch1 = s1[marker1];
                }

                while (marker2 < len2 && (chunk2.Length == 0 || InChunk(ch2, chunk2[0])))
                {
                    chunk2.Append(ch2);
                    marker2++;

                    if (marker2 < len2)
                        ch2 = s2[marker2];
                }

                var result = 0;
                // If both chunks contain numeric characters, sort them numerically
                if (char.IsDigit(chunk1[0]) && char.IsDigit(chunk2[0]))
                {
                    var numericChunk1 = Convert.ToInt32(chunk1.ToString());
                    var numericChunk2 = Convert.ToInt32(chunk2.ToString());

                    if (numericChunk1 < numericChunk2)
                        result = -1;
                    if (numericChunk1 > numericChunk2)
                        result = 1;
                }
                else
                {
                    result = string.CompareOrdinal(chunk1.ToString(), chunk2.ToString());
                }

                if (result != 0)
                    return result;
            }

            return 0;
        }

        private static bool InChunk(char ch, char otherCh)
        {
            var type = ChunkType.Alphanumeric;

            if (char.IsDigit(otherCh))
                type = ChunkType.Numeric;

            return (type != ChunkType.Alphanumeric || !char.IsDigit(ch)) && (type != ChunkType.Numeric || char.IsDigit(ch));
        }

        private enum ChunkType
        {
            Alphanumeric,
            Numeric
        }
    }
}