# Assigning out and ref parameters

Sometimes methods have `out` or `ref` parameters that need to be
filled in when the faked method is called. Use
`AssignsOutAndRefParameters`:

```csharp
A.CallTo(() => aFake.AMethod(anInt, ref aRef, out anOut))
 .Returns(true)
 .AssignsOutAndRefParameters("new aRef value", "new anOut value");
```

`AssignsOutAndRefParameters` takes a `params object[]`, with one
element (in order) for each of the `out` and `ref` parameters in the
call being faked - the other parameters are omitted.

While assigning `out` and `ref` parameters, the `Returns` method (or
[some variant](specifying-return-values.md)) is often used to specify
the return value for a non-void method - `AssignsOutAndRefParameters`
does not do this on its own. If `AssignsOutAndRefParameters` is used
without a `Returns`, the return value will be a [Dummy](dummies.md).
When both `Returns` and `AssignsOutAndRefParameters` are used,
`Returns` must be specified first.

## Assigning Values Calculated at Call Time

When `out` or `ref` parameter values aren't known until the method is
called, `AssignsOutAndRefParametersLazily` can be used.

```csharp
string theValue;
A.CallTo(() => aFake.AMethod(anInt, ref aRef, out anOut))
 .Returns(true)
 .AssignsOutAndRefParametersLazily((int someInt, string someRef, string someOut) =>
     new[] { "new aRef value: " + someInt, "new anOut value" });
```

As shown above, the inputs to the method may be used to calculate the
values to assign. Convenient overloads exist for methods of up to 8
parameters.

The type of the `Func` sent to `AssignsOutAndRefParametersLazily`
isn't checked at compile time, but any _type_ mismatch will trigger
a helpful error message.

If more advanced decision-making is required, or the method has more
than 8 parameters, the convenience methods won't work. Use the variant
that takes an `IFakeObjectCall` instead:

```csharp
string theValue;
A.CallTo(() => aFake.AMethod(anInt, ref aRef, out anOut))
 .Returns(true)
 .AssignsOutAndRefParametersLazily(call => CalculateValuesFrom(call));
```
The `IFakeObjectCall` object provides access to

* information about the `Method` being called, as a `MethodInfo`,
* the `Arguments`, accessed by position or name, and
* the original `FakedObject`

## Implicitly Assigning `out` Parameter Values

Any `Expression`-based `A.CallTo` configuration that's made on a
method that has an `out` parameter will cause the value of the variable
used in the `A.CallTo` to be assigned to the `out` parameter when the
method is actually called. For example:

```csharp
string configurationValue = "lollipop";
A.CallTo(() => aFakeDictionary.TryGetValue(theKey, out configurationValue))
 .Returns(true); 

string fetchedValue;
aFakeDictionary.TryGetValue(theKey, out fetchedValue);

// fetchedValue is now "lollipop";
```

If this behavior is not desired, `AssignsOutAndRefParameters` (or `…Lazily`) can be used to provide different behavior.
