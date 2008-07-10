This project adds some support for the runtime registration of interceptors
as well as some .NET 3.5 extensions to make it easier to register
methods/properties for interception.

The one line example:

// Before finding the customer, print a debugging statement
InterceptBefore<CustomerService>(cs => cs.Find(),
    i => Debug.WriteLine("Finding Customer"));

The longer version: There's a class CustomerService that has a Find method
and we want to write out a debugging statement every time Find is called.
Firstly, let's create a module that registers the interception:

using Ninject.Core;
// import the helper extension methods
using NinjectContrib.Interception;

public class CustomerServiceModule : StandardModule
{
    public override void Load()
    {
        // Just assume the CustomerService is registered
        Bind<CustomerService>().ToSelf();
        
        // Use the extension methods (hence the "this.")
        this.InterceptBefore<CustomerService>(cs => cs.Find(),
            i => Debug.WriteLine("Finding Customer"));
    }
}

This basically says, intercept the Find method on CustomerService
before it's invoked and call Debug.WriteLine. The Find method will
then proceed as normal.

To be activated, this module will need to be loaded into the Kernel,
along with one of the ProxyFactory modules (LinFu or DynamicProxy2)
as well as the MethodInterceptorModule from this project:

var kernel = new StandardKernel(
    new LinFuModule(),
    new MethodInterceptorModule(),
    new CustomerServiceModule()
);

var customerService = kernel.Get<CustomerService>();
var customer = customerService.Find();
// the debugging statement would be printed,
// regardless of what happens in Find()

If the Find() method had parameters, then they can be specified
in the registration however you like (the actual values are not
used). For example if the signature of Find was:

public Customer Find(string name)

then you could register an interceptor like this:

InterceptBefore<CustomerService>(cs => cs.Find(""),
    i => Debug.WriteLine("Finding Customer"));
    
If you find that using actual values confuses the intent of the
code, then you could just use the default keyword:

InterceptBefore<CustomerService>(cs => cs.Find(default(string)),
    i => Debug.WriteLine("Finding Customer"));

As well as methods, property getters/setters can also be intercepted:

InterceptBeforeGet<CustomerService>(cs => cs.Name,
    i => Debug.WriteLine("CustomerService.Name was retrieved"));
    
The extension methods give access to the standard Ninject
IInvocation, so you can access (or modify) the arguments
of the method, and the return value as well:

// Will find Customer "Bob", regardless of
// the name argument passed in to Find()
InterceptBefore<CustomerService>(cs => cs.Find(default(string)),
    i => i.Request.Arguments[0] = "Bob");

// Modifies the CustomerService name property
InterceptAfterGet<CustomerService>(cs => cs.Name,
    i => i.ReturnValue = "Changed Name");
    
Have a look in the NinjectContrib.Interception.Tests project
for more examples and sample code.

Here's the full list of extension methods provided:

InterceptReplace: Replaces the whole method invocation
InterceptBefore: Is called before the method proceeds
InterceptAfter: Is called after the method has proceeded
InterceptAround: Can specify a before and after action
InterceptReplaceGet: Replaces the property getter
InterceptReplaceSet: Replaces the property setter
InterceptBeforeGet: Is called before the property getter
InterceptBeforeSet: Is called before the property setter
InterceptAfterGet: Is called after the property getter
InterceptAfterSet: Is called after the property setter
InterceptAroundGet: ... you get the idea...
InterceptAroundSet: ... ditto


Any questions, feel free to email me.

Michael Hart
michael.hart.au@gmail.com