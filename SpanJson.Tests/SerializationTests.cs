﻿using System;
using System.Collections.Generic;
using SpanJson.Resolvers;
using Xunit;
using Xunit.Sdk;

namespace SpanJson.Tests
{
    public class SerializationTests
    {
        public class Parent
        {
            public List<Child> Children { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class Child
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [Fact]
        public void NoNameMatches()
        {
            var parent = new Parent { Age = 30, Name = "Adam", Children = new List<Child> { new Child { Name = "Cain", Age = 5 } } };
            var serializedWithCamelCase =
                JsonSerializer.Generic.Serialize<Parent, ExcludeNullsOriginalCaseResolver>(parent);
            serializedWithCamelCase = serializedWithCamelCase.ToLowerInvariant();
            Assert.Contains("age", serializedWithCamelCase);
            var deserialized =
                JsonSerializer.Generic.Deserialize<Parent, ExcludeNullsOriginalCaseResolver>(serializedWithCamelCase);
            Assert.NotNull(deserialized);
            Assert.Null(deserialized.Children);
            Assert.Equal(0, deserialized.Age);
            Assert.Null(deserialized.Name);
        }

        public class Node
        {
            public Guid Id { get; set; }
            public List<Node> Children { get; set; } = new List<Node>();
        }

        //[Fact] // TODO Break Recursion
        //public void Loops()
        //{
        //    var node = new Node {Id = Guid.NewGuid()};
        //    node.Children.Add(node);
        //    //var serialized = JsonSerializer.Generic.Serialize(node);
        //    Assert.NotNull(serialized);
        //}
    }
}