# How to make an interface internal ?

## question 
I am looking for ways to make an interface internal so that it does not appear in public interface of python SDK. This is the interface which emits `EvaluationResultsOperations` which shows up on client. I would like to generate it but keep it hidden from public interface.
 
I tried following:
1. Mark all operations under it internal
2. Adding @access decorator to interface but that fails.

Is there a way to achieve it ?

## answer
We do not support hiding an entire operation group directly. The recommended approach is to redefine your client in client.tsp to control which operations appear on the public client. When you do this properly, the default service clients and their Operations classes should not be emitted—as long as you remove all previously generated SDK output before regenerating. Or you can use _patch.py to customize the code.
