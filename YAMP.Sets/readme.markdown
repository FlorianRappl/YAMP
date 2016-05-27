Set (SetValue)
==============

Features
--------

- Supports Sorted and Hashed Sets
- Portable library compatibility
- SortedSet is borrowed from Core FX (.NET Foundation)
- "Object oriented" functionality (@this) on most functions
- "@this" functions (MemberFunctions) get object's name automatically prepended (to limit name collisions)
- Leverages on YAMP function's declaration style pattern (and supports YAMP help)
- Allows StringValue and ScalarValue Key types
- Each element holds a Dictionary<String, Value>
- Each Set can have name attached to it
- Result Set's names are "concatenated" based on applied operations
- Only Element's key is compared during lifetime. Dictionary's values are always ignored on Compare/Insertion operations to preserve *nonrepeated* Set's rules.

Some Examples
-------------
    //Empty set (unsorted)
    a = newSet("A")
    =>
    {
        Name = A, Sorted = False
    }
<p>

    //Unsorted set with 3 elements
    b = newSet("B", 3, 2, 10)
    =>
    {
    Name = B, Sorted = False
      ID = 3 {
      }
      ID = 2 {
      }
      ID = 10 {
      }
    }
<p>

    //Sorted set with 3 elements
    c = newSortedSet("C", 5, 1, 3, 1)
    =>
    {
    Name = C, Sorted = True`
      ID = 1 {
      }
      ID = 3 {
      }
      ID = 5 {
      }
    }
<p>

    //Sets UNION (using MemberFunction)
    //note: New set preserves the (un/sorted) bit from the first set.
    d = b.SetUnion(c)
    =>
    {
    Name = (B+C), Sorted = False
      ID = 3 {
      }
      ID = 2 {
      }
      ID = 10 {
      }
      ID = 1 {
      }
      ID = 5 {
      }
    }
<p>

    //Sets UNION.
    //note: SetUnion allows multiple sets in one operation
    d = c.SetUnion(b, a)
    =>
    {
    Name = (C+B+A), Sorted = True
      ID = 1 {
      }
      ID = 2 {
      }
      ID = 3 {
      }
      ID = 5 {
      }
      ID = 10 {
      }
    }
<p>

    //Sets EXCEPT.
    //note: Using (static/normal style) Except function.
    e = Except(c, b)
    =>
    {
    Name = (C-B), Sorted = True
      ID = 1 {
      }
      ID = 5 {
      }
    }
<p>

    //Cascaded functions
    cc = c.SetUnion(a).SetUnion(b)
    =>
    {
    Name = ((C+A)+B), Sorted = True
      ID = 1 {
      }
      ID = 2 {
      }
      ID = 3 {
      }
      ID = 5 {
      }
      ID = 10 {
      }
    }
<p>

    //Cascaded functions, combining "@this" and "static"
    cc = except(c, b.SetUnion(a))
    =>
    {
    Name = (C-(B+A)), Sorted = True
      ID = 1 {
      }
      ID = 5 {
      }
    }
<p>

    //SetCount (number of elements)
    cc.SetCount()
    => 2

**More TDB**

Still missing
-------------
### more operations implementation
- set XExcept(a, b) - Symmetric Except-XOR
- bool set.Is[Proper]Subset(setb) - as .Net
- bool set.Is[Proper]Superset(setb) - as .Net
- bool set.Overlaps(setb) - as .Net
- bool set.SetEquals(setb) - as .Net
- bool set.SetContains(key) - Check if set constains one element (key)
- set set.Remove(key) - as .Net
- Element set.Element(key) - Returns the Element with "key" value
- ToRange() - returns a RangeValue, if all keys are of ScalarValue type)
- NewSet(name, rangevalue, isOrdered) - Creates a Set from a RangeValue
- set.AsSort() / set.AsUnsort() - Returns a copied Sorted/Unsorted set.
(...)

Operators implementation (check if possible with this operators / create new ones)
+ (union)
- (except)
& (intersect)
^ (XExcept)
== (SetEquals)
~= (~SetEquals)
[] (Element)

Problems/Known Issues (to be solved!)
-------------------------------------

This is **ok**

    10 + cc.SetCount()
    => 12

This **explodes**

    cc.SetCount() + 10
    => Exception: Stack empty.

This is **ok**

    (cc.SetCount()) + 10
    => 12


**More TDB**
