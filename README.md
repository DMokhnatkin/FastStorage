# FastStorage

Этот проект реализует C\# коллекцию, которая позволяет создавать индексы для входных данных и использовать их в LINQ to objects запросах к этой коллекции.

Например, пусть есть некоторые классы P и P2:

```CSharp
class P
{
    public int A { get; set; }

    public float B { get; set; }

    public P2 C { get; set; }
}

class P2
{
    public int D { get; set; }

    public int E { get; set; }
}
```
А также некоторая коллекция объектов этих классов:

```CSharp
private readonly P[] _data = new[]
{
    new P {A = 1, B = 5, C = new P2{ D = 1}},
    new P {A = 2, B = 4, C = new P2{ D = 2}},
    new P {A = 3, B = 3, C = new P2{ D = 3}},
    new P {A = 4, B = 2, C = new P2{ D = 4}},
    new P {A = 5, B = 1, C = new P2{ D = 5}}
};
```
Мы хотим выполнять LINQ to objects запросы к этим данным. 
В стандартной своей реализации LINQ to objects будет перебирать все элементы массива последовательно (O(n)). 
Эта библиотека добавляет возможность создания и использования индексов для этих данных:

```CSharp

var fastCollection = _data
    .AddIndex(x => x.A, new RedBlackTreeIndexFactory(), new HashTableIndexFactory())
    .AddIndex(x => x.B, new RedBlackTreeIndexFactory())
    .AddIndex(x => x.A + x.B, new RedBlackTreeIndexFactory())
    .Build();
```

После этого, при выполнении такого запроса для поиска/фильтрации будут использованы добавленные ранее индексы.
Таким образом, благодаря добавленным индексам на основе красно-черного дерева, сложность выполнения запроса будет O(log(n))

```CSharp
var t = fastCollection
    .Where(x => x.A + x.B < 10)
    .Where(x => x.A > 4)
    .Select(x => x.C)
    .Where(x => x.D > 2)
    .Select(x => x.E + x.D)
    .ToArray();
```
