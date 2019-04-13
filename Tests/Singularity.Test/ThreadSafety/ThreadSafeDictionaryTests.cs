﻿using System.Collections.Generic;
using Singularity.Collections;
using Singularity.Test.Collections;
using Xunit;

namespace Singularity.Test.ThreadSafety
{
    public class ThreadSafeDictionaryTests
    {
        [Fact]
        public void AddAndGet()
        {
            var testCases = new List<(ReferenceInt input, object output)>();

            for (var i = 0; i < 1000; i++)
            {
                testCases.Add((i, new object()));
            }

            var dic = new ThreadSafeDictionary<ReferenceInt, object>();

            var tester = new ThreadSafetyTester<(ReferenceInt input, object expectedOutput)>();

            tester.Test(testCase =>
            {
                dic.Add(testCase.input, testCase.expectedOutput);
                object output = dic.Search(testCase.input);
                Assert.Equal(testCase.expectedOutput, output);
            }, testCases);

            Assert.Equal(testCases.Count * tester.TaskCount ,dic.Count);
        }
    }
}