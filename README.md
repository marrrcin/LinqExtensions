LinqExtensions
==============

Some useful LINQ extensions like IComparer(T) and IEqualityComparer(T) lambda wrappers, linq permutations, linq merge, linq flatten.

## Linq Shuffle
You can use IEnumerable.Shuffle() to shuffle arrays etc. For example:<br/>
```csharp
var numbers = Enumerable.Range(0, 15); 
string output = "";
numbers.Shuffle().ToList().ForEach(item =>
{
    output += item.ToString() + " ";
});
Console.WriteLine(output);
```
<br/>Will result in something like:
<code>14 3 5 6 4 13 2 7 8 9 10 11 0 1 12</code>
## Linq OrderBy Lambda Comparer
You can use IEnumerable.OrderBy and OrderByDescending with two lambda expressions instead of creating new instance of IComparer, for example:
```csharp
numbers.OrderBy(i=>i,(i1,i2)=>i1-i2)
```
First argument is keySelector, second is the comparer function.
## Linq Distinct Lambda Equality Comparer
You can use IEnumerable.Distinct with one or two lambda expressions. First expression is a equality comparer function, second is GetHashCode function. If you skip second expression, the default (i=>1) will be used, and therefore you can use only comparer function like in the example bellow.<br/>
```csharp
var something = new string[]
{
    "John",
    "JoHn",
    "Jack",
    "JACK",
    "Lisbeth",
    "Vincent",
    "VINcent"
};
string output = "";
something.Distinct((a,b)=>a.ToLower()==b.ToLower()).ToList().ForEach(item =>
{
    output += item.ToString() + " ";
});
Console.WriteLine(output);
```
Above will output:
```
John Jack Lisbeth Vincent
```

## Linq Permutations
You can use IEnumerable.Permutations to lazy-generate permutations of your arrays etc. For example:
```csharp
foreach (var permutation in Enumerable.Range(1,4).Permutations())
{
    string output = "";
    permutation.ToList().ForEach(item =>
    {
        output += item.ToString() + " ";
    });
    Console.WriteLine(output);
}
```
Will result in:
```
1 2 3 4 
1 2 4 3 
1 3 2 4 
1 3 4 2 
1 4 2 3 
1 4 3 2 
2 1 3 4 
2 1 4 3 
2 3 1 4 
2 3 4 1 
2 4 1 3 
2 4 3 1 
3 1 2 4 
3 1 4 2 
3 2 1 4 
3 2 4 1 
3 4 1 2 
3 4 2 1 
4 1 2 3 
4 1 3 2 
4 2 1 3 
4 2 3 1 
4 3 1 2 
4 3 2 1 
```
You can also provide IEqualityComparer lambda and get permutations of any objects (this is a non-creative example):
```csharp
var array = new dynamic[3];
for (int i = 0; i < 3; i++)
{
    dynamic something = new { letter = (char)(65 + i) };
    array[i] = something;
}

foreach (var permutation in array.Permutations((o, o1) => o.letter == o1.letter))
{
    string output = "";
    permutation.ToList().ForEach(item =>
    {
        output += item.letter.ToString() + " ";
    });
    Console.WriteLine(output);
}
```
And the result will be:
```
A B C 
A C B 
B A C 
B C A 
C A B 
C B A 
```

## Linq Flatten
You can use IEnumerable.Flatten, to flatten deeply nested collections. There are two implementations - recursive and non recursive.
Example:
```
var array = new dynamic[]
{
    new[] {"lol", "xyz", "abc"},
    new dynamic[] {"lol2", new dynamic[] {"nested1", new[] {"nested2","nested2-other"}}},
    "trolo"
};

var result = array.Flatten<string>();
//or non recursive implementation:
result = array.Flatten<string>(false);
```
Result:
```
    [0]: "lol"
    [1]: "xyz"
    [2]: "abc"
    [3]: "lol2"
    [4]: "nested1"
    [5]: "nested2"
    [6]: "nested2-other"
    [7]: "trolo"
```

## Linq Merge
You can use 
```
LinqCollections.Merge<T>(Func<T,T,int> compare,params IList<T>[] toMerge)
```
to merge any number of lists (they should be sorted). There is an additional overload which can remove duplicates:
```
LinqCollections.Merge<T>(Func<T,T,int> compare,true,params IList<T>[] toMerge)
```
Example 1:
```
var array1 = Enumerable.Range(0, 15).Where(i=>i%3==0).ToList();
var array2 = Enumerable.Range(0, 15).Where(i=>i%3==1).ToList();
var array3 = Enumerable.Range(0, 15).Where(i=>i%3==2).ToList();

var result = LinqCollections.Merge((a, b) => a - b, array1, array2,array3);
```
Result 1:
```
    [0]: 0
    [1]: 0
    [2]: 0
    [3]: 0
    [4]: 0
    [5]: 1
    [6]: 1
    [7]: 1
    [8]: 1
    [9]: 1
    [10]: 2
    [11]: 2
    [12]: 2
    [13]: 2
    [14]: 2
```

Example 2 (remove duplicates):
```
var array1 = Enumerable.Range(0, 1000).Select(i => 2).ToList();
var array2 = Enumerable.Range(0, 1000).Select(i => 1).ToList();

var result = LinqCollections.Merge((a, b) => a - b,true, array1, array2);
```
Result 2:
```
    [0]: 1
    [1]: 2
```

## Linq Minimum with Index
You can use IEnumerable.MinWithIndex to get Tuple of your Type & Index of minimum element from Enumerable.
