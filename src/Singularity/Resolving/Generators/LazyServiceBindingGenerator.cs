﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Singularity.Expressions;

namespace Singularity.Resolving.Generators
{
    /// <summary>
    /// Creates bindings so that the binding can be resolved as a <see cref="Lazy{T}"/>
    /// </summary>
    public sealed class LazyServiceBindingGenerator : IServiceBindingGenerator
    {
        /// <inheritdoc />
        public IEnumerable<ServiceBinding> Resolve(IInstanceFactoryResolver resolver, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Lazy<>))
            {
                Type funcType = typeof(Func<>).MakeGenericType(type.GenericTypeArguments[0]);
                Type lazyType = typeof(Lazy<>).MakeGenericType(type.GenericTypeArguments[0]);

                ConstructorInfo constructor = lazyType.GetConstructor(new[] { funcType });

                foreach (InstanceFactory factory in resolver.TryResolveAll(funcType))
                {
                    var context = (ExpressionContext)factory.Context;
                    context.Expression = Expression.New(constructor, factory.Context.Expression);
                    var newBinding = new ServiceBinding(type, BindingMetadata.GeneratedInstance, context.Expression, type, ConstructorResolvers.Default, Lifetimes.Transient);
                    newBinding.Factories.Add(new InstanceFactory(type, context));
                    yield return newBinding;
                }
            }
        }
    }
}
