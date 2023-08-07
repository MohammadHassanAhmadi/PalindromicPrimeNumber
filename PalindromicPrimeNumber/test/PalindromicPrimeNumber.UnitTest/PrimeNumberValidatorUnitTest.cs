using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using PalindromicPrimeNumber.Services;
using Xunit;

namespace PalindromicPrimeNumber.UnitTest
{
    public class PrimeNumberValidatorUnitTest
    {
        public PrimeNumberValidatorUnitTest()
        {
            notPrimeNumbersLessThan1000 = new List<int>();
            for (var i = 1; i < 1000; i++)
                if (!primeNumbersLessThan1000.Contains(i))
                    notPrimeNumbersLessThan1000.Add(i);
        }

        private readonly int[] primeNumbersLessThan1000 =
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103,
            107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223,
            227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347,
            349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463,
            467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607,
            613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743,
            751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883,
            887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997
        };

        private readonly List<int> notPrimeNumbersLessThan1000;

        [Fact]
        public void TestValidate_WhenNumberIsNotPrime_ReturnsFalse()
        {
            var primeNumberValidator = new PrimeNumberValidator();
            foreach (var number in notPrimeNumbersLessThan1000)
                primeNumberValidator.Validate(number).Should().BeFalse();
        }

        [Fact]
        public void TestValidate_WhenNumberIsPrime_ReturnsTrue()
        {
            var primeNumberValidator = new PrimeNumberValidator();
            foreach (var number in primeNumbersLessThan1000)
                primeNumberValidator.Validate(number).Should().BeTrue();
        }
    }
}
/*This is a non-trivial task as it involves parsing two different types of data, comparing them, and outputting the differences. Below is a simplified example of how you might go about it. This example uses `System.Xml.Schema` for the XSD parsing and `Microsoft.CodeAnalysis.CSharp` for the C# parsing.

This code won't cover all edge cases and nuances of XML schema and C# class representation but it should serve as a starting point. 

```csharp
using System;
using System.Xml.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class XsdToCSharpComparer
{
    public Dictionary<string, XmlSchemaComplexType> XsdComplexTypes { get; private set; }
    public List<ClassDeclarationSyntax> CSharpClasses { get; private set; }

    public XsdToCSharpComparer(string xsdFilePath, string csFilePath)
    {
        XsdComplexTypes = new Dictionary<string, XmlSchemaComplexType>();
        ParseXsd(xsdFilePath);

        CSharpClasses = new List<ClassDeclarationSyntax>();
        ParseCSharp(csFilePath);
    }

    private void ParseXsd(string xsdFilePath)
    {
        var xsd = XmlSchema.Read(File.OpenRead(xsdFilePath), null);

        foreach(XmlSchemaObject item in xsd.Items)
        {
            if(item is XmlSchemaComplexType complexType)
            {
                XsdComplexTypes.Add(complexType.Name, complexType);
            }
        }
    }

    private void ParseCSharp(string csFilePath)
    {
        var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(csFilePath));
        var root = (CompilationUnitSyntax)tree.GetRoot();
        CSharpClasses = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
    }

    public void Compare()
    {
        foreach(var cSharpClass in CSharpClasses)
        {
            if(XsdComplexTypes.ContainsKey(cSharpClass.Identifier.Text))
            {
                // Assuming that the class properties correspond to the complexType sequence
                var xsdProperties = XsdComplexTypes[cSharpClass.Identifier.Text].Particle as XmlSchemaSequence;
                var cSharpProperties = cSharpClass.DescendantNodes().OfType<PropertyDeclarationSyntax>();

                foreach (var xsdElement in xsdProperties.Items.OfType<XmlSchemaElement>())
                {
                    if(!cSharpProperties.Any(p => p.Identifier.Text == xsdElement.Name))
                    {
                        Console.WriteLine($"Property {xsdElement.Name} of class {cSharpClass.Identifier.Text} is missing in the C# class but exists in the XSD.");
                    }
                }

                foreach (var cSharpProperty in cSharpProperties)
                {
                    if(!xsdProperties.Items.OfType<XmlSchemaElement>().Any(x => x.Name == cSharpProperty.Identifier.Text))
                    {
                        Console.WriteLine($"Property {cSharpProperty.Identifier.Text} of class {cSharpClass.Identifier.Text} is missing in the XSD but exists in the C# class.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Class {cSharpClass.Identifier.Text} is missing in the XSD but exists in the C# classes.");
            }
        }

        foreach(var xsdComplexType in XsdComplexTypes)
        {
            if(!CSharpClasses.Any(c => c.Identifier.Text == xsdComplexType.Key))
            {
                Console.WriteLine($"Class {xsdComplexType.Key} is missing in the C# classes but exists in the XSD.");
            }
        }
    }
}
```

Please note that this is a simplified version and might not cover all use cases. It will only cover the class names and properties. If you want to compare methods and other elements, you'll need to extend this code. Also, make sure to add proper error handling as this is just a basic structure of the program.
*/
