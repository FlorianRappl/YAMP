using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMP.Core.Tests
{
    [TestFixture]
    public class Objects : Base
    {
        [Test]
        public void CreateObject()
        {
            Test("object()", false);
        }

        [Test]
        public void SetAndGetValueInObject()
        {
            Test("a = object(); a.b = 7; a.b", 7.0);
        }

        [Test]
        public void SetAndGetToCalculateValueInObject()
        {
            Test("a = object(); a.b = 7; a.c = 19.5; a.c - a.b", 12.5);
        }

        [Test]
        public void NestedObjectInObject()
        {
            Test("a = object(); a.b = object(); a.b.c = 2.9; a.b.c - 1.0", 1.9);
        }

        [Test]
        public void ObjectOperatorPrecedenceAdditionLeft()
        {
            Test("a = object(); a.b = 2; a.b-1", 1.0);
        }

        [Test]
        public void ObjectOperatorPrecedenceAdditionRight()
        {
            Test("a = object(); a.b = 2; 3-a.b", 1.0);
        }

        [Test]
        public void ObjectOperatorPrecedenceMultiplicationLeft()
        {
            Test("a = object(); a.b = 2; a.b*2", 4.0);
        }

        [Test]
        public void ObjectOperatorPrecedenceMultiplicationRight()
        {
            Test("a = object(); a.b = 2; 5*a.b", 10.0);
        }

        [Test]
        public void ObjectOperatorPrecedencePowerArgument()
        {
            Test("a = object(); a.b = 2; 3^a.b", 8.0);
        }

        [Test]
        public void ObjectOperatorPrecedenceNegate()
        {
            Test("a = object(); a.b = 2; -a.b", -2.0);
        }

        [Test]
        public void ObjectOperatorPrecedenceFlip()
        {
            Test("a = object(); a.b = false; ~a.b", 1.0);
        }

        [Test]
        public void ObjectOperatorPrecedenceFactorial()
        {
            Test("a = object(); a.b = 3; a.b!", 6.0);
        }
    }
}
