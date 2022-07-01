﻿// <auto-generated>
//   This code file has automatically been added by the "Bannerlord.ModuleManager.Source" NuGet package (https://www.nuget.org/packages/Bannerlord.ModuleManager.Source).
//   Please see https://github.com/BUTR/Bannerlord.ModuleManager for more information.
//
//   IMPORTANT:
//   DO NOT DELETE THIS FILE if you are using a "packages.config" file to manage your NuGet references.
//   Consider migrating to PackageReferences instead:
//   https://docs.microsoft.com/en-us/nuget/consume-packages/migrate-packages-config-to-package-reference
//   Migrating brings the following benefits:
//   * The "Bannerlord.ModuleManager.Source" folder and the "ModuleUtilities.cs" file don't appear in your project.
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
    using global::System.Collections.Generic;
    using global::System.Linq;

#nullable enable
#pragma warning disable
#if !BANNERLORDBUTRMODULEMANAGER_PUBLIC
    internal
#else
    public
# endif
        sealed record ModuleIssue(ModuleInfoExtended Target, string SourceId, ModuleIssueType Type)
    {
        public string Reason { get; init; }
        public ApplicationVersionRange SourceVersion { get; init; }
    }
    
#if !BANNERLORDBUTRMODULEMANAGER_PUBLIC
    internal
#else
    public
# endif
        enum ModuleIssueType
    {
        MissingDependencies,
        DependencyMissingDependencies,
        DependencyValidationError,
        VersionMismatch,
        Incompatible,
    }

#if !BANNERLORDBUTRMODULEMANAGER_PUBLIC
    internal
#else
    public
