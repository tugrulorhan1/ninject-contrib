This project provides a drop-in replacement for the UnityBootstrapper that
ships with the Composite Application Guidance for WPF project
(http://www.codeplex.com/CompositeWPF)

This allows you to load your own Ninject modules and bindings in your
custom Composite WPF modules.

To use this in your Composite application, just replace the UnityBootstrapper
with the provided NinjectBootstrapper:

namespace HelloWorldSample
{
    //internal class Bootstrapper : UnityBootstrapper
    internal class Bootstrapper : NinjectBootstrapper

// ...

If you have any references to any other DI containers in your modules
(as the StockTrader sample in Composite WPF has), then these will
need to be replaced with a reference to Ninject's IKernel and the
relevant registration/retrieval methods will need to be converted
accordingly.

If you have any questions, feel free to ask me at:

michael.hart.au@gmail.com

Cheers,

Michael Hart