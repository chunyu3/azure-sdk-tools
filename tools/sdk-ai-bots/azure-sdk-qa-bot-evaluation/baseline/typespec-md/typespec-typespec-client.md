# How to make an interface internal ?

## question 
I am looking for ways to make an interface internal so that it does not appear in public interface of python SDK. This is the interface which emits `EvaluationResultsOperations` which shows up on client. I would like to generate it but keep it hidden from public interface.
 
I tried following:
1. Mark all operations under it internal
2. Adding @access decorator to interface but that fails.

Is there a way to achieve it ?

## answer
If your goal is to **hide an operations interface from the public Python SDK surface**, here's the key takeaway:

> **We currently do not support hiding an entire operation group directly.**

However, there are **two main approaches** to achieve your intent:

1. **Use `client.tsp` to redefine your client** and control which operations appear on the public client. When you redefine the client structure this way, **default service clients (and their `Operations` classes) should not be emitted**—as long as there’s no leftover generated code from a previous run. Make sure to:

   * Remove all previously generated SDK output before regenerating.
   * Verify your custom client structure is complete.

2. **Use `_patch.py` to customize visibility**, e.g., by renaming `client.evaluation_results` to `client._evaluation_results`, if needed. This is a workaround until more flexible TypeSpec features are available.

> Also, note that the `@access` decorator doesn’t work on interfaces right now—it’s not interpreted that way by the client generator (tcgc).

So, the current best practice is to **redefine the client properly in `client.tsp`**, clean your generated code, and regenerate to reflect the changes accurately.
