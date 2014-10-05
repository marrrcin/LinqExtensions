LinqExtensions
==============

Some useful LINQ extensions like IComparer(T) and IEqualityComparer(T) lambda wrappers.

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