# endif
        static class ModuleUtilities
    {
        /*
        private static bool CheckModuleCompatibility(ModuleInfoExtended moduleInfoExtended)
        {
            static bool CheckIfSubModuleCanBeLoaded(SubModuleInfoExtended subModuleInfo)
            {
                if (subModuleInfo.Tags.Count > 0)
                {
                    foreach (var kv in subModuleInfo.Tags)
                    {
                        if (!Enum.TryParse<SubModuleTags>(kv.Key, out var tag))
                            continue;

                        foreach (var value in kv.Value)
                        {
                            if (!GetSubModuleTagValiditiy(tag, value))
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                return true;
            }
            static bool GetSubModuleTagValiditiy(SubModuleTags tag, string value) => tag switch
            {
                SubModuleTags.RejectedPlatform => !Enum.TryParse<Platform>(value, out var platform) || ApplicationPlatform.CurrentPlatform != platform,
                SubModuleTags.ExclusivePlatform => !Enum.TryParse<Platform>(value, out var platform) || ApplicationPlatform.CurrentPlatform == platform,
                SubModuleTags.DependantRuntimeLibrary => !Enum.TryParse<Runtime>(value, out var runtime) || ApplicationPlatform.CurrentRuntimeLibrary == runtime,
                SubModuleTags.IsNoRenderModeElement => value.Equals("false"),
                SubModuleTags.DedicatedServerType => value.ToLower() switch
                {
                    "none" => true,
                    _ => false
                },
                _ => true
            };

            foreach (var subModule in moduleInfoExtended.SubModules.Where(CheckIfSubModuleCanBeLoaded))
            {
                var asm = Path.GetFullPath(Path.Combine(BasePath.Name, "Modules", moduleInfoExtended.Id, "bin", "Win64_Shipping_Client", subModule.DLLName));
                switch (Manager._compatibilityChecker.CheckAssembly(asm))
                {
                    case CheckResult.TypeLoadException:
                        AppendIssue(instance, moduleInfoExtended, "Not binary compatible with the current game version!");
                        return false;
                    case CheckResult.ReflectionTypeLoadException:
                        AppendIssue(instance, moduleInfoExtended, "Not binary compatible with the current game version!");
                        return false;
                    case CheckResult.GenericException:
                        AppendIssue(instance, moduleInfoExtended, "There was an error checking for binary compatibility with the current game version");
                        return false;
                }
            }

            return true;
        }
        */
        
        public static IEnumerable<ModuleIssue> ValidateModule(
            Dictionary<string, ModuleInfoExtended> modules, ModuleInfoExtended targetModule, HashSet<ModuleInfoExtended> visitedModules,
            Func<ModuleInfoExtended, bool> isSelected)
        {
            /*
            if (!CheckModuleCompatibility(moduleInfoExtended))
            {
                yield return new ModuleIssue(moduleInfoExtended, moduleInfoExtended.Id, "Not compatible!");
                yield break;
            }
            */

            // Check that all dependencies are present
            foreach (var dependency in targetModule.DependentModules)
            {
                // Ignore the check for Optional
                if (dependency.IsOptional) continue;

                if (!modules.ContainsKey(dependency.Id))
                {
                    yield return new ModuleIssue(targetModule, dependency.Id, ModuleIssueType.MissingDependencies)
                    {
                        Reason = $"Missing {dependency.Id} {dependency.Version}",
                        SourceVersion = new(dependency.Version, dependency.Version)
                    };
                    yield break;
                }
            }
            foreach (var metadata in targetModule.DependentModuleMetadatas)
            {
                // Ignore the check for Optional
                if (metadata.IsOptional) continue;

                // Ignore the check for Incompatible
                if (metadata.IsIncompatible) continue;

                if (!modules.ContainsKey(metadata.Id))
                {
                    if (metadata.Version != ApplicationVersion.Empty)
                    {
                        yield return new ModuleIssue(targetModule, metadata.Id, ModuleIssueType.MissingDependencies)
                        {
                            Reason = $"Missing {metadata.Id} {metadata.Version}",
                            SourceVersion = new(metadata.Version, metadata.Version)
                        };
                    }
                    if (metadata.VersionRange != ApplicationVersionRange.Empty)
                    {
                        yield return new ModuleIssue(targetModule, metadata.Id, ModuleIssueType.MissingDependencies)
                        {
                            Reason = $"Missing {metadata.Id} {metadata.VersionRange}",
                            SourceVersion = metadata.VersionRange
                        };
                    }
                    yield break;
                }
            }

            // Check that the dependencies themselves have all dependencies present
            var opts = new ModuleSorterOptions { SkipOptionals = true, SkipExternalDependencies = true };
            foreach (var dependency in ModuleSorter.GetDependentModulesOf(modules.Values, targetModule, visitedModules, opts).ToArray())
            {
                if (targetModule.DependentModules.FirstOrDefault(dmm => dmm.Id == dependency.Id) is { } dependencyData)
                {
                    // Not found, might be from DependentModuleMetadatas
                    if (dependencyData is null) continue;

                    // Ignore the check for Optional
                    if (dependencyData.IsOptional) continue;

                    // Check missing dependency dependencies
                    if (!modules.TryGetValue(dependency.Id, out var depencencyModuleInfo))
                    {
                        yield return new ModuleIssue(targetModule, dependency.Id, ModuleIssueType.DependencyMissingDependencies)
                        {
                            Reason = $"'{dependency.Id}' is missing it's dependencies!"
                        };
                        yield break;
                    }
                
                    // Check depencency correctness
                    if (ValidateModule(modules, depencencyModuleInfo, visitedModules, isSelected).Any())
                    {
                        yield return new ModuleIssue(targetModule, dependency.Id, ModuleIssueType.DependencyValidationError)
                        {
                            Reason = $"'{dependency.Id}' has unresolved issues!"
                        };
                        yield break;
                    }
                }
                if (targetModule.DependentModuleMetadatas.FirstOrDefault(dmm => dmm.Id == dependency.Id) is { } dependencyMetadata)
                {
                    // Not found, might be from DependentModules
                    if (dependencyMetadata is null) continue;

                    // Handle onlyirect dependencies
                    if (dependencyMetadata.LoadType != LoadType.LoadBeforeThis) continue;
                    
                    // Ignore the check for Optional
                    if (dependencyMetadata.IsOptional) continue;

                    // Ignore the check for Incompatible
                    if (dependencyMetadata.IsIncompatible) continue;

                    // Check missing dependency dependencies
                    if (!modules.TryGetValue(dependency.Id, out var depencencyModuleInfo))
                    {
                        yield return new ModuleIssue(targetModule, dependency.Id, ModuleIssueType.DependencyMissingDependencies)
                        {
                            Reason = $"'{dependency.Id}' is missing it's dependencies!"
                        };
                        yield break;
                    }

                    // Check depencency correctness
                    if (ValidateModule(modules, depencencyModuleInfo, visitedModules, isSelected).Any())
                    {
                        yield return new ModuleIssue(targetModule, dependency.Id, ModuleIssueType.DependencyValidationError)
                        {
                            Reason = $"'{dependency.Id}' has unresolved issues!"
                        };
                        yield break;
                    }
                }
            }

            // Check that the dependencies have the minimum required version set by DependedModuleMetadatas
            var comparer = new ApplicationVersionComparer();
            foreach (var metadata in targetModule.DependentModuleMetadatas.Where(m => /*!m.IsOptional &&*/ !m.IsIncompatible))
            {
                // Handle only direct dependencies
                if (metadata.LoadType != LoadType.LoadBeforeThis) continue;

                // Ignore the check for empty versions
                if (metadata.Version == ApplicationVersion.Empty && metadata.VersionRange == ApplicationVersionRange.Empty) continue;

                // Dependency is loaded
                if (!modules.TryGetValue(metadata.Id, out var dependedModule)) continue;

                if (metadata.Version != ApplicationVersion.Empty)
                {
                    // dependedModuleMetadata.Version > dependedModule.Version
                    if (!metadata.IsOptional && (comparer.Compare(metadata.Version, dependedModule?.Version) > 0))
                    {
                        yield return new ModuleIssue(targetModule, dependedModule?.Id, ModuleIssueType.VersionMismatch)
                        {
                            Reason = $"'{dependedModule?.Id}' wrong version <= {metadata.Version}",
                            SourceVersion = new(metadata.Version, metadata.Version)
                        };
                        yield break;
                    }
                }

                if (metadata.VersionRange != ApplicationVersionRange.Empty)
                {
                    // dependedModuleMetadata.Version > dependedModule.VersionRange.Min
                    // dependedModuleMetadata.Version < dependedModule.VersionRange.Max
                    if (!metadata.IsOptional)
                    {
                        if (comparer.Compare(metadata.VersionRange.Min, dependedModule?.Version) > 0)
                        {
                            yield return new ModuleIssue(targetModule, dependedModule?.Id, ModuleIssueType.VersionMismatch)
                            {
                                Reason = $"'{dependedModule?.Id}' wrong version < [{metadata.VersionRange}]",
                                SourceVersion = metadata.VersionRange
                            };
                            yield break;
                        }
                        if (comparer.Compare(metadata.VersionRange.Max, dependedModule?.Version) < 0)
                        {
                            yield return new ModuleIssue(targetModule, dependedModule?.Id, ModuleIssueType.VersionMismatch)
                            {
                                Reason = $"'{dependedModule?.Id}' wrong version > [{metadata.VersionRange}]",
                                SourceVersion = metadata.VersionRange
                            };
                            yield break;
                        }
                    }
                }
            }

            // Do not load this mod if an incompatible mod is selected
            foreach (var dependency in targetModule.IncompatibleModules)
            {
                // Dependency is loaded
                if (!modules.TryGetValue(dependency.Id, out var depencencyModule) && !isSelected(depencencyModule)) continue;

                // If the incompatible mod is selected, this mod should be disabled
                if (isSelected(depencencyModule))
                {
                    yield return new ModuleIssue(targetModule, depencencyModule.Id, ModuleIssueType.Incompatible)
                    {
                        Reason = $"'{depencencyModule.Id}' is incompatible with this module"
                    };
                    yield break;
                }
            }
            foreach (var metadata in targetModule.DependentModuleMetadatas.Where(m => m.IsIncompatible))
            {
                // Dependency is loaded
                if (!modules.TryGetValue(metadata.Id, out var metadataModule) && !isSelected(metadataModule)) continue;

                // If the incompatible mod is selected, this mod should be disabled
                if (isSelected(metadataModule))
                {
                    yield return new ModuleIssue(targetModule, metadataModule.Id, ModuleIssueType.Incompatible)
                    {
                        Reason = $"'{metadataModule.Id}' is incompatible with this module"
                    };
                    yield break;
                }
            }

            // If another mod declared incompatibility and is selected, disable this
            foreach (var module in modules.Values)
            {
                // Ignore self
                if (module.Id == targetModule.Id) continue;

                if (!isSelected(module)) continue;
                
                foreach (var metadata in module.DependentModuleMetadatas.Where(m => m.IsIncompatible && m.Id == targetModule.Id))
                {
                    // If the incompatible mod is selected, this mod is disabled
                    if (isSelected(module))
                    {
                        yield return new ModuleIssue(targetModule, module.Id, ModuleIssueType.Incompatible)
                        {
                            Reason = $"'{module.Id}' is incompatible with this module"
                        };
                        yield break;
                    }
                }
            }
        }
        
        public static IEnumerable<ModuleIssue> ToggleModuleSelection(
            Dictionary<string, ModuleInfoExtended> modules, ModuleInfoExtended targetModule, HashSet<ModuleInfoExtended> visitedModules,
            Func<ModuleInfoExtended, bool> getSelected, Action<ModuleInfoExtended, bool> setSelected,
            Func<ModuleInfoExtended, bool> getDisabled, Action<ModuleInfoExtended, bool> setDisabled)
        {
            if (getSelected(targetModule))
            {
                foreach (var issue in DisableModule(modules, targetModule, visitedModules, getSelected, setSelected, getDisabled, setDisabled))
                    yield return issue;
            }
            else
            {
                foreach (var issue in EnableModule(modules, targetModule, visitedModules, getSelected, setSelected, getDisabled, setDisabled))
                    yield return issue;
            }
        }
        public static IEnumerable<ModuleIssue> EnableModule(
            Dictionary<string, ModuleInfoExtended> modules, ModuleInfoExtended targetModule, HashSet<ModuleInfoExtended> visitedModules,
            Func<ModuleInfoExtended, bool> getSelected, Action<ModuleInfoExtended, bool> setSelected,
            Func<ModuleInfoExtended, bool> getDisabled, Action<ModuleInfoExtended, bool> setDisabled)
        {
            setSelected(targetModule, true);

            var opt = new ModuleSorterOptions { SkipOptionals = true, SkipExternalDependencies = true };
            var dependencies = ModuleSorter.GetDependentModulesOf(modules.Values, targetModule, visitedModules, opt).ToArray();
            
            // Select all dependencies
            foreach (var module in modules.Values)
            {
                if (!getSelected(module) && dependencies.Any(d => d.Id == module.Id))
                {
                    foreach (var issue in EnableModule(modules, module, visitedModules, getSelected, setSelected, getDisabled, setDisabled))
                        yield return issue;
                }
            }
            
            // Enable modules that are marked as LoadAfterThis
            foreach (var metadata in targetModule.DependentModuleMetadatas)
            {
                if (metadata.IsOptional) continue;
                
                if (metadata.IsIncompatible) continue;
                
                if (metadata.LoadType != LoadType.LoadAfterThis) continue;
                
                if (!modules.TryGetValue(metadata.Id, out var metadataModule)) continue;

                if (!getSelected(metadataModule))
                {
                    foreach (var issue in EnableModule(modules, metadataModule, visitedModules, getSelected, setSelected, getDisabled, setDisabled))
                        yield return issue;
                }
            }

            // Deselect and disable any mod that is incompatible with this one
            foreach (var metadata in targetModule.DependentModuleMetadatas.Where(dmm => dmm.IsIncompatible))
            {
                if (!modules.TryGetValue(metadata.Id, out var incompatibleModule)) continue;
                
                if (getSelected(incompatibleModule))
                {
                    foreach (var issue in DisableModule(modules, incompatibleModule, visitedModules, getSelected, setSelected, getDisabled, setDisabled))
                        yield return issue;
                }

                setDisabled(incompatibleModule, true);
                yield return new ModuleIssue(incompatibleModule, incompatibleModule.Id, ModuleIssueType.Incompatible)
                {
                    Reason = $"'{targetModule.Id}' is incompatible with this"
                };
            }

            // Disable any mod that declares this mod as incompatible
            foreach (var module in modules.Values)
            {
                foreach (var dmm in module.DependentModuleMetadatas.Where(dmm => dmm.IsIncompatible && dmm.Id == targetModule.Id))
                {
                    if (getSelected(module))
                    {
                        foreach (var issue in DisableModule(modules, module, visitedModules, getSelected, setSelected, getDisabled, setDisabled))
                            yield return issue;
                    }

                    // We need to re-check that everything is alright with the external dependency
                    setDisabled(module, getDisabled(module) | !ModuleSorter.AreAllDependenciesOfModulePresent(modules.Values, module));
                }
            }
        }
        public static IEnumerable<ModuleIssue> DisableModule(
            Dictionary<string, ModuleInfoExtended> modules, ModuleInfoExtended targetModule, HashSet<ModuleInfoExtended> visitedModules,
            Func<ModuleInfoExtended, bool> getSelected, Action<ModuleInfoExtended, bool> setSelected,
            Func<ModuleInfoExtended, bool> getDisabled, Action<ModuleInfoExtended, bool> setDisabled)
        {
            setSelected(targetModule, false);

            var opt = new ModuleSorterOptions { SkipOptionals = true, SkipExternalDependencies = true };
            
            // Vanilla check
            // Deselect all modules that depend on this module if they are selected
            foreach (var module in modules.Values/*.Where(m => !m.IsOfficial)*/)
            {
                var dependencies = ModuleSorter.GetDependentModulesOf(modules.Values, module, visitedModules, opt);
                if (getSelected(module) && dependencies.Any(d => d.Id == targetModule.Id))
                {
                    foreach (var issue in DisableModule(modules, module, visitedModules, getSelected, setSelected, getDisabled, setDisabled))
                        yield return issue;
                }
            }

            // Enable for selection any mod that is incompatible with this one
            foreach (var metadata in targetModule.DependentModuleMetadatas.Where(dmm => dmm.IsIncompatible))
            {
                if (!modules.TryGetValue(metadata.Id, out var incompatibleModule)) continue;
                setDisabled(incompatibleModule, false);
            }

            // Check if any mod that declares this mod as incompatible can be Enabled
            foreach (var module in modules.Values)
            {
                foreach (var metadata in module.DependentModuleMetadatas.Where(dmm => dmm.IsIncompatible && dmm.Id == targetModule.Id))
                {
                    // We need to re-check that everything is alright with the external dependency
                    setDisabled(module, getDisabled(module) & !ModuleSorter.AreAllDependenciesOfModulePresent(modules.Values, module));
                }
            }
        }
    }
#pragma warning restore
#nullable restore
}
