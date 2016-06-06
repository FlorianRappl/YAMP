Set (SetValue)
==============

Features
--------

- Supports Sorted and Hashed Sets
- Portable library compatibility
- Leverages on YAMP function's declaration style pattern (and supports YAMP help)
- "Object oriented" functionality (@this) on most functions
- "@this" functions (MemberFunctions) get object's name automatically prepended (to limit name collisions)
- Allows StringValue and ScalarValue Key types
- Each Set can have name attached to it
- Result Set's names are "concatenated" based on applied operations, and are of sorted or unsorted type based on first set argument

Note: SortedSet implementation is borrowed from Core FX (.NET Foundation)

Some Examples
-------------
    //Empty set (unsorted)
    a = newSet("A")
    =>
    {
        Name = A, Sorted = False, Count = 0
    }
<p>

    //Unsorted set with 3 elements
    b = newSet("B", 3, 2, 10)
    =>
    {
    Name = B, Sorted = False, Count = 3
      ID = 3
      ID = 2
      ID = 10
    }
<p>

    //Sorted set with 3 elements
    c = newSortedSet("C", 5, 1, 3, 1)
    =>
    {
    Name = C, Sorted = True, Count = 3
      ID = 1
      ID = 3
      ID = 5
    }
<p>

    //Sets UNION (using MemberFunction)
    //note: New set preserves the (un/sorted) bit from the first set.
    a = newSet("A"); b = newSet("B", 3, 2, 10); c = newSortedSet("C", 5, 1, 3, 1);
    d = b.SetUnion(c)
    =>
    {
    Name = (B+C), Sorted = False, Count = 5
      ID = 3
      ID = 2
      ID = 10
      ID = 1
      ID = 5
    }
<p>

    //Sets UNION.
    //note: SetUnion allows multiple sets in one operation
    a = newSet("A"); b = newSet("B", 3, 2, 10); c = newSortedSet("C", 5, 1, 3, 1);
    d = c.SetUnion(b, a)
    =>
    {
    Name = (C+B+A), Sorted = True, Count = 5
      ID = 1
      ID = 2
      ID = 3
      ID = 5
      ID = 10
    }
<p>

    //Sets EXCEPT.
    //note: Using (static/normal style) Except function.
    b = newSet("B", 3, 2, 10); c = newSortedSet("C", 5, 1, 3, 1);
    e = TExcept(c, b)
    =>
    {
    Name = (C-B), Sorted = True, Count = 2
      ID = 1
      ID = 5
    }
<p>

    //Cascaded functions
    a = newSet("A"); b = newSet("B", 3, 2, 10); c = newSortedSet("C", 5, 1, 3, 1);
    cc = c.SetUnion(a).SetUnion(b)
    =>
    {
    Name = ((C+A)+B), Sorted = True, Count = 5
      ID = 1
      ID = 2
      ID = 3
      ID = 5
      ID = 10
    }
<p>

    //Cascaded functions, combining "@this" and "static"
    a = newSet("A"); b = newSet("B", 3, 2, 10); c = newSortedSet("C", 5, 1, 3, 1);
    cc = TExcept(c, b.SetUnion(a))
    =>
    {
    Name = (C-(B+A)), Sorted = True, Count = 2
      ID = 1
      ID = 5
    }
<p>

    //SetCount (number of elements)
    a = newSet("A"); b = newSet("B", 3, 2, 10); c = newSortedSet("C", 5, 1, 3, 1);
    cc = TExcept(c, b.SetUnion(a));
    cc.SetCount()
    => 2

**More TDB**

Operators implementation
------------------------
    + (TUnion) 
    - (TExcept)
    & (TIntersect)
    ^ (TExceptXor)
    == (TEquals)
    ~= (!=) (not TEquals)

Normal Functions
----------------

- set NewRandomSet(name, maxsize, randomrange, sorted) - creates a new set with numeric values
- set NewSet(name, ...values) - creates a new **Unsorted** set with numeric or string values, or all elements of Matrixes
- set NewSortedSet(name, ...values) - creates a new **Sorted** set with numeric or string values, or all elements of Matrixes
- set TAdd(s1, id) - Adds the element to the Set, and returns the set. If Matrix, adds all elements
- set TAsSort(s1) - returns a Sorted copy
- set TAsUnSort(s1) - returns an Unsorted copy
- bool TContains(s1, id) - checks whether the set contains the given element's id
- int TCount(s1) - returns the number of elements in the Set
- bool TEquals(s1, s2) - compares two sets regarding Elements identifiers
- set TExcept(s1, ..., sN) - returns a new Set, with the first Set Except(ed) of the other Sets
- set TExceptXor(s1, ..., sN) - returns a new Set, with the Symmetric Except(XOR) of all Sets
- set TIntersect(s1, ..., sN) - returns a new Set, with the Intersection of all Sets
- bool TIsProperSubsetOf(s1, s2) - checks if s1 is a proper subset of s2
- bool TIsProperSupersetOf(s1, s2) - checks if s1 is a proper super of s2
- bool SetIsSorted(s1) - checks if set is of Sorted type
- bool TIsSubsetOf(s1, s2) - checks if s1 is a subset of s2
- bool TIsSupersetOf(s1, s2) - checks if s1 is a superset of s2
- bool TOverlaps(s1, s2) - checks whether the sets overlap over at least one common element
- set TRemove(s1, id) - Removes the specified element from the set.
- matrix TToMatrix(s1) - Creates a single row Matrix with all Numeric keys
- set TUnion(s1, ..., sN) - returns a new Set with the union of multiple Sets

Member Functions (applied to a Set object)
---------------------------------

- set s.SetAdd(id) - Adds the element to the Set, and returns the set. If Matrix, adds all its elements
- set s.SetAsSort() - returns a Sorted copy
- set s.SetAsUnSort() - returns an Unsorted copy
- bool s.SetContains(id) - checks whether the set contains the given element's id
- int s.SetCount() - returns the number of elements in the Set
- bool s.SetEquals(s2) - compares two sets regarding Elements identifiers
- set s.SetExcept(s2, ..., sN) - returns a new Set, Except(ed) of the given Sets
- set s.SetExceptXor(s2, ..., sN) - returns a new Set, with the Symmetric Except(XOR) of all Sets
- set s.SetIntersect(s2, ..., sN) - returns a new Set, with the Intersection of all Sets
- bool s.SetIsProperSubsetOf(s2) - checks if set is a proper subset of s2
- bool s.SetIsProperSupersetOf(s2) - checks if set is a proper superset of s2
- bool s.SetIsSorted() - checks if set is of Sorted type
- bool s.SetIsSubsetOf(s2) - checks if set is a subset of s2
- bool s.SetIsSupersetOf(s2) - checks if set is a superset of s2
- bool s.SetOverlaps(s2) - checks whether the sets overlap over at least one common element
- set s.SetRemove(id) - Removes the specified element from the Set
- matrix s.SetToMatrix() - Creates a single row Matrix with all Numeric keys
- set s.SetUnion(s2, ..., sN) - returns a new Set with the union of all Sets


Still missing
-------------
(...)


**More TDB**
