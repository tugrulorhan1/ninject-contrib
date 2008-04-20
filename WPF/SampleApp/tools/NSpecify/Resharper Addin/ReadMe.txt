ReSharper UnitTestSupport Add-in for NSpecify
==============================================================
The Unit test support plug-in for resharper will give you the ability to 
run your NSpecify specification with the ReSharper UnitRun 1.0 from JetBrains.

Installation
1. Close Visual Studio 2005. 
2. Extract the contents of archive, including the NSpecify folder, to: %ProgramFiles%\JetBrains\ReSharper\VS2005\Plugins 
4. Launch Visual Studio 2005. 
5. Open a project containing NSpecify specs. 
6. Open a spec's file containing a functionality with Specifications. Standard ReSharper icons will appear in the left 
   margin and allow you to run the specifications. 
   
Known Issues - James Kovacs (http://www.jameskovacs.com)

	ReSharper Unit Test Runner icons do not appear beside Functionality Fixture and Specification Methods. 
	-------------------------------------------------------------------------------------------------------
	This is typically caused by having an assembly reference to another unit test framework. ReSharper 
	Unit Test Runner (and UnitRun) only support a single unit test framework per test assembly. 
	Remove references to other test frameworks from the References in your spec project. This is a known 
	limitation in the JetBrains' current unit test runner implementation. A plugin votes on whether the 
	current assembly contains tests that it can run. If the plugin votes "yes", the unit test runner 
	stops querying additional plugins. NUnit and csUnit get queried before third-party plugins. 
   
For additional ReSharper test plug-in:
* MbUnit plug-in - http://der-albert.com/
* VstsUnit Plug-in - http://www.jameskovacs.com/