﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

using Singularity.Collections;
using Singularity.Expressions;
using Singularity.Logging;
using Singularity.Resolving.Generators;

namespace Singularity
{
    /// <summary>
    /// Defines settings for the singularity container.
    /// </summary>
    public sealed class SingularitySettings
    {
        /// <summary>
        /// The default singularity settings.
        /// </summary>
        public static SingularitySettings Default => new SingularitySettings();

        /// <summary>
        /// Settings for microsoft dependency injection.
        /// </summary>
        public static SingularitySettings Microsoft => new SingularitySettings(s =>
        {
            s.AutoDispose(Lifetimes.Transient, Lifetimes.PerContainer, Lifetimes.PerScope, Lifetimes.PerGraph);
            s.IgnoreResolveError(new PatternTypeMatcher("Microsoft.*"));
        });

        /// <summary>
        /// The <see cref="IServiceBindingGenerator"/>s that are used to dynamically generate bindings.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ConfigurationList<IServiceBindingGenerator> ServiceBindingGenerators { get; } = new ConfigurationList<IServiceBindingGenerator> {
            new ContainerServiceBindingGenerator(),
            new CollectionServiceBindingGenerator(),
            new ExpressionServiceBindingGenerator(),
            new LazyServiceBindingGenerator(),
            new FactoryServiceBindingGenerator(),
            new ConcreteServiceBindingGenerator(),
            new OpenGenericBindingGenerator()
        };

        /// <summary>
        /// Specifies what lifetimes should be auto disposed if the instance is a <see cref="IDisposable"/> and the service is registered with <see cref="ServiceAutoDispose.Default"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public HashSet<Type> AutoDisposeLifetimes { get; } = new HashSet<Type>();

        /// <summary>
        /// The constructor selector that will be used by default.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IConstructorResolver ConstructorResolver { get; private set; } = ConstructorResolvers.Default;

        /// <summary>
        /// The logger that is used.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ISingularityLogger Logger { get; private set; } = Loggers.Default;

        /// <summary>
        /// Prevents Singularity to throw errors in some cases
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<ITypeMatcher> ResolveErrorsExclusions { get; } = new List<ITypeMatcher>();

        /// <summary>
        /// Excludes some types from a <see cref="IServiceBindingGenerator"/>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<Type, List<ITypeMatcher>> ResolverExclusions { get; } = new Dictionary<Type, List<ITypeMatcher>>();

        private SingularitySettings(Action<SingularitySettings>? configurator = null) { configurator?.Invoke(this); }

        public void ConfigureServiceBindingGenerators(Action<ConfigurationList<IServiceBindingGenerator>> configurator)
        {
            configurator.Invoke(ServiceBindingGenerators);
        }

        public void IgnoreResolveError(ITypeMatcher match)
        {
            ResolveErrorsExclusions.Add(match);
        }

        public void ExcludeAutoRegistration<TResolverType>(ITypeMatcher match)
            where TResolverType : IServiceBindingGenerator
        {
            ExcludeAutoRegistration(typeof(TResolverType), match);
        }

        public void ExcludeAutoRegistration(Type resolverType, ITypeMatcher match)
        {
            if (!ResolverExclusions.TryGetValue(resolverType, out List<ITypeMatcher> exclusions))
            {
                exclusions = new List<ITypeMatcher>();
                ResolverExclusions.Add(resolverType, exclusions);
            }
            exclusions.Add(match);
        }

        public void With(ISingularityLogger logger) => Logger = logger;

        public void With(IConstructorResolver constructorResolver) => ConstructorResolver = constructorResolver;

        public void AutoDispose(params ILifetime[] autoDisposeLifetimes)
        {
            foreach (ILifetime autoDisposeLifetime in autoDisposeLifetimes)
            {
                AutoDispose(autoDisposeLifetime);
            }
        }
        public void AutoDispose(ILifetime lifetime) => AutoDisposeLifetimes.Add(lifetime.GetType());
    }
}
