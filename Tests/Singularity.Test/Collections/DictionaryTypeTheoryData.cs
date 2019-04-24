﻿using System;
using System.Collections.Generic;
using Singularity.TestClasses.Extensions;
using Singularity.TestClasses.TestClasses;
using Xunit;

namespace Singularity.Test.Collections
{
    public class DictionaryTypeTheoryData : TheoryData<IEnumerable<(Type type, ReferenceInt value)>>
    {
        public DictionaryTypeTheoryData()
        {
            var typeMappings = new List<(Type type, ReferenceInt value)>();
            typeMappings.Add((typeof(ITestService10), 0));
            typeMappings.Add((typeof(ITestService11), 1));
            typeMappings.Add((typeof(ITestService12), 2));

            typeMappings.Add((typeof(ITestService20), 4));

            IEnumerable<IEnumerable<(Type type, ReferenceInt value)>> permutations = typeMappings.GetPermutations();

            foreach (IEnumerable<(Type type, ReferenceInt value)> permutation in permutations)
            {
                Add(permutation);
            }
        }
    }
}