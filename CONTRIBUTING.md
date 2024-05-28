# Contributing

The main project fork lives [on GitHub](https://github.com/G-Research/ShapeSifter).

Please note that while we do accept contributions, this library is primarily built and maintained to serve the needs of G-Research, and our main priorities are to ensure that it continues to do so.
We provide the library in the hope that it will be useful, but we can't sink unlimited amounts of effort into its maintenance, so please forgive us if we choose not to accept some particular contribution you propose.
We will try to address issues and review incoming pull requests, but we do so on a [best-effort](https://en.wikipedia.org/wiki/Best-effort_delivery) basis.

In practice, this means that we are more likely to accept correctness fixes, but we may be more conservative about accepting new features, and we can't promise to take every contribution; and if you have encountered an issue which we ourselves do not encounter, we may or may not choose to fix it.
(For such problems, we are much more likely to review and accept a pull request that proposes a fix, than to create a fix from scratch.)

## Issues

Please raise bug reports and feature requests as Issues on [the main GitHub project](https://github.com/G-Research/ShapeSifter/issues).

## Pull requests

Before embarking on a large change, we strongly recommend checking via a GitHub Issue first that we are likely to accept it.

You may find that the following guidelines will help you produce a change that we accept:

* Keep your change as small and focused as is practical.
* Ensure that your change is thoroughly tested.
* Document any choices you make which are not immediately obvious.
* Explain why your change is necessary or desirable.

## On your first checkout

There are pull request checks on this repo, enforcing [Fantomas](https://github.com/fsprojects/fantomas/)-compliant formatting.
After checking out the repo, you may wish to add a pre-push hook to ensure locally that formatting is complete, rather than having to wait for the CI checks to tell you that you haven't formatted your code.
Consider performing the following command to set this up in the repo:
```bash
git config core.hooksPath hooks/
```
Before your first push (but only once), you will need to install the [.NET local tools](https://docs.microsoft.com/en-us/dotnet/core/tools/local-tools-how-to-use) which form part of the pre-push hook:
```bash
dotnet tool restore
```

## Dependencies

For maximum compatibility, this project targets the earliest version of `FSharp.Core` that is practical (see the [Notes and Guidance on FSharp.Core](https://fsharp.github.io/fsharp-compiler-docs/fsharp-core-notes.html) for more details).
It targets `netstandard2.0` so that it can be used in the legacy .NET Framework.

We try to keep `ShapeSifter`'s dependency footprint small.

## Branch strategy

Releases are made from the `main` branch.

## License

This project is licensed with the MIT license, a copy of which you can find at the repository root.

