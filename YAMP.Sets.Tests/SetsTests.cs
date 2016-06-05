using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMP.Sets.Tests
{
    [TestFixture]
    public class SetsTests : Base
    {
        string setNameA = "\"A\"";

        string F(string fmt, params object[] args)
        {
            return string.Format(fmt, args);
        }


        [Test]
        public void SET_NewEmptySet()
        {
            TestNoException(F("newset({0})", setNameA));
        }

        [Test]
        public void SET_NewSetWithNumericKeys()
        {
            var set = new SetValue("whatevername", true); //sorted
            set.Set.Add(3);
            set.Set.Add(1);
            set.Set.Add(2);

            //They are "equal" if they contain the same keys. Set's Equality is based on (only) that.
            //It doesn't matter the set's name, sorting or even the key's dictionary values
            TestValue(F("unsorted = newSet({0}, 2, 1, 3)", setNameA), set);
            TestValue(F("sorted = newSortedSet({0}, 3, 2, 1)", setNameA), set);
        }

        [Test]
        public void SET_NewSetWithAlphaNumericKeys()
        {
            var set = new SetValue("otherset", true); //sorted
            set.Set.Add(3);
            set.Set.Add("1");
            set.Set.Add("abC");

            //They are "equal" if they contain the same keys. Set's Equality is based on (only) that.
            //It doesn't matter the set's name, sorting or even the key's dictionary values
            TestValue(F("unsorted = newSet({0}, \"abC\", \"1\", 3)", setNameA), set);
            TestValue(F("sorted = newSortedSet({0}, \"abC\", \"1\", 3)", setNameA), set);
        }

        [Test]
        public void SET_NewRandomSet()
        {
            TestNoException(F("newRandomSet({0}, 1000, 10000, 1)", setNameA));
        }

        [Test]
        public void SET_Add()
        {
            var set = new SetValue("otherset", true); //sorted
            set.Set.Add(3);
            set.Set.Add("2");
            set.Set.Add(1);

            TestValue(F("newSet({0}, 1, \"2\").SetAdd(3)", setNameA), set);
            TestValue(F("TAdd(newSet({0}, 1, \"2\"), 3)", setNameA), set);
        }

        [Test]
        public void SET_AsSort()
        {
            TestValue(F("newSet({0}).SetAsSort().SetIsSorted()", setNameA), 1);
            TestValue(F("TIsSorted(TAsSort(newSet({0})))", setNameA), 1);
            TestValue(F("TAsSort(newSet({0})).SetIsSorted()", setNameA), 1);
        }

        [Test]
        public void SET_AsUnsort()
        {
            TestValue(F("newSortedSet({0}).SetAsUnsort().SetIsSorted()", setNameA), 0);
            TestValue(F("TIsSorted(TAsUnsort(newSortedSet({0})))", setNameA), 0);
            TestValue(F("TAsUnsort(newSortedSet({0})).SetIsSorted()", setNameA), 0);
        }

        [Test]
        public void SET_Contains()
        {
            string sSetA = "'A', '3', 3, '1', 'abC'".Replace('\'', '\"');

            TestValue(F("newSortedSet({0}).SetContains(3)", sSetA), 1);
            TestValue(F("newSortedSet({0}).SetContains(3.1)", sSetA), 0);
            TestValue(F("newSortedSet({0}).SetContains(\"abC\")", sSetA), 1);
            TestValue(F("newSortedSet({0}).SetContains(\"abc\")", sSetA), 0);
            TestValue(F("newSortedSet({0}).SetContains(1)", sSetA), 0);
        }

        [Test]
        public void SET_Count()
        {
            TestValue(F("tcount(newSet({0}, 1, 2, 3, 2, 1))", setNameA), 3);
            TestValue(F("newSet({0}, 1, 2, 3, 2, 1).SetCount()", setNameA), 3);
            TestValue(F("newSortedSet({0}, 1, 2, 3, 2, 1).SetCount()", setNameA), 3);
        }

        [Test]
        public void SET_Equality()
        {
            var setA = new SetValue("A-sorted", true); //sorted
            setA.Set.Add("3"); //alpha
            setA.Set.Add(3); //numeric
            setA.Set.Add("1");
            setA.Set.Add("abC");

            var setB = new SetValue("B-unsorted", false); //unsorted
            setB.Set.Add("1");
            setB.Set.Add(3); //numeric
            setB.Set.Add("abC");
            setB.Set.Add("3");

            //They are "equal" if they contain the same keys. Set's Equality is based on (only) that.
            //It doesn't matter the set's name, or sorting type
            Assert.AreEqual(setA, setB);
            Assert.AreEqual(setB, setA);

            string sSetA = "'A', '3', 3, '1', 'abC'".Replace('\'', '\"');
            string sSetB = "'B', '1', 3, 'abC', '3'".Replace('\'', '\"');

            TestValue(F("(newSortedSet({0})) == (newSet({1}))", sSetA, sSetB), 1);
            TestValue(F("newSortedSet({0}) == newSet({1})", sSetA, sSetB), 1);
            TestValue(F("TEquals(newSortedSet({0}), newSet({1}))", sSetA, sSetB), 1);
        }

        [Test]
        public void SET_TExcept()
        {
            var set = new SetValue("Result");
            set.Set.Add(1);
            set.Set.Add(2);
            set.Set.Add("1");
            set.Set.Add("2");

            string sSetA = "'A', 1, 2, 3, '1', '2', '3'".Replace('\'', '\"');
            string sSetB = "'B', 3, 4, 5, '3', '4', '5'".Replace('\'', '\"');

            /*
            a=newSortedSet("A", 1, 2, 3, "1", "2", "3")
            b=newSet("B", 3, 4, 5, "3", "4", "5")
            a.SetExcept(b) == newSet("", 1, 2, "1", "2")
            a-b == newSet("", 1, 2, "1", "2")
            b-a == newSet("", 5, 4, "4", "5")
            */

            TestValue(F("newSortedSet({0}).SetExcept(newset({1}))", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); a.SetExcept(b) == newSet(\"\", 1, 2, \"1\", \"2\")", sSetA, sSetB), 1);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); a-b == newSet(\"\", 1, 2, \"1\", \"2\")", sSetA, sSetB), 1);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); c=a.SetExcept(b)", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); TExcept(a,b)", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); b-a == newSet(\"\", 5, \"4\", 4, \"5\")", sSetA, sSetB), 1);
        }

        [Test]
        public void SET_TExceptXor()
        {
            var set = new SetValue("Result");
            set.Set.Add(1);
            set.Set.Add(2);
            set.Set.Add("1");
            set.Set.Add("2");
            set.Set.Add("4");
            set.Set.Add("5");
            set.Set.Add(5);
            set.Set.Add(4);

            string sSetA = "'A', 1, 2, 3, '1', '2', '3'".Replace('\'', '\"');
            string sSetB = "'B', 3, 4, 5, '3', '4', '5'".Replace('\'', '\"');

            /*
            a=newSortedSet("A", 1, 2, 3, "1", "2", "3")
            b=newSet("B", 3, 4, 5, "3", "4", "5")
            a.SetExcept(b) == newSet("", 1, 2, "1", "2")
            a-b == newSet("", 1, 2, "1", "2")
            b-a == newSet("", 5, 4, "4", "5")
            */

            TestValue(F("newSortedSet({0}).SetExceptXor(newset({1}))", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); c=a.SetExceptXor(b)", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); TExceptXor(a,b)", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); b^a", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); b^a == a^b", sSetA, sSetB), 1);
        }

        [Test]
        public void SET_TIntersect()
        {
            var set = new SetValue("Result");
            set.Set.Add("3"); //alpha
            set.Set.Add(3); //numeric

            string sSetA = "'A', 1, 2, 3, '1', '2', '3'".Replace('\'', '\"');
            string sSetB = "'B', 3, 4, 5, '3', '4', '5'".Replace('\'', '\"');

            /*
            a=newSortedSet("A", 1, 2, 3, "1", "2", "3")
            b=newSet("B", 3, 4, 5, "3", "4", "5")
            c=a&b
            c=b&a
            c=b.SetIntersect(a)
            a&b==b&a
            */

            TestValue(F("a=newSortedSet({0}); b=newset({1}); c=a&b", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); c=b&a", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); c=b.SetIntersect(a)", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); a&b == b&a", sSetA, sSetB), 1);
        }

        [Test]
        public void SET_AreSubsets()
        {
            string sSetA = "'A', 1, 2, 3, '1', '2', '3'".Replace('\'', '\"');
            string sSetB = "'B', 3, '3', 4".Replace('\'', '\"');
            string sSetC = "'C', 3, '3'".Replace('\'', '\"');
            string sSetD = "'D', '3', 3".Replace('\'', '\"');

            /*
            a=newSortedSet("A", 1, 2, 3, "1", "2", "3")
            b=newSet("B", 3, "3", 4)
            c=newSet("C", 3, "3")
            d=newSet("D", "3", 3)
            b.SetIsSubsetOf(a)
            c.SetIsSubsetOf(a)
            TIsSubsetOf(c, a)
            TIsSubsetOf(b, a)
            TIsProperSubsetOf(c, b)
            TIsSubsetOf(c, d)
            TIsProperSubsetOf(c, d)
            */

            TestValue(F("a=newSortedSet({0}); b=newset({1}); b.SetIsSubsetOf(a)", sSetA, sSetB), 0);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); TIsSubsetOf(b, a)", sSetA, sSetB), 0);
            TestValue(F("a=newSortedSet({0}); c=newset({1}); c.SetIsSubsetOf(a)", sSetA, sSetC), 1);
            TestValue(F("a=newSortedSet({0}); c=newset({1}); TIsSubsetOf(c, a)", sSetA, sSetC), 1);
            TestValue(F("c=newSortedSet({0}); b=newset({1}); TIsProperSubsetOf(c, b)", sSetC, sSetB), 1);
            TestValue(F("c=newSortedSet({0}); b=newset({1}); c.SetIsProperSubsetOf(b)", sSetC, sSetB), 1);
            TestValue(F("c=newSortedSet({0}); d=newset({1}); TIsSubsetOf(c, d)", sSetC, sSetD), 1);
            TestValue(F("c=newSortedSet({0}); d=newset({1}); TIsProperSubsetOf(c, d)", sSetC, sSetD), 0);
            TestValue(F("a=newSet(\"A\"); c=newset({1}); TIsProperSubsetOf(a, c)", sSetA, sSetC), 1);
            TestValue(F("a=newSet(\"A\"); b=newset({1}); a.SetIsProperSubsetOf(b)", sSetA, sSetB), 1);
            TestValue(F("a=newSet(\"A\"); b=newset(\"B\"); a.SetIsProperSubsetOf(b)"), 0);
        }

        [Test]
        public void SET_AreSupersets()
        {
            string sSetA = "'A', 1, 2, 3, '1', '2', '3'".Replace('\'', '\"');
            string sSetB = "'B', 3, '3', 4".Replace('\'', '\"');
            string sSetC = "'C', 3, '3'".Replace('\'', '\"');
            string sSetD = "'D', '3', 3".Replace('\'', '\"');

            /*
            a=newSortedSet("A", 1, 2, 3, "1", "2", "3")
            b=newSet("B", 3, "3", 4)
            c=newSet("C", 3, "3")
            d=newSet("D", "3", 3)
            a.SetIsSupersetOf(b)
            a.SetIsSupersetOf(c)
            TIsSupersetOf(a, c)
            TIsSupersetOf(a, b)
            TIsProperSupersetOf(b, c)
            TIsSupersetOf(d, c)
            TIsProperSupersetOf(d, c)
            */

            TestValue(F("a=newSortedSet({0}); b=newset({1}); a.SetIsSupersetOf(b)", sSetA, sSetB), 0);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); TIsSupersetOf(a, b)", sSetA, sSetB), 0);
            TestValue(F("a=newSortedSet({0}); c=newset({1}); a.SetIsSupersetOf(c)", sSetA, sSetC), 1);
            TestValue(F("a=newSortedSet({0}); c=newset({1}); TIsSupersetOf(a, c)", sSetA, sSetC), 1);
            TestValue(F("c=newSortedSet({0}); b=newset({1}); TIsProperSupersetOf(b, c)", sSetC, sSetB), 1);
            TestValue(F("c=newSortedSet({0}); b=newset({1}); b.SetIsProperSupersetOf(c)", sSetC, sSetB), 1);
            TestValue(F("c=newSortedSet({0}); d=newset({1}); TIsSupersetOf(d, c)", sSetC, sSetD), 1);
            TestValue(F("c=newSortedSet({0}); d=newset({1}); TIsProperSupersetOf(d, c)", sSetC, sSetD), 0);
            TestValue(F("a=newSet(\"A\"); c=newset({1}); TIsProperSupersetOf(c, a)", sSetA, sSetC), 1);
            TestValue(F("a=newSet(\"A\"); b=newset({1}); b.SetIsProperSupersetOf(a)", sSetA, sSetB), 1);
            TestValue(F("a=newSet(\"A\"); b=newset(\"B\"); b.SetIsProperSupersetOf(a)"), 0);
        }

        [Test]
        public void SET_Overlaps()
        {
            string sSetA = "'A', 1, 2, 3, '1', '2', '3'".Replace('\'', '\"');
            string sSetB = "'B', 3, '3', 4".Replace('\'', '\"');
            string sSetC = "'C'".Replace('\'', '\"');

            /*
            a=newSortedSet("A", 1, 2, 3, "1", "2", "3")
            b=newSet("B", 3, "3", 4)
            c=newSet("C")
            a.SetOverlaps(b)
            b.SetOverlaps(a)
            TOverlaps(a, b)
            TOverlaps(a, c)
            TOverlaps(c, a)
            */

            TestValue(F("a=newSortedSet({0}); b=newset({1}); a.SetOverlaps(b)", sSetA, sSetB), 1);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); b.SetOverlaps(a)", sSetA, sSetB), 1);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); TOverlaps(a, b)", sSetA, sSetB), 1);
            TestValue(F("a=newSortedSet({0}); c=newset({1}); TOverlaps(a, c)", sSetA, sSetC), 0);
            TestValue(F("a=newSortedSet({0}); c=newset({1}); TOverlaps(c, a)", sSetA, sSetC), 0);
        }

        [Test]
        public void SET_Remove()
        {
            var set = new SetValue("otherset", true); //sorted
            set.Set.Add("2");
            set.Set.Add(1);

            TestValue(F("newSet({0}, 3, 1, \"2\").SetRemove(3)", setNameA), set);
            TestValue(F("TRemove(newSet({0}, 3, 1, \"2\"), 3)", setNameA), set);
        }

        [Test]
        public void SET_Matrixes()
        {
            var set = new SetValue("otherset", true); //sorted
            set.Set.Add(4);
            set.Set.Add(1);

            var mat = set.ToMatrix();

            TestValue(F("Length(newSet({0}, 4, \"2\", 1).SetToMatrix())", setNameA), 2);
            TestValue(F("newSet({0}, 1, \"2\", 4).SetToMatrix()", setNameA), mat);
            TestValue(F("TToMatrix(newSet({0}, 1, \"2\", 4))", setNameA), mat);

            TestValue(F("newSet({0}, [4, 1]).SetCount()", setNameA), 2);

            TestValue(F("r1=newRandomSet(\"\", 100, 1000, 1); m=r1.SetToMatrix(); r2=newSet(\"\", m); r1==r2", setNameA), 1);
        }


        [Test]
        public void SET_TUnion()
        {
            var set = new SetValue("Result");
            set.Set.Add("3"); //alpha
            set.Set.Add(3); //numeric
            set.Set.Add("1");
            set.Set.Add("abC");

            string sSetA = "'A', '3', 'abC'".Replace('\'', '\"');
            string sSetB = "'B', 3, '1'".Replace('\'', '\"');

            /*
            a=newSortedSet("A", "3", "abC")
            b=newSet("B", 3, "1")
            c=a+b
            c=b+a
            a+b==b+a
            a.SetUnion(b) == b+a
            a.SetUnion(b) == TUnion(b,a)
            */

            TestValue(F("a=newSortedSet({0}); b=newset({1}); c=a+b", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); c=b+a", sSetA, sSetB), set);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); a+b == b+a", sSetA, sSetB), 1);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); a.SetUnion(b) == b+a", sSetA, sSetB), 1);
            TestValue(F("a=newSortedSet({0}); b=newset({1}); a.SetUnion(b) == TUnion(b,a)", sSetA, sSetB), 1);
        }

        [Test]
        public void SET_ValidateObjectMethod()
        {
            TestExpression("(23) + 1", false);
            TestExpression("fun(23) + 1", false);
            TestExpression("a.m()", false);
            TestExpression("a.m() + 1", false);
            TestExpression("a.m().z() + 1", false);
            TestExpression("a.m().z() == a.s()", false);
            TestExpression("a.m(v.d())", false);
            TestExpression("x.abc(2 + 19 * 5)", false);
            TestExpression("x.abc + (2 + 19 * 5)", false);

            //TestExpression("a.()", true);
            //TestExpression("a.(1)", true);
            TestExpression(".()", true);
            TestExpression(".(1)", true);
            TestExpression(".a()", true);
        }

        [Test]
        public void SET_NestedObjectInObject()
        {
            var set = new SetValue("Result");
            set.Set.Add("3"); //alpha
            set.Set.Add(3); //numeric

            string sSetA = "'A', newset('', 1, 2, 3).SetCount(), '3'".Replace('\'', '\"');

            TestValue(F("a=newSortedSet({0})", sSetA), set);
        }

    }
}
