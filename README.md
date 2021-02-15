# Rimworld-Library-ThreadUtility
A rimworld library that could make your mod run in a sub-thread


The usage:

Load this library before your own mod.

```
//Implement the interface named ITickThreaded
class ClassInSubThread : ThingComp, ITickThreaded
    {
        //This method will run in the sub-thread
        public void TickThreaded()
        {
            //Do something
        }
    }
```
