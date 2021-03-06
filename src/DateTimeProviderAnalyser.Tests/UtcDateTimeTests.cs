﻿using DateTimeProviderAnalyser.DateTimeUtcNow;
using DateTimeProviderAnalyser.Tests.TestHelpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace DateTimeProviderAnalyser.Tests
{
    public class UtcDateTimeTests : CodeFixVerifier
    {
        private const string SourceCodeWithIssue = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public TypeName()
            {
                var now = DateTime.UtcNow;
            }
        }
    }";

        private const string SourceCodeWithFix = @"
    using System;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public TypeName()
            {
                var now = DateTimeProvider.UtcNow;
            }
        }
    }";

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new DateTimeUtcNowCodeFix();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DateTimeUtcNowAnalyser();
        }

        [Fact]
        public void ExpectNoDiagnosticResults()
        {
            const string source = @"";
            VerifyCSharpDiagnostic(source);
        }

        [Fact]
        public void IdentifySuggestedFix()
        {
            var expected = new DiagnosticResult
            {
                Id = "DateTimeUtcNowAnalyser",
                Message = "Use DateTimeProvider.UtcNow instead of DateTime.UtcNow",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] {new DiagnosticResultLocation("Test0.cs", 10, 27)}
            };

            VerifyCSharpDiagnostic(SourceCodeWithIssue, expected);
        }

        [Fact]
        public void ApplySuggestedFix()
        {
            var expected = new DiagnosticResult
            {
                Id = "DateTimeUtcNowAnalyser",
                Message = "Use DateTimeProvider.UtcNow instead of DateTime.UtcNow",
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] {new DiagnosticResultLocation("Test0.cs", 10, 27)}
            };

            VerifyCSharpDiagnostic(SourceCodeWithIssue, expected);
            VerifyCSharpFix(SourceCodeWithIssue, SourceCodeWithFix, null, true);
        }
    }
}